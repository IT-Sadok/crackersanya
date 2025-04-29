using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class LibraryService : ILibraryService
{
    private readonly IJsonRepository _jsonRepository;

    public LibraryService(IJsonRepository jsonRepository)
    {
        _jsonRepository = jsonRepository;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _jsonRepository.LoadDataAsync<Book>();
    }

    public async Task AddBookAsync(Book book)
    {
        var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
        books.Add(book);
        await _jsonRepository.SaveDataAsync(books);
    }

    public async Task RemoveBookByCodeAsync(string code)
    {
        var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
        var bookToRemove = books.FirstOrDefault(b => b.Code == code);
        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            await _jsonRepository.SaveDataAsync(books);
        }
        else
        {
            Console.WriteLine("No book found with the specified code.");
        }
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        var books = await _jsonRepository.LoadDataAsync<Book>();
        return books.Where(b => b.IsAvailable);
    }

    public async Task<string> BorrowBookAsync(string code)
    {
        var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
        var bookToBorrow = books.FirstOrDefault(b => b.Code == code && b.IsAvailable);
        if (bookToBorrow != null)
        {
            bookToBorrow.IsAvailable = false;
            await _jsonRepository.SaveDataAsync(books);
            return $"You have successfully borrowed '{bookToBorrow.Title}'.";
        }
        else
        {
            return "The book is not available for borrowing.";
        }
    }

    public async Task<string> ReturnBookAsync(string code)
    {
        var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
        var bookToReturn = books.FirstOrDefault(b => b.Code == code && !b.IsAvailable);
        if (bookToReturn != null)
        {
            bookToReturn.IsAvailable = true;
            await _jsonRepository.SaveDataAsync(books);
            return $"You have successfully returned '{bookToReturn.Title}'.";
        }
        else
        {
            return "The book is not available for returning.";
        }
    }
}
