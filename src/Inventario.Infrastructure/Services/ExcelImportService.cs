using ClosedXML.Excel;
using Inventario.Application.Common.Attributes;
using Inventario.Application.Interfaces.Services;
using Inventario.Infrastructure.Common.Helpers;
using System.Reflection;

namespace Inventario.Infrastructure.Services
{
    public class ExcelImportService : IExcelImportService
    {
        public IReadOnlyList<T> Import<T>(Stream fileStream, Func<IReadOnlyDictionary<string, string>, T> map)
        {
            var rows = ImportRaw(fileStream);
            return rows.Select(map).ToList();
        }
        public IReadOnlyList<T> Import<T>(Stream fileStream) where T : new()
        {
            var rows = ImportRaw(fileStream);
            var result = new List<T>();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var row in rows)
            {
                var item = new T();

                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttribute<ExcelColumnAttribute>();
                    if (attr == null) continue;

                    if (!row.TryGetValue(Normalize(attr.Name), out var value))
                        continue;

                    object? parsedValue = ExcelValueParser.ConvertValue(value, prop.PropertyType);
                    prop.SetValue(item, parsedValue);
                }

                result.Add(item);
            }

            return result;
        }

        public IReadOnlyList<IReadOnlyDictionary<string, string>> ImportRaw(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1);

            var result = new List<IReadOnlyDictionary<string, string>>();

            var headerRow = worksheet.Row(1);

            var headers = headerRow.CellsUsed()
                .Select((c, i) => new
                {
                    Name = Normalize(c.GetString()),
                    Index = i + 1
                })
                .ToList();

            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var dict = new Dictionary<string, string>();

                foreach (var header in headers)
                {
                    var value = row.Cell(header.Index).GetString();
                    dict[header.Name] = Normalize(value);
                }

                result.Add(dict);
            }

            return result;
        }

        public byte[] GenerateErrorReport<T>(Stream originalStream, List<(int RowIndex, string Motivo)> errores) where T : class
        {
            // Reiniciamos la posición del stream por si fue leído previamente
            if (originalStream.CanSeek) originalStream.Position = 0;

            using var workbook = new XLWorkbook(originalStream);
            var worksheet = workbook.Worksheet(1);

            // 1. Identificar la última columna para poner el mensaje de error
            var lastColumnUsed = worksheet.LastColumnUsed().ColumnNumber();
            var errorColumn = lastColumnUsed + 1;

            // Header de error
            var headerCell = worksheet.Cell(1, errorColumn);
            headerCell.Value = "MOTIVO_ERROR";
            headerCell.Style.Font.SetBold().Font.SetFontColor(XLColor.White);
            headerCell.Style.Fill.SetBackgroundColor(XLColor.DarkRed);

            // 2. Procesar los errores
            foreach (var (rowIndex, motivo) in errores)
            {
                var row = worksheet.Row(rowIndex);

                // Resaltar toda la fila en un rojo claro (Misty Rose)
                row.Style.Fill.SetBackgroundColor(XLColor.MistyRose);

                // Escribir el motivo del error en la última columna
                var cellError = worksheet.Cell(rowIndex, errorColumn);
                cellError.Value = motivo;
                cellError.Style.Font.SetFontColor(XLColor.Red).Font.SetBold();

                // Borde opcional para resaltar la celda de error
                cellError.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                cellError.Style.Border.SetOutsideBorderColor(XLColor.Red);
            }

            worksheet.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        private static string Normalize(string? value)
            => value?.Trim().ToUpperInvariant() ?? string.Empty;

    }
}
