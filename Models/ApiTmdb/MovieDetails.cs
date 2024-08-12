namespace Models.ApiTmdb;

public class MovieDetails
{
    public bool adult { get; set; }
    public string? backdrop_path { get; set; }
    public object? belongs_to_collection { get; set; }
    public int budget { get; set; }
    public List<Genre> genres { get; set; } = new List<Genre>();
    public string homepage { get; set; } = string.Empty;
    public int id { get; set; }
    public string? imdb_id { get; set; }
    public List<string> origin_country { get; set; } = new List<string>();
    public string original_language { get; set; } = string.Empty;
    public string original_title { get; set; } = string.Empty;
    public string overview { get; set; } = string.Empty;
    public double popularity { get; set; }
    public string? poster_path { get; set; }
    public List<ProductionCompany> production_companies { get; set; } = new List<ProductionCompany>();
    public List<ProductionCountry> production_countries { get; set; } = new List<ProductionCountry>();
    public string release_date { get; set; } = string.Empty;
    public int revenue { get; set; }
    public int runtime { get; set; }
    public List<SpokenLanguage> spoken_languages { get; set; } = new List<SpokenLanguage>();
    public string status { get; set; } = string.Empty;
    public string tagline { get; set; } = string.Empty;
    public string title { get; set; } = string.Empty;
    public bool video { get; set; }
    public double vote_average { get; set; }
    public int vote_count { get; set; }
    public Credits Credits { get; set; } = new();

}


