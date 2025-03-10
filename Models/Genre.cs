namespace CPO2.Models;

public class Genre
{
    public int Id { get; set; }
    public string GenreName { get; set; } = string.Empty;
    public List<Series> Series { get; set; } = new();
}
