using Models.Data.MoviesAndTV;

namespace DataLibs.Data.Movies;

public class AnimesLib: AbstractDataDB<Anime>
{

    public AnimesLib() : base(ConstStr.ConfigPath.AnimePath)
    {
    }
}