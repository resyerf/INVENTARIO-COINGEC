namespace Inventario.Application.Interfaces.Services
{
    public interface IExcelImportService
    {
        IReadOnlyList<T> Import<T>(Stream fileStream, Func<IReadOnlyDictionary<string, string>, T> map);
        IReadOnlyList<T> Import<T>(Stream fileStream) where T : new();
        IReadOnlyList<IReadOnlyDictionary<string, string>> ImportRaw(Stream fileStream);
    }
}
