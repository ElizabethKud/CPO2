namespace CPO2.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SeriesId { get; set; }
    public Series Series { get; set; } = null!;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
}
