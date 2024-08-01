using ClosedXML.Excel;
using Models.Movies;

namespace AppAvaliacao.Helpers.ExcelHerper;

public class DownloadExcelHelper
{
    public Stream DownloadExcel(List<Assessments> assessments)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Assessments");

        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Assessment";
        worksheet.Cell(1, 4).Value = "Director";
        worksheet.Cell(1, 5).Value = "ImagePath";
        worksheet.Cell(1, 6).Value = "Gender";
        worksheet.Cell(1, 7).Value = "Duration";
        worksheet.Cell(1, 8).Value = "Position";
        worksheet.Cell(1, 9).Value = "Concluded";
        worksheet.Cell(1, 10).Value = "Comments";
        worksheet.Cell(1, 11).Value = "Category";
        worksheet.Cell(1, 12).Value = "LastUpdate";
        worksheet.Cell(1, 13).Value = "Launch";

        int row = 2;
        foreach (var assessment in assessments)
        {
            worksheet.Cell(row, 1).Value = assessment.Id;
            worksheet.Cell(row, 2).Value = assessment.Name;
            worksheet.Cell(row, 3).Value = assessment.Assessment;
            worksheet.Cell(row, 4).Value = assessment.Director;
            worksheet.Cell(row, 5).Value = assessment.ImagePath;
            worksheet.Cell(row, 6).Value = assessment.Gender;
            worksheet.Cell(row, 7).Value = assessment.Duration;
            worksheet.Cell(row, 8).Value = assessment.Position;
            worksheet.Cell(row, 9).Value = assessment.Concluded;
            worksheet.Cell(row, 10).Value = assessment.Comments;
            worksheet.Cell(row, 11).Value = assessment.Category;
            worksheet.Cell(row, 12).Value = assessment.LastUpdate;
            worksheet.Cell(row, 13).Value = assessment.Launch;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var array = new MemoryStream(stream.ToArray());
        return array;
    }
}
