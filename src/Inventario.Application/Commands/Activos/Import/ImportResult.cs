namespace Inventario.Application.Commands.Activos.Import
{
    public record ImportResult(bool Success, int Procesados, int Errores, byte[]? ErrorFile = null);
}
