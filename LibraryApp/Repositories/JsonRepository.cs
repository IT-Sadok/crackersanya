using LibraryApp.Interfaces;
using LibraryApp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LibraryApp.Repositories;

public class JsonRepository : IJsonRepository
{
    private readonly string _filePath;
    private readonly ILogger<JsonRepository> _logger;
    private readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1);

    public JsonRepository(IConfiguration configuration, ILogger<JsonRepository> logger)
    {
        _filePath = configuration["LibraryPath"] ?? throw new ArgumentNullException("LibraryPath is not configured.");
        _logger = logger;

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task SaveDataAsync<T>(T data)
    {
        await SemaphoreExecutor.ExecuteAsync(_fileSemaphore, async () =>
        {
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, jsonString);
        });
    }

    public async Task<List<T>> LoadDataAsync<T>()
    {
        return await SemaphoreExecutor.ExecuteAsync(_fileSemaphore, async () =>
        {
            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
        });
    }
}
