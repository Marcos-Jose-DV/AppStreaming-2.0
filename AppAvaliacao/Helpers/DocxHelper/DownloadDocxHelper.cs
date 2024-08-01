using Xceed.Words.NET;
using Models.Movies;
namespace AppAvaliacao.Helpers.DownloadDocx;

public class DownloadDocxHelper
{
    public Stream? DownloadDocx(IEnumerable<Assessments> assessments)
    {
        using MemoryStream stream = new();
        using DocX docx = DocX.Create(stream);
        Xceed.Document.NET.Paragraph paragraph = docx.InsertParagraph();

        foreach (var assessment in assessments)
        {
            paragraph.Append(
                $"Id: {assessment.Id}\n" +
                $"Name: {assessment.Name}\n" +
                $"Assessment: {assessment.Assessment}\n" +
                $"Director: {assessment.Director}\n" +
                $"ImagePath: {assessment.ImagePath}\n" +
                $"Gender: {assessment.Gender}\n" +
                $"Position: {assessment.Duration}\n" +
                $"Concluded: {assessment.Concluded}\n" +
                $"Comments: {assessment.Comments}\n" +
                $"Category: {assessment.Category}\n" +
                $"LastUpdate: {assessment.LastUpdate}\n" +
                $"Launch: {assessment.Launch}\n\n");

            paragraph.AppendLine();
        }
        docx.Save();
        var array = new MemoryStream(stream.ToArray());
        return array;
    }
}
