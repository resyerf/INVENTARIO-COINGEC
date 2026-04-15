namespace Inventario.Domain.ValueObjects
{
    public record CategoryType
    {
        public string Value { get; }

        // Definimos las constantes según tu Excel
        public static readonly CategoryType Ecom = new("ECOM");
        public static readonly CategoryType Gen = new("EGEN");
        public static readonly CategoryType Herp = new("HERP");
        public static readonly CategoryType Sism = new("SISM");
        public static readonly CategoryType Egeof = new("EGEOF");
        public static readonly CategoryType Ageof = new("AGEOF");

        private CategoryType(string value) => Value = value;

        public static CategoryType FromString(string value)
        {
            // Aquí podrías validar que el string pertenezca a tu lista oficial
            return new CategoryType(value.ToUpper());
        }
    }
}