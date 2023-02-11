using System.Security.Cryptography;
using ConstStr;
using Utils.Text.Csv;

namespace DataLibs.Book;

public class BooksLib: AbstractDataDB<Models.Data.Books.Book>
{
    public BooksLib() : base(ConfigPath.BooksPath)
    {
    }
}