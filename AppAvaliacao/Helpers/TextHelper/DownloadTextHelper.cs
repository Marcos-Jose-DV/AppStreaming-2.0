using Models.Movies;


namespace AppAvaliacao.Helpers.TextHelper;

public class DownloadTextHelper
{
    public Stream DownloadText(IEnumerable<Assessments> assessments)
    {
        using MemoryStream stream = new();
        using StreamWriter writer = new(stream);
        foreach (var assessment in assessments)
        {
            writer.WriteLine(
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
        }

        var array = new MemoryStream(stream.ToArray());
        return array;
    }
}
