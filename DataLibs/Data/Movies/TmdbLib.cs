using Models.Data.MoviesAndTV;

namespace DataLibs.Data.Movies;

public class TmdbLib: AbstractDataDB<TmdbMovie>
{
    public TmdbLib() : base(ConstStr.ConfigPath.TmdbPath)
    {
    }
}