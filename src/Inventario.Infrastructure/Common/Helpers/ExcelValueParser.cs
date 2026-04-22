namespace Inventario.Infrastructure.Common.Helpers
{
    public static class ExcelValueParser
    {
        public static string GetString(string value)
            => value?.Trim() ?? string.Empty;

        public static int GetInt(string value)
            => int.TryParse(value, out var n) ? n : 0;

        public static decimal? GetDecimal(string value)
            => decimal.TryParse(value, out var d) ? d : null;

        public static DateTime? GetDate(string value)
            => DateTime.TryParse(value, out var dt) ? dt : null;

        public static object? ConvertValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
                return value;

            if (targetType == typeof(int) || targetType == typeof(int?))
                return int.TryParse(value, out var i) ? i : 0;

            if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                return decimal.TryParse(value, out var d) ? d : null;

            if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                return DateTime.TryParse(value, out var dt) ? dt : null;

            return null;
        }
    }
}
