using ClosedXML.Excel;
using Inventario.Application.Interfaces.Services;

namespace Inventario.Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] Export<T>(IEnumerable<T> data, string sheetName, Dictionary<string, Func<T, object?>> columnMapping)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Add Headers
            var headers = columnMapping.Keys.ToList();
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }

            // Format Headers
            var headerRange = worksheet.Range(1, 1, 1, headers.Count);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.AirForceBlue;
            headerRange.Style.Font.FontColor = XLColor.White;

            // Fill Data
            var dataList = data.ToList();
            for (int row = 0; row < dataList.Count; row++)
            {
                var item = dataList[row];
                for (int col = 0; col < headers.Count; col++)
                {
                    var headerKey = headers[col];
                    var value = columnMapping[headerKey](item);
                    
                    if (value != null)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = XLCellValue.FromObject(value);
                    }
                }
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
