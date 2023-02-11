using CsvHelper.Configuration.Attributes;

namespace Models.Data.MoviesAndTV;

public class TmdbMovie
{
    [Name("id")]
    public string Id { get; set; }
    [Name("title")]
    public string Title { get; set; }
    [Name("overview")]
    public string Overview { get; set; }
    [Name("original_language")]
    public string OriginalLanguage { get; set; }
    [Name("vote_count")]
    public string VoteCount { get; set; }
    [Name("vote_average")]
    public string VoteAverage { get; set; }
}