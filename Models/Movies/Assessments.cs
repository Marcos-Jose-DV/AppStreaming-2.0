using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models.Movies;

[Table("Avaliações")]
public class Assessments
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]

    public string Name { get; set; }
    [Required, MaxLength(2)]
    public string Assessment { get; set; }

    [Required, MaxLength(100)]
    public string Director { get; set; }
    public string ImagePath { get; set; }
    public string ImagePathBackDrop { get; set; }

    [Required, MaxLength(100)]
    public string Gender { get; set; }
    public double Duration { get; set; }
    public double Position { get; set; }
    public bool Concluded { get; set; }
    public string Comments { get; set; }

    [Required, MaxLength(10)]
    public string Category { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime Launch { get; set; }
}
