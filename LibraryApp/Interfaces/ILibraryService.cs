using LibraryApp.Models;

namespace LibraryApp.Services
{
    public interface ILibraryService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task RemoveBookByCodeAsync(string code);
        Task<List<Book>> GetAvailableBooksAsync();
        Task<string> BorrowBookAsync(string code);
        Task<string> ReturnBookAsync(string code);
    }
}
