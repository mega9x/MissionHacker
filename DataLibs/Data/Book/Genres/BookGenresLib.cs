using ConstStr;
using Utils.Text.Csv;

namespace DataLibs.Book.Genres;

public class BookGenresLib : AbstractStringDataDB
{
    public BookGenresLib() : base(ConfigPath.BookGenresPath)
    {
    }
}