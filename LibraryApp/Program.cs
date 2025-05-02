using LibraryApp.Common;
using LibraryApp.Interfaces;
using LibraryApp.Repositories;
using LibraryApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Define the path to the "Shelf" folder
string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
string libraryPath = Path.Combine(projectRoot, Constants.DefaultLibraryPath);

// Create Host with DI
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register dependencies
        services.AddSingleton<IJsonRepository, JsonRepository>();
        services.AddSingleton<IConfiguration>(provider =>
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "LibraryPath", libraryPath }
                })
                .Build();
            return configuration;
        });
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
await menuService.RunAppAsync();
