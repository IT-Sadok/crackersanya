using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface ILibraryService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task AddBookAsync(Book book);
    Task RemoveBookByCodeAsync(string code);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task<string> BorrowBookAsync(string code);
    Task<string> ReturnBookAsync(string code);
}
