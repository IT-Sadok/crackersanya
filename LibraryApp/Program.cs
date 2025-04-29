using LibraryApp.Repositories;
using LibraryApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        // Define the path to the "Shelf" folder
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string libraryPath = Path.Combine(projectRoot, "Shelf", "library.json");

        // Create Host with DI
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register dependencies
                services.AddSingleton<IJsonRepository>(provider => new JsonRepository(libraryPath));
                services.AddSingleton<ILibraryService, LibraryService>();
                services.AddSingleton<IBookService, BookService>();
                services.AddSingleton<IDisplayService, DisplayService>();
                services.AddSingleton<IMenuService, MenuService>();
                services.AddSingleton<ISplashScreenService, SplashScreenService>();
            })
            .Build();

        // Retrieve ISplashScreenService and display the opening animation
        var splashScreenService = host.Services.GetRequiredService<ISplashScreenService>();
        splashScreenService.ShowGreetingsAnimation();

        // Run the application
        var menuService = host.Services.GetRequiredService<IMenuService>();
        await menuService.ShowMenuAsync();
    }
}
