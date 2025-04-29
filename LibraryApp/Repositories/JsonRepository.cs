using System.Text.Json;

namespace LibraryApp.Repositories
{
    public class JsonRepository : IJsonRepository
    {
        private readonly string _filePath;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;

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
            try
            {
                string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public async Task<List<T>> LoadDataAsync<T>()
        {
            try
            {
                var jsonData = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<T>();
            }
        }
    }
}
