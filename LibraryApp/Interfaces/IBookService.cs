using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    IEnumerable<Book> SearchBooks(IEnumerable<Book> books, string query);
    Task<OperationResult<Book>> BorrowBookAsync(string code);
    Task<OperationResult<Book>> ReturnBookAsync(string code);
    Task AddBookAsync(Book book);
    Task RemoveBookByCodeAsync(string code);
}