namespace Models.ApiTmdb;

public class MovieResults
{
    public int page { get; set; }
    public List<Movies> Results { get; set; }
    public int total_pages { get; set; }
    public int total_results { get; set; }
}

