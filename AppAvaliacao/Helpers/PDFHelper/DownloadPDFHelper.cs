using iTextSharp.text;
using iTextSharp.text.pdf;
using Models.Movies;


namespace AppAvaliacao.Helpers.PDFHerlper;

public class DownloadPDFHelper
{
    public Stream DownloadPDF(IEnumerable<Assessments> assessments)
    {
        using MemoryStream stream = new();
        Document doc = new();
        PdfWriter writer = PdfWriter.GetInstance(doc, stream);

        doc.Open();

        foreach (var assessment in assessments)
        {
            Paragraph paragraph = new(
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

            doc.Add(paragraph);
        }

        doc.Close();
        var array = new MemoryStream(stream.ToArray());
        return array;
    }
}
