namespace Models.ApiTmdb;
public class Movies
{
    public string Backdrop_Path { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Original_Title { get; set; }
    public string overview { get; set; }
    public string poster_path { get; set; }
    public string media_type { get; set; }
    public bool adult { get; set; }
    public string original_language { get; set; }
    public List<int> genre_ids { get; set; }
    public double popularity { get; set; }
    public string release_date { get; set; }
    public bool video { get; set; }
    public double Vote_Average { get; set; }
    public int Vote_Count { get; set; }
}

