using System.Security.Cryptography;
using System.Text;
using DataLibs.Book;
using DataLibs.Book.Artsts;
using DataLibs.Book.Genres;
using DataLibs.Data.Movies;
using DataLibs.Data.Movies.Genres;
using Utils.Randomizer;

namespace DataLibs.InfoGen;

public class EntertainmentGen
{
    public StringBuilder Books { get; set; } = new StringBuilder();
    public StringBuilder BookGenres { get; set; } = new StringBuilder();
    public StringBuilder BookArtists { get; set; } = new StringBuilder();
    public StringBuilder Movies = new StringBuilder();
    public StringBuilder MovieGenres { get; set; } = new StringBuilder();
    public List<Action> GenreStartAction { get; set; }
    public List<Action> GenreEndAction { get; set; }
    public EntertainmentGen()
    {
        GenreStartAction = new List<Action>
        {
            () => Movies.Append("any of "),
            () => Movies.Append("the REPLACE wrote by "),
            () => Movies.Append("wrote by "),
        };
        GenreEndAction = new List<Action>
        {
            () => Movies.Append("'s book."),
            () => Movies.Append("'s book"),
            () => Movies.Append("'s"),
        };
    }
    public string GetMovies()
    {
        var num = RandomNumberGenerator.GetInt32(1, 3);
        Function.ChanceInvokeOrDefault(() => {
            var tmdb = new TmdbLib();
            for (var i = 0; i < num; i++)
            {
                Movies.Append(tmdb.GetRandom().Title).Append(", ");
            }
        }, () => {
            var netflixMovie = new NetflixMoviesLib();
            for (var i = 0; i < num; i++)
            {
                Movies.Append(netflixMovie.GetRandom().Title).Append(", ");
            }
        }, 2);
        var title = Utils.Text.Randomizer.RandomTitleStyle(Movies.ToString());
        Movies.Clear();
        var last = title.LastIndexOf(",");
        return title[..last];
    }
    public string GetMovieGenres()
    {
        var num = RandomNumberGenerator.GetInt32(1, 3);
        var genre = new NetflixMoviesLib();
        for (var i = 0; i < num; i++)
        {
            MovieGenres.Append(genre.GetRandom().Title).Append(", ");
        }
        var title = Utils.Text.Randomizer.RandomTitleStyle(MovieGenres.ToString());
        MovieGenres.Clear();
        var last = title.LastIndexOf(",");
        return title[..last];
    }
    public string GetBooks()
    {
        var num = RandomNumberGenerator.GetInt32(1, 3);
        var book = new BooksLib();
        for (var i = 0; i < num; i++)
        {
            Books.Append(book.GetRandom().Title).Append(", ");
        }
        var title = Utils.Text.Randomizer.RandomTitleStyle(Books.ToString());
        Books.Clear();
        var last = title.LastIndexOf(",");
        return title[..last];
    }
    public string GetBookGenres()
    {
        var num = RandomNumberGenerator.GetInt32(1, 3);
        var book = new BookGenresLib();
        for (var i = 0; i < num; i++)
        {
            BookGenres.Append(book.GetRandom()).Append(", ");
        }
        var title = Utils.Text.Randomizer.RandomTitleStyle(BookGenres.ToString());
        BookGenres.Clear();
        var last = title.LastIndexOf(",");
        return title[..last];
    }
    public string GetBookArtists()
    {
        var num = RandomNumberGenerator.GetInt32(1, 3);
        var book = new BookArtistsLib();
        for (var i = 0; i < num; i++)
        {
            BookArtists.Append(book.GetRandom()).Append(", ");
        }
        var title = Utils.Text.Randomizer.RandomTitleStyle(BookArtists.ToString());
        BookArtists.Clear();
        var last = title.LastIndexOf(",");
        return title[..last];
    }
}