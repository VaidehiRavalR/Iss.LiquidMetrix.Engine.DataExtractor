using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataExtractor.Utils
{
    /// <summary>  
    /// Class <code>DataExtractorHelper</code>
    ///  is a helper class containing common functionalty that will be used across solution
    /// </summary>
    public static class DataExtractorHelper
    {
        public static Task ConvertToCsv(string filepath, string csvFilePath, char separator = ';')
        {
            var workBook = new XLWorkbook(filepath);
            var workSheet = workBook.Worksheets.First();
            var lastCellAddress = workBook.Worksheets.First().RangeUsed().LastCell().Address;


            File.WriteAllLines(csvFilePath, workSheet.Rows(1, lastCellAddress.RowNumber)
              .Select(r => string.Join(separator, r.Cells(1, lastCellAddress.ColumnNumber)
                  .Select(cell =>
                  {
                      var cellValue = cell.GetValue<string>();
                      return cellValue.Contains(separator) ? $"\"{cellValue}\"" : cellValue;

                  }))));

            return Task.CompletedTask;
        }
    }
}
