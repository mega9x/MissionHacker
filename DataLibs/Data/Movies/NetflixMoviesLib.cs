using Models.Data.MoviesAndTV;

namespace DataLibs.Data.Movies;

public class NetflixMoviesLib: AbstractDataDB<NetflixMovies>
{
    public NetflixMoviesLib() : base(ConstStr.ConfigPath.NetflixDbPath)
    {
    }
}