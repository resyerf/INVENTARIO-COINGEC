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

        private static string Clean(string? value)
        {
            return value?.Trim().ToUpperInvariant() ?? string.Empty;
        }
        private static string Normalize(string? value)
            => value?.Trim().ToUpperInvariant() ?? string.Empty;

    }
}
