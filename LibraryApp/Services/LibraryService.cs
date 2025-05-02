using LibraryApp.Interfaces;
using LibraryApp.Models;
using Microsoft.Extensions.Logging;

namespace LibraryApp.Services;

public class LibraryService : ILibraryService
{
    private readonly IJsonRepository _jsonRepository;
    private readonly ILogger<LibraryService> _logger;

    public LibraryService(IJsonRepository jsonRepository, ILogger<LibraryService> logger)
    {
        _jsonRepository = jsonRepository;
        _logger = logger;
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
            _logger.LogError("No book found with the specified code.");
        }
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        var books = await _jsonRepository.LoadDataAsync<Book>();
        return books.Where(b => b.IsAvailable);
    }

    public async Task<OperationResult<Book>> BorrowBookAsync(string code)
    {
        var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
        var bookToBorrow = books.FirstOrDefault(b => b.Code == code && b.IsAvailable);

        if (bookToBorrow != null)
        {
            bookToBorrow.IsAvailable = false;
            await _jsonRepository.SaveDataAsync(books);

            return OperationResult<Book>.SuccessResult(
                $"You have successfully borrowed '{bookToBorrow.Title}'.",
                bookToBorrow
            );
        }
        else
        {
            return OperationResult<Book>.FailureResult("The book is not available for borrowing.");
        }
    }

    public async Task<OperationResult<Book>> ReturnBookAsync(string code)
    {
        var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
        var bookToReturn = books.FirstOrDefault(b => b.Code == code && !b.IsAvailable);

        if (bookToReturn != null)
        {
            bookToReturn.IsAvailable = true;
            await _jsonRepository.SaveDataAsync(books);

            return OperationResult<Book>.SuccessResult(
                $"You have successfully returned '{bookToReturn.Title}'.",
                bookToReturn
            );
        }
        else
        {
            return OperationResult<Book>.FailureResult("The book is not available for returning.");
        }
    }
}
