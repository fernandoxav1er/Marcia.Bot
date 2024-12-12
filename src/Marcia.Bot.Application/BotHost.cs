using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Marcia.Bot.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Marcia.Bot.Application;

public class BotHost : IBotHost
{
    private readonly ILogger<BotHost> _logger;
    private readonly CommandService _commands;
    private readonly DiscordSocketClient _client;
    private readonly IConfiguration _configuration;
    private IServiceProvider? _serviceProvider;

    public BotHost(IConfiguration configuration, ILogger<BotHost> logger)
    {
        _logger = logger;
        _configuration = configuration;

        DiscordSocketConfig config = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);
        _commands = new CommandService();
    }

    public async Task StartAsync(IServiceProvider services)
    {
        string discordToken = _configuration["DiscordToken"] ?? throw new Exception("Token do Discord não encontrado!");
        _logger.LogInformation($"Iniciando serviço com token: {discordToken}");

        _serviceProvider = services;

        _logger.LogInformation("Adicionando módulos de comandos...");
        var modules = await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
        _logger.LogInformation($"Módulos registrados: {string.Join(", ", _commands.Modules.Select(m => m.Name))}");

        await _client.LoginAsync(TokenType.Bot, discordToken);
        await _client.StartAsync();

        _client.MessageReceived += HandleCommandAsync;
        _logger.LogInformation("Marcia operando e esperando comandos..");
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        if (arg is not SocketUserMessage message || message.Author.IsBot) return;

        _logger.LogInformation($"{DateTime.Now.ToShortTimeString()} - {message.Author}: {message.Content}");

        int position = 0;
        bool messageIsCommand = message.HasCharPrefix('!', ref position);

        if (messageIsCommand)
        {
            _logger.LogInformation("Comando detectado: " + message.Content);
            var context = new SocketCommandContext(_client, message);
            _logger.LogInformation("Executando comando...");
            var result = await _commands.ExecuteAsync(context, position, _serviceProvider);
        }
    }

    public async Task StopAsync()
    {
        _logger.LogWarning("Serviço finalizado.");

        if (_client != null)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
    }
}
