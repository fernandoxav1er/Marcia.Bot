using Marcia.Bot.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Marcia.Bot.Application;
public class Program
{
    private static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();

    private static async Task MainAsync(string[] args)
    {
        var services = new ServiceCollection();
        Startup.ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        var botService = serviceProvider.GetRequiredService<IBotHost>();
        await botService.StartAsync(serviceProvider);

        while (Console.ReadKey(true).Key != ConsoleKey.Q) { }
        await botService.StopAsync();
    }
}
