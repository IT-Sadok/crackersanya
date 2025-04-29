using LibraryApp.Models;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    List<Book> SearchBooks(List<Book> books, string query);
    Task<string> BorrowBookAsync(string code);
    Task<string> ReturnBookAsync(string code);
    Task AddBookAsync(Book book);
    Task RemoveBookByCodeAsync(string code);
}