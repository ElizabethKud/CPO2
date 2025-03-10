namespace CPO2.Models;

public class Series
{
    public int Id { get; set; }
    public string SeriesName { get; set; } = string.Empty;
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
    public List<Book> Books { get; set; } = new();
}
