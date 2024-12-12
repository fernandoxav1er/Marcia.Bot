namespace Marcia.Bot.Domain.Interfaces;

public interface IBotHost
{
    Task StartAsync(IServiceProvider services);
    Task StopAsync();
}
