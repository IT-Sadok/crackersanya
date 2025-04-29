namespace LibraryApp.Models;

public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public string Code { get; set; }

    public bool IsAvailable { get; set; }
}
