using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class DisplayService : IDisplayService
{
    public void DisplayBooks(List<Book> books, int currentPage, int pageSize)
    {
        Console.Clear();
        int totalPages = (int)Math.Ceiling((double)books.Count / pageSize);
        Console.WriteLine($"All Books (Page {currentPage + 1}/{totalPages}):");

        var booksToShow = books.Skip(currentPage * pageSize).Take(pageSize).ToList();
        for (int i = 0; i < booksToShow.Count; i++)
        {
            var book = booksToShow[i];
            Console.Write($"{i + 1}. {book.Title} by {book.Author} (Code: {book.Code}, Release Date: {book.ReleaseDate?.ToString("yyyy-MM-dd") ?? "Not specified"}, Status: ");
            if (book.IsAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Available");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not Available");
            }
            Console.ResetColor();
        }
    }

    public void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
