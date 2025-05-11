using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Utils;
using Microsoft.Extensions.Logging;

namespace LibraryApp.Services;

public class LibraryService : ILibraryService
{
    private readonly IJsonRepository _jsonRepository;
    private readonly ILogger<LibraryService> _logger;
    private readonly SemaphoreSlim _serviceOperationSemaphore = new SemaphoreSlim(1, 1);

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
        await SemaphoreExecutor.ExecuteAsync(_serviceOperationSemaphore, async () =>
        {
            var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
            books.Add(book);
            await _jsonRepository.SaveDataAsync(books);
        });     
    }

    public async Task RemoveBookByCodeAsync(string code)
    {
        await SemaphoreExecutor.ExecuteAsync(_serviceOperationSemaphore, async () =>
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
                _logger.LogError("Remove failed: No book found with the specified code '{BookCode}'.", code);
            }
        });
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        var books = await _jsonRepository.LoadDataAsync<Book>();
        return books.Where(b => b.IsAvailable);
    }

    public async Task<OperationResult<Book>> BorrowBookAsync(string code)
    {
        return await SemaphoreExecutor.ExecuteAsync(_serviceOperationSemaphore, async () =>
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
        });
    }

    public async Task<OperationResult<Book>> ReturnBookAsync(string code)
    {
        return await SemaphoreExecutor.ExecuteAsync(_serviceOperationSemaphore, async () =>
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
        });
    }

    public async Task<OperationResult<Book>> UpdateBookAsync(string code, string field, string value)
    {
        return await SemaphoreExecutor.ExecuteAsync(_serviceOperationSemaphore, async () =>
        {
            var books = (await _jsonRepository.LoadDataAsync<Book>()).ToList();
            var bookToUpdate = books.FirstOrDefault(b => b.Code == code);

            if (bookToUpdate == null)
            {
                _logger.LogError("Update failed: No book found with the specified code {BookCode}.", code);
                return OperationResult<Book>.FailureResult($"No book found with code {code} to update.");
            }

            switch (field)
            {
                case "Title":
                    bookToUpdate.Title = value;
                    await _jsonRepository.SaveDataAsync(books);
                    _logger.LogInformation("Successfully updated field '{UpdateField}' for book {BookCode}.", field, code);
                   
                    return OperationResult<Book>.SuccessResult($"Successfully updated title for book '{bookToUpdate.Title}'.", bookToUpdate);

                case "Author":
                    bookToUpdate.Author = value;
                    await _jsonRepository.SaveDataAsync(books);
                    _logger.LogInformation("Successfully updated field '{UpdateField}' for book {BookCode}.", field, code);
                  
                    return OperationResult<Book>.SuccessResult($"Successfully updated author for book '{bookToUpdate.Title}'.", bookToUpdate);

                case "ReleaseDate":
                    if (DateTime.TryParse(value, out DateTime date))
                    {
                        bookToUpdate.ReleaseDate = date;
                        await _jsonRepository.SaveDataAsync(books);
                        _logger.LogInformation("Successfully updated field '{UpdateField}' for book {BookCode}.", field, code);
                        
                        return OperationResult<Book>.SuccessResult($"Successfully updated publication date for book '{bookToUpdate.Title}'.", bookToUpdate);
                    }
                    else
                    {
                        _logger.LogError("Update failed: Invalid date format '{UpdateValue}' for PublicationDate for book {BookCode}.", value, code);
                       
                        return OperationResult<Book>.FailureResult($"Invalid date format for PublicationDate for book {code}.");
                    }

                default:
                    _logger.LogWarning("Update failed: Attempted to update unknown or unsupported field '{UpdateField}' for book {BookCode}.", field, code);
                    
                    return OperationResult<Book>.FailureResult($"Unknown or unsupported field '{field}' for book {code}.");
            }
        });
    }
}