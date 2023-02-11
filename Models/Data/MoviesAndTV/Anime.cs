using CsvHelper.Configuration.Attributes;

namespace Models.Data.MoviesAndTV;

public class Anime
{
    [Name("anime_id")]
    public string ID { get; set; }
    [Name("name")]
    public string Name { get; set; }
    [Name("genre")]
    public string Genres { get; set; }
    [Name("type")]
    public string Type { get; set; }
    [Name("episodes")]
    public string Ep { get; set; }
    [Name("rating")]
    public string Rating { get; set; }
    [Name("members")]
    public string Members { get; set; }
}