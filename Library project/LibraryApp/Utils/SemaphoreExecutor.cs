using Microsoft.Extensions.Logging;

namespace LibraryApp.Utils;

public static class SemaphoreExecutor
{
    public static async Task ExecuteAsync(SemaphoreSlim semaphore, Func<Task> operation)
    {
        if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        await semaphore.WaitAsync();

        try
        {
            await operation();
        }
        finally
        {
                semaphore.Release();
        }
    }

    public static async Task<TResult> ExecuteAsync<TResult>(SemaphoreSlim semaphore, Func<Task<TResult>> operation, ILogger logger = null)
    {
        if (semaphore == null) throw new ArgumentNullException(nameof(semaphore));
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        await semaphore.WaitAsync();

        try
        {
            return await operation();
        }
        finally
        {
                semaphore.Release();
        }
    }
}