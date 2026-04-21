namespace Inventario.Application.Interfaces.Services
{
    public interface IExcelExportService
    {
        byte[] Export<T>(IEnumerable<T> data, string sheetName, Dictionary<string, Func<T, object?>> columnMapping);
    }
}
