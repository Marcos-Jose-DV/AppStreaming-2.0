using CommunityToolkit.Maui.Alerts;
using Models;
using Models.ApiTmdb;
using Models.Movies;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

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
    public async Task<Assessments> GetAssessmentsAsync(int id)
    {
        var movie = new Assessments();


        HttpResponseMessage response = await _httpClient.GetAsync($"{AppSettings.BaseUrl}/movie/{id}?append_to_response=credits&language=pt-BR");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStreamAsync();
            try
            {
                var result = await JsonSerializer.DeserializeAsync<MovieDetails>(jsonString, _jsonOptions);
                movie.Name = result.title;
                movie.Launch = DateTime.Parse(result.release_date);

                movie.Gender = string.Join(", ", result.genres.Select(g => g.Name));
                movie.Duration = result.runtime;
                movie.Category = "Movie";


                if (result.Credits != null && result.Credits.Crew != null)
                {
                    var directors = result.Credits.Crew
                        .Where(c => c.Job == "Director")
                        .Select(c => c.Name)
                        .ToList();

                    if (directors != null && directors.Count > 0)
                    {
                        movie.Director = string.Join(", ", directors);
                    }
                    else
                    {
                        movie.Director = "vazio";
                    }
                }

                if (!string.IsNullOrEmpty(result.backdrop_path))
                {
                    string backdropUrl = $"{AppSettings.ImageBaseUrl}{result.backdrop_path}";
                    string backdropFilePath = Path.Combine("D:\\00_Servidor\\Imagens", $"{SanitizeFilename(result.title)}_backdrop.jpg");

                    bool backdropExists = File.Exists(backdropFilePath);
                    if (!backdropExists)
                    {
                        await DownloadImageAsync(backdropUrl, backdropFilePath);
                    }
                    movie.ImagePathBackDrop = backdropFilePath;
                }

                if (!string.IsNullOrEmpty(result.poster_path))
                {
                    string posterUrl = $"{AppSettings.ImageBaseUrl}{result.poster_path}";
                    string posterFilePath = Path.Combine("D:\\00_Servidor\\Imagens", $"{SanitizeFilename(result.title)}_poster.jpg");

                    bool posterExist = File.Exists(posterFilePath);
                    if (!posterExist)
                    {
                        await DownloadImageAsync(posterUrl, posterFilePath);
                    }
                    movie.ImagePath = posterFilePath;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("Token expirado ou credenciais inválidas.");
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException($"Erro de autenticação: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Erro ao deserializar JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar dados do filme.");
            }
        }
        return movie;
    }
    public async Task<IEnumerable<CardHome>> GetMovies(int page)
    {
        var carsHome = new List<CardHome>();

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{AppSettings.BaseUrl}/trending/movie/week?language=pt-BR&page={page}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<MovieResults>(jsonString, _jsonOptions);

                foreach (var movie in result.Results)
                {

                    var cardHome = new CardHome
                        (
                          movie.Id,
                          $"{AppSettings.ImageBaseUrl}{movie.poster_path}",
                          movie.popularity.ToString()
                        );

                    carsHome.Add(cardHome);
                }
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("Token expirado ou credenciais inválidas.");
        }
        catch (AuthenticationException ex)
        {
            throw new AuthenticationException("Erro de autenticação: " + ex.Message);
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Erro ao deserializar JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao verificar dados do filme. {ex.Message}");
        }

        return carsHome;
    }
    private async Task DownloadImageAsync(string imageUrl, string filePath)
    {
        var cancellationToken = new CancellationToken();
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

                    await Toast.Make($"Arquivo salvo em: {filePath}").Show(cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            await Toast.Make($"O arquivo não foi salvo: {ex.Message}").Show(cancellationToken);
        }
    }
    public static string SanitizeFilename(string filename)
    {
        return Regex.Replace(filename, @"[<>:""/\\|?*]", string.Empty);
    }
}