using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface ILibraryService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task AddBookAsync(Book book);
    Task RemoveBookByCodeAsync(string code);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task<OperationResult<Book>> BorrowBookAsync(string code);
    Task<OperationResult<Book>> ReturnBookAsync(string code);
}
