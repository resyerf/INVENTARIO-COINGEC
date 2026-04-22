using Inventario.Application.Common.Attributes;

namespace Inventario.Application.DTOs
{
    public class ActivoImportDto
    {
        [ExcelColumn("EQUIPO / HERRAMIENTA")]
        public string NombreEquipo { get; set; }

        [ExcelColumn("ETIQUETADO")]
        public string Etiquetado { get; set; }

        [ExcelColumn("MARCA")]
        public string Marca { get; set; }

        [ExcelColumn("MODELO")]
        public string Modelo { get; set; }

        [ExcelColumn("SERIE")]
        public string Serie { get; set; }

        [ExcelColumn("CLASIFICACION POR TIPO")]
        public string Tipo { get; set; }

        [ExcelColumn("CODIGO")]
        public string CodigoEquipo { get; set; }

        [ExcelColumn("USUARIO")]
        public string Usuario { get; set; }

        [ExcelColumn("FECHA DE ADQUISICION")]
        public DateTime? FechaAdquisicion { get; set; }

        [ExcelColumn("CANTIDAD")]
        public int Cantidad { get; set; }

        [ExcelColumn("SOBRA")]
        public string Sobra { get; set; } // opcional (no lo usas en API)

        [ExcelColumn("ESTADO / CONDICION")]
        public string Estado { get; set; } // opcional

        [ExcelColumn("FECHA DE MANTENIMIENTO")]
        public DateTime? FechaMantenimiento { get; set; }

        [ExcelColumn("FECHA DE CALIBRACION")]
        public DateTime? FechaCalibracion { get; set; }

        [ExcelColumn("LUGAR DE ALMACENAMIENTO Y/O COMPRA")]
        public string Ubicacion { get; set; }

        [ExcelColumn("COSTO UNITARIO")]
        public decimal? CostoUnitario { get; set; }

        [ExcelColumn("COSTO TOTAL")]
        public decimal? CostoTotal { get; set; } // opcional (derivable)
    }
}
