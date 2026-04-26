using Inventario.Application.Common.Attributes;

namespace Inventario.Application.DTOs
{
    public class UsuarioImportDto
    {
        [ExcelColumn("documento_identidad")]
        public string DocumentoIdentidad { get; set; }
        [ExcelColumn("nombre_completo")]
        public string NombreCompleto { get; set; }
        [ExcelColumn("correo")]
        public string Correo { get; set; }
        [ExcelColumn("celular")]
        public string Celular { get; set; }
        [ExcelColumn("area")]
        public string Area { get; set; }
        [ExcelColumn("puesto")]
        public string Puesto { get; set; }
    }
}
