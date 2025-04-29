using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class BookService : IBookService
{
    private readonly ILibraryService _libraryService;

    public BookService(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _libraryService.GetAllBooksAsync();
    }

    public IEnumerable<Book> SearchBooks(IEnumerable<Book> books, string query)
    {
        return books.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<string> BorrowBookAsync(string code)
    {
        return await _libraryService.BorrowBookAsync(code);
    }

    public async Task<string> ReturnBookAsync(string code)
    {
        return await _libraryService.ReturnBookAsync(code);
    }

    public async Task AddBookAsync(Book book)
    {
        await _libraryService.AddBookAsync(book);
    }

    public async Task RemoveBookByCodeAsync(string code)
    {
        await _libraryService.RemoveBookByCodeAsync(code);
    }
}
