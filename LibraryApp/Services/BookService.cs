using LibraryApp.Models;
using LibraryApp.Services;

public class BookService : IBookService
{
    private readonly ILibraryService _libraryService;

    public BookService(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _libraryService.GetAllBooksAsync();
    }

    public List<Book> SearchBooks(List<Book> books, string query)
    {
        return books.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
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
