using Inventario.Application.Commands.Activos.Import;
using Inventario.Application.DTOs;
using Inventario.Application.Interfaces.Services;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Usuarios.Import
{
    internal sealed class ImportUsuariosCommandHandler(
        IExcelImportService excelService,
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<ImportUsuariosCommand, ImportResult>
    {
        public async Task<ImportResult> Handle(ImportUsuariosCommand request, CancellationToken cancellationToken)
        {
            List<UsuarioImportDto> items = excelService
                .Import<UsuarioImportDto>(request.FileStream)
                .ToList();

            if (items.Count == 0)
                return new ImportResult(true, 0, 0);

            List<(int RowIndex, string Motivo)> erroresReporte = new();
            List<Usuario> nuevosUsuarios = new();

            // =========================
            // 1. PRE-CARGAS Y SETS DE CONTROL
            // =========================

            // Para validar contra la BD (lo que ya existe antes de empezar)
            List<string> documentosExcel = items
                .Select(x => x.DocumentoIdentidad?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.ToUpperInvariant())
                .Distinct()
                .ToList();

            var documentosExistentesEnBD = await usuarioRepository
                .GetByDocumentNbrListAsync(documentosExcel, cancellationToken);

            var documentosBDSet = documentosExistentesEnBD
                .Select(x => x.DocumentoIdentidad)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Para controlar duplicados DENTRO del mismo Excel en tiempo real
            HashSet<string> documentosProcesadosEnBucle = new(StringComparer.OrdinalIgnoreCase);
            HashSet<string> nombresProcesadosEnBucle = new(StringComparer.OrdinalIgnoreCase);

            // =========================
            // 2. PROCESO PRINCIPAL
            // =========================

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                int fila = i + 2; // +2 porque el Excel empieza en 1 y tiene cabecera

                string? docKey = item.DocumentoIdentidad?.Trim()?.ToUpperInvariant();
                string? nombreKey = item.NombreCompleto?.Trim()?.ToUpperInvariant();

                // --- VALIDACIÓN DE NOMBRE (OBLIGATORIO) ---
                if (string.IsNullOrWhiteSpace(nombreKey))
                {
                    erroresReporte.Add((fila, "El campo nombre_completo es obligatorio"));
                    continue;
                }

                if (nombresProcesadosEnBucle.Contains(nombreKey))
                {
                    erroresReporte.Add((fila, $"Nombre duplicado dentro del archivo ({nombreKey})"));
                    continue;
                }

                // --- VALIDACIÓN DE DOCUMENTO ---
                if (!string.IsNullOrWhiteSpace(docKey))
                {
                    // ¿Duplicado en el mismo Excel?
                    if (documentosProcesadosEnBucle.Contains(docKey))
                    {
                        erroresReporte.Add((fila, $"Documento duplicado dentro del archivo ({docKey})"));
                        continue;
                    }

                    // ¿Ya existe en la BD?
                    if (documentosBDSet.Contains(docKey))
                    {
                        erroresReporte.Add((fila, $"Documento ya existe en el sistema ({docKey})"));
                        continue;
                    }
                }

                // --- VALIDACIÓN DE CORREO ---
                if (!string.IsNullOrWhiteSpace(item.Correo) && !EsCorreoValido(item.Correo))
                {
                    erroresReporte.Add((fila, $"Correo inválido ({item.Correo})"));
                    continue;
                }

                // --- VALIDACIÓN DE CELULAR ---
                if (!string.IsNullOrWhiteSpace(item.Celular) && !EsCelularValido(item.Celular))
                {
                    erroresReporte.Add((fila, $"Celular inválido ({item.Celular})"));
                    continue;
                }

                // --- SI LLEGÓ AQUÍ, ES VÁLIDO ---
                nuevosUsuarios.Add(Usuario.Create(
                    nombreKey,
                    docKey,
                    item.Correo,
                    item.Celular,
                    item.Area,
                    item.Puesto
                ));

                // Registrar en los sets para que la siguiente fila detecte si es duplicada
                nombresProcesadosEnBucle.Add(nombreKey);
                if (!string.IsNullOrWhiteSpace(docKey))
                    documentosProcesadosEnBucle.Add(docKey);
            }

            // =========================
            // 3. INSERCIÓN DE REGISTROS VÁLIDOS
            // =========================
            if (nuevosUsuarios.Count > 0)
            {
                usuarioRepository.AddRange(nuevosUsuarios);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // =========================
            // 4. RESPUESTA Y ERRORES
            // =========================

            // Si hubo errores, generamos el archivo de reporte pero informamos cuántos SÍ se insertaron
            if (erroresReporte.Count > 0)
            {
                byte[] fileError = excelService
                    .GenerateErrorReport<UsuarioImportDto>(request.FileStream, erroresReporte);

                // ImportResult: Success (false porque hubo errores), Insertados, Fallidos, Excel
                return new ImportResult(false, nuevosUsuarios.Count, erroresReporte.Count, fileError);
            }

            // Si todo fue perfecto
            return new ImportResult(true, nuevosUsuarios.Count, 0);
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(correo);
                return addr.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        private bool EsCelularValido(string celular)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(celular, @"^\d{9,15}$");
        }
    }
}