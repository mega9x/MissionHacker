namespace DataLibs.Data.Movies.Genres;

public class MovieGenres: AbstractStringDataDB
{

    public MovieGenres() : base(ConstStr.ConfigPath.MovieTvGenresPath)
    {
    }
}