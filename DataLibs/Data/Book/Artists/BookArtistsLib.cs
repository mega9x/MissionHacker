using ConstStr;
using Utils.Text.Csv;

namespace DataLibs.Book.Artsts;

public class BookArtistsLib : AbstractStringDataDB
{
    public BookArtistsLib() : base(ConfigPath.BookArtistsPath)
    {
    }
}