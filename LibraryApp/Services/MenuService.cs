using LibraryApp.Builders;
using LibraryApp.Helpers;
using LibraryApp.Services;

public class MenuService : IMenuService
{
    private readonly IBookService _bookService;
    private readonly IDisplayService _displayService;

    public MenuService(IBookService bookService, IDisplayService displayService)
    {
        _bookService = bookService;
        _displayService = displayService;
    }

    public async Task ShowMenuAsync()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the library app!");
            Console.WriteLine("Menu:");
            Console.WriteLine("1. View all books");
            Console.WriteLine("2. Search books");
            Console.WriteLine("3. Return a book");
            Console.WriteLine("4. Add a book");
            Console.WriteLine("5. Remove a book");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an action (1-6): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllBooksAsync();
                    break;
                case "2":
                    await SearchBooksAsync();
                    break;
                case "3":
                    await ReturnBookAsync();
                    break;
                case "4":
                    await AddBookAsync();
                    break;
                case "5":
                    await RemoveBookAsync();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    _displayService.DisplayError("Invalid choice, please try again.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task ViewAllBooksAsync()
    {
        var books = await _bookService.GetAllBooksAsync();
        if (books.Count == 0)
        {
            _displayService.DisplayError("No books available.");
            return;
        }

        const int pageSize = 10;
        int currentPage = 0;

        while (true)
        {
            _displayService.DisplayBooks(books, currentPage, pageSize);

            Console.WriteLine("\nOptions:");
            Console.WriteLine("Enter the number of the book to borrow (only if available).");
            if (currentPage > 0) Console.WriteLine("P. Previous page");
            if ((currentPage + 1) * pageSize < books.Count) Console.WriteLine("N. Next page");
            Console.WriteLine("E. Exit");

            Console.Write("\nChoose an option: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int bookNumber) && bookNumber > 0 && bookNumber <= books.Skip(currentPage * pageSize).Take(pageSize).Count())
            {
                var selectedBook = books.Skip(currentPage * pageSize).Take(pageSize).ToList()[bookNumber - 1];
                if (!selectedBook.IsAvailable)
                {
                    _displayService.DisplayError("This book is not available for borrowing.");
                }
                else
                {
                    var result = await _bookService.BorrowBookAsync(selectedBook.Code);
                    if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedBook.IsAvailable = false;
                        _displayService.DisplaySuccess(result);
                    }
                    else
                    {
                        _displayService.DisplayError(result);
                    }
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            else if (input?.ToUpper() == "P" && currentPage > 0)
            {
                currentPage--;
            }
            else if (input?.ToUpper() == "N" && (currentPage + 1) * pageSize < books.Count)
            {
                currentPage++;
            }
            else if (input?.ToUpper() == "E")
            {
                break;
            }
            else
            {
                _displayService.DisplayError("Invalid option. Please try again.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task SearchBooksAsync()
    {
        Console.Clear();
        Console.WriteLine("Search for a book:");
        string query = InputHelper.PromptForInput("Enter a keyword (title or author): ", "Search query cannot be empty.");

        var books = await _bookService.GetAllBooksAsync();
        var filteredBooks = _bookService.SearchBooks(books, query);

        if (filteredBooks.Count == 0)
        {
            _displayService.DisplayError("No books found matching your query.");
        }
        else
        {
            _displayService.DisplayBooks(filteredBooks, 0, filteredBooks.Count);
        }

        Console.WriteLine("\nPress any key to return to the menu...");
        Console.ReadKey();
    }

    private async Task AddBookAsync()
    {
        Console.Clear();
        Console.WriteLine("Adding a new book");

        string title = InputHelper.PromptForInput("Enter the book title: ", "The book title cannot be empty.");
        string author = InputHelper.PromptForInput("Enter the author's name: ", "The author's name cannot be empty.");
        string code = InputHelper.PromptForInput("Enter the book code: ", "The book code cannot be empty.");

        DateTime? releaseDate = InputHelper.PromptForDate("Enter the book's release date (yyyy-MM-dd) or leave it empty: ");

        var book = new BookBuilder()
            .WithId(Guid.NewGuid())
            .WithTitle(title)
            .WithAuthor(author)
            .WithReleaseDate(releaseDate)
            .WithCode(code)
            .Build();

        await _bookService.AddBookAsync(book);
        _displayService.DisplaySuccess("The book has been successfully added!");
    }

    private async Task RemoveBookAsync()
    {
        Console.Clear();
        Console.WriteLine("Removing a book");

        string code = InputHelper.PromptForInput("Enter the book code to remove: ", "The book code cannot be empty.");
        if (ConfirmAction($"Are you sure you want to remove the book with code {code}?"))
        {
            await _bookService.RemoveBookByCodeAsync(code);
            _displayService.DisplaySuccess("The book has been successfully removed.");
        }
        else
        {
            _displayService.DisplayError("Action canceled.");
        }
    }

    private async Task ReturnBookAsync()
    {
        Console.Clear();
        Console.WriteLine("Returning a book");

        string code = InputHelper.PromptForInput("Enter the book code to return: ", "The book code cannot be empty.");
        var result = await _bookService.ReturnBookAsync(code);

        if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
        {
            _displayService.DisplaySuccess(result);
        }
        else
        {
            _displayService.DisplayError(result);
        }
    }



    private bool ConfirmAction(string message)
    {
        Console.Write($"{message} (Y/N): ");
        var input = Console.ReadLine();
        return input?.ToUpper() == "Y";
    }
}
