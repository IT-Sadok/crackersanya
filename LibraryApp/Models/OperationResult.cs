namespace LibraryApp.Models;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public OperationResult(bool success, string message, T? data = default)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static OperationResult<T> SuccessResult(string message, T? data = default)
    {
        return new OperationResult<T>(true, message, data);
    }

    public static OperationResult<T> FailureResult(string message)
    {
        return new OperationResult<T>(false, message);
    }
}
