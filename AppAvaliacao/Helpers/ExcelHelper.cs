using FileUpload.PickFile;
using ClosedXML.Excel;

namespace AppAvaliacao.Helpers;

public class ExcelHelper
{
    public async Task<string[,]> ExcelReader(string path = null)
    {
        if (path is null)
        {
            var fileResult = await PickerFileHelper.PickerImage();
            path = fileResult.FullPath;
        }

        using var fileStream = new FileStream(path, FileMode.Open);
        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(2);
        var colMax = worksheet.ColumnsUsed().Count();
        var rowMax = worksheet.RowsUsed().Count();

        string[,] data = new string[rowMax - 1, colMax];

        for (int row = 2; row <= rowMax; row++)
        {
            for (int col = 1; col <= colMax; col++)
            {
                data[row - 2, col - 1] = worksheet.Cell(row, col).GetValue<string>();
            }
        }

        return data;
    }
}
