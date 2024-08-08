using Models.ApiTmdb;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace AppAvaliacao.Services;

public class RestService
{
    private readonly HttpClient _httpClient;
    JsonSerializerOptions _jsonOptions;


    public RestService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.ApiKey);
        _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
    public async Task<List<Movie>> GetMovies()
    {
        var movies = new List<Movie>();
        for (int i = 0; i < 100; i++)
        {

            HttpResponseMessage response = await _httpClient.GetAsync($"{AppSettings.BaseUrl}/trending/movie/week?language=pt-BR&page={i}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStreamAsync();
                try
                {
                    var result = await JsonSerializer.DeserializeAsync<MovieResults>(jsonString, _jsonOptions);

                    foreach (var movie in result.Results)
                    {
                        movies.Add(movie);

                        //if (!string.IsNullOrEmpty(movie.Backdrop_Path))
                        //{
                        //    string backdropUrl = $"{AppSettings.ImageBaseUrl}{movie.Backdrop_Path}";
                        //    string backdropFilePath = Path.Combine("D:\\00_Servidor\\Imagens", $"{SanitizeFilename(movie.Title)}_backdrop.jpg");
                        //    await DownloadImageAsync(backdropUrl, backdropFilePath);
                        //}

                        //if (!string.IsNullOrEmpty(movie.poster_path))
                        //{
                        //    string posterUrl = $"{AppSettings.ImageBaseUrl}{movie.poster_path}";
                        //    string posterFilePath = Path.Combine("D:\\00_Servidor\\Imagens", $"{SanitizeFilename(movie.Title)}_poster.jpg");
                        //    await DownloadImageAsync(posterUrl, posterFilePath);
                        //}
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        return movies;
    }
    private async Task DownloadImageAsync(string imageUrl, string filePath)
    {
        try
        {
            var fileExiste = File.Exists(filePath);
            if (!fileExiste)
            {
                var response = await _httpClient.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {

                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    Console.WriteLine($"Downloaded and saved image to {filePath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download image from {imageUrl}: {ex.Message}");
        }
    }
    public static string SanitizeFilename(string filename)
    {
        return Regex.Replace(filename, @"[<>:""/\\|?*]", string.Empty);
    }
}