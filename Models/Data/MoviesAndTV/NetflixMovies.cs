using CsvHelper.Configuration.Attributes;

namespace Models.Data.MoviesAndTV;

public class NetflixMovies
{
    [Name("index")]
    public string Index { get; set; }
    [Name("id")]
    public string Id { get; set; }
    [Name("title")]
    public string Title { get; set; }
    [Name("type")]
    public string Type { get; set; }
    [Name("release_year")]
    public string ReleaseYear { get; set; }
    [Name("age_certification")]
    public string AgeCertification { get; set; }
    [Name("runtime")]
    public string Runtime { get; set; }
    [Name("genres")]
    public string Genres { get; set; }
    [Name("production_countries")]
    public string ProductionCountries { get; set; }
    [Name("seasons")]
    public string Seasons { get; set; }
    [Name("imdb_id")]
    public string ImdbId { get; set; }
    [Name("imdb_score")]
    public string ImdbScore { get; set; }
    [Name("imdb_votes")]
    public string ImdbVotes { get; set; }
}