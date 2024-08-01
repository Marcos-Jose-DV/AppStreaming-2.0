using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAvaliacao.Models;

[Table("Avaliações")]
public class Assessments
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Assessment { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Duration { get; set; }
    public double Position { get; set; }
    public bool Concluded { get; set; }
    public string Comments { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; }
    public DateTime Launch { get; set; }
}
