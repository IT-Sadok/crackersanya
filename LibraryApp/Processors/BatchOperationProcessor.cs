using LibraryApp.Interfaces;
using LibraryApp.Models;
using Microsoft.Extensions.Logging;
using static LibraryApp.Common.Enums;

namespace LibraryApp.Processors;

public class BatchOperationProcessor : IBatchOperationProcessor
{
    private readonly ILibraryService _libraryService;
    private readonly ILogger<BatchOperationProcessor> _logger;
    private readonly Random _random = new Random();

    public BatchOperationProcessor(ILibraryService libraryService, ILogger<BatchOperationProcessor> logger)
    {
        _libraryService = libraryService;
        _logger = logger;
    }

    public async Task ProcessLibraryOperationsAsync(List<LibraryOperation> operations)
    {
        if (operations == null || operations.Count == 0)
        {
            _logger.LogInformation("No operations to process.");
            return;
        }

        var tasks = new List<Task>();

        foreach (var op in operations)
        {
            tasks.Add(Task.Run(async () =>
            {
                await PerformSingleOperationAsync(op);
            }));
        }

        await Task.WhenAll(tasks);
    }

    private async Task PerformSingleOperationAsync(LibraryOperation op)
    {
        if (op == null)
        {
            _logger.LogWarning("Skipping null operation.");
            return;
        }

        try
        {
            switch (op.Type)
            {
                case LibraryOperationType.Borrow:
                    if (string.IsNullOrEmpty(op.BookCode))
                    {
                        _logger.LogWarning("Skipping {OperationType} operation: BookCode is empty.", op.Type);
                        return;
                    }
                    await _libraryService.BorrowBookAsync(op.BookCode);
                    break;

                case LibraryOperationType.Return:
                    if (string.IsNullOrEmpty(op.BookCode))
                    {
                        _logger.LogWarning("Skipping {OperationType} operation: BookCode is empty.", op.Type);
                        return;
                    }
                    await _libraryService.ReturnBookAsync(op.BookCode);
                    break;

                case LibraryOperationType.Update:
                    if (string.IsNullOrEmpty(op.BookCode) || string.IsNullOrEmpty(op.UpdateField))
                    {
                        _logger.LogWarning("Skipping {OperationType} operation: BookCode or UpdateField is empty.", op.Type);
                        return;
                    }
                    await _libraryService.UpdateBookAsync(op.BookCode, op.UpdateField, op.UpdateValue);
                    break;

                case LibraryOperationType.Unknown:
                default:
                    _logger.LogWarning("Skipping operation with Unknown type.");
                    break;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing operation Type='{OperationType}' for book '{BookCode}'. Details: Field='{UpdateField}', Value='{UpdateValue}'",
                             op.Type, op.BookCode, op.UpdateField, op.UpdateValue);
        }
    }

    public async Task RunSimulationAsync(int numberOfOperations = 50)
    {
        _logger.LogInformation("Starting library operations simulation with {NumberOfOperations} operations.", numberOfOperations);

        var simulationOperations = GenerateSimulationOperations(numberOfOperations);

        await ProcessLibraryOperationsAsync(simulationOperations);

        _logger.LogInformation("Library operations simulation finished.");
    }

    private List<LibraryOperation> GenerateSimulationOperations(int count)
    {
        var operations = new List<LibraryOperation>(count);
        var operationTypes = new[] { LibraryOperationType.Borrow, LibraryOperationType.Return, LibraryOperationType.Update };
        var updateFields = new[] { "Title", "Author", "ReleaseDate" };

        var bookCodes = Enumerable.Range(1, 50)
                                .Select(i => $"BOOK{i:D3}")
                                .ToList();

        for (int i = 0; i < count; i++)
        {
            var operationType = operationTypes[_random.Next(operationTypes.Length)];
            var bookCode = bookCodes[_random.Next(bookCodes.Count)];

            var operation = new LibraryOperation
            {
                Type = operationType,
                BookCode = bookCode
            };

            if (operationType == LibraryOperationType.Update)
            {
                var updateField = updateFields[_random.Next(updateFields.Length)];
                var currentTime = DateTime.Now;
                operation.UpdateField = updateField;

                switch (updateField)
                {
                    case "Title":
                        operation.UpdateValue = $"New_book_title_{currentTime}_{i}";
                        break;
                    case "Author":
                        operation.UpdateValue = $"New_book_author_{currentTime}_{i}";
                        break;
                    case "ReleaseDate":
                        operation.UpdateValue = currentTime.ToString();
                        break;
                    default:
                        break;
                }
            }

            operations.Add(operation);
        }

        return operations;
    }
}