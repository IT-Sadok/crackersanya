using LibraryApp.Models;

public interface IDisplayService
{
    void DisplayBooks(List<Book> books, int currentPage, int pageSize);
    void DisplaySuccess(string message);
    void DisplayError(string message);
}
