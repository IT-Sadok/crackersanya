namespace LibraryApp.Repositories
{
    public interface IJsonRepository
    {
        Task SaveDataAsync<T>(T data);
        Task<List<T>> LoadDataAsync<T>();
    }
}
