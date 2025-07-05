using LibraryApp.Models;

namespace LibraryApp.Interfaces;

public interface IDisplayService
{
    void DisplayBooks(List<Book> books, int currentPage, int pageSize);
    void DisplaySuccess(string message);
    void DisplayError(string message);
}
