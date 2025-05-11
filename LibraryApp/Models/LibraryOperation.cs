using static LibraryApp.Common.Enums;

namespace LibraryApp.Models;

public class LibraryOperation
{
    public LibraryOperationType Type { get; set; }

    public required string BookCode { get; set; }

    public string? UpdateField { get; set; }

    public string? UpdateValue { get; set; }
}