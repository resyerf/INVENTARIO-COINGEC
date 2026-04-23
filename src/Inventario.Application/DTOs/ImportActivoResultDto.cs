namespace Inventario.Application.DTOs
{
    public sealed class ImportActivoResultDto
    {
        public int Insertados { get; set; }
        public int Errores { get; set; }
        public byte[]? ArchivoErrores { get; set; }
    }
}
