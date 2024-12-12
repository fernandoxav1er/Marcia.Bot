using Marcia.Bot.Domain.Interfaces;
using Marcia.Bot.Infrastructure.HttpClients;
using Marcia.Bot.Infrastructure.MongoDB.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Reflection;

namespace Marcia.Bot.Application;
public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        services
            .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddConsole();
            })
            .AddHttpClient()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IMongoClient, MongoClient>(x => new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value))
            .AddSingleton(x => x.GetRequiredService<IMongoClient>().GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IBotRepository, BotRepository>()
            .AddScoped<IBotHost, BotHost>()
            .AddScoped<IBrasilApiHttpClient, BrasilApiHttpClient>()
            .AddScoped<IChatGptHttpClient, ChatGptHttpClient>();
    }
}
