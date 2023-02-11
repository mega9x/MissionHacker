using CsvHelper;

namespace DataLibs.Data.Movies.Genres;

public class AnimeGenres:AbstractStringDataDB
{
    public AnimeGenres() : base(ConstStr.ConfigPath.AnimeGenresPath)
    {
    }
}