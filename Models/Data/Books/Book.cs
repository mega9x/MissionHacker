using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace Models.Data.Books;

public class Book
{
    [Name("bookID")]
    [JsonPropertyName("bookID")]
    public string Id{ get; set; }
    [Name("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [Name("authors")]
    [JsonPropertyName("authors")]
    public string Authors { get; set; }
    [Name("average_rating")]
    [JsonPropertyName("average_rating")]
    public string AverageRating { get; set; }
    [Name("isbn")]
    [JsonPropertyName("isbn")]
    public string ISBN { get; set; }
    [Name("isbn13")]
    [JsonPropertyName("isbn13")]
    public string ISBN13 { get; set; }
    [Name("language_code")]
    [JsonPropertyName("language_code")]
    public string LanguareCode { get; set; }
    [Name("num_pages")]
    [JsonPropertyName("num_page")]
    public string NumPage { get; set; }
    [Name("ratings_count")]
    [JsonPropertyName("ratings_count")]
    public string RatingsCount { get; set; }
    [Name("text_reviews_count")]
    [JsonPropertyName("text_reviews_count")]
    public string ReviewsCount { get; set; }
    [Name("publication_date")]
    [JsonPropertyName("publication_date")]
    public string PublicationDate { get; set; }
    [Name("publisher")]
    [JsonPropertyName("publisher")]
    public string Publisher { get; set; }
}