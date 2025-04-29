using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    IEnumerable<Book> SearchBooks(IEnumerable<Book> books, string query);
    Task<string> BorrowBookAsync(string code);
    Task<string> ReturnBookAsync(string code);
    Task AddBookAsync(Book book);
    Task RemoveBookByCodeAsync(string code);
}