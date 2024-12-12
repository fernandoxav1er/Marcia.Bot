using Discord;
using Discord.Commands;
using Marcia.Bot.Domain.Interfaces;
using Marcia.Bot.Domain.Models;
using Marcia.Bot.Infrastructure.HttpClients;
using Microsoft.Extensions.Logging;

namespace Marcia.Bot.Application.Commands;

public class ChatCommand : ModuleBase<SocketCommandContext>, IGetMessageReference
{
    private readonly IChatGptHttpClient _chatService;
    private readonly IUserRepository _userRequestRepository;
    private readonly IBotRepository _botResponseRepository;
    private readonly ILogger<ChatCommand> _logger;

    public MessageReference MessageReference => new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id);

    public ChatCommand(
        IChatGptHttpClient chatService, 
        ILogger<ChatCommand> logger, 
        IUserRepository userRequestRepository, 
        IBotRepository botResponseRepository)
    {
        _chatService = chatService;
        _logger = logger;
        _userRequestRepository = userRequestRepository;
        _botResponseRepository = botResponseRepository;
    }

    [Command("chat")]
    [Summary("Tirar dúvidas com um modelo de IA")]
    public async Task ExecuteAsync([Remainder] string userMessage)
    {
        try
        {
            var userRequest = new UserRequest
            {
                UserId = Context.User.Id.ToString(),
                Message = userMessage,
                Timestamp = DateTime.UtcNow
            };

            await _userRequestRepository.AddAsync(userRequest);

            var prompt = "Responda essa mensagem em até 2000 caracteres: " + userMessage;

            var request = new ChatRequest.Send
            {
                model = "llama-3.1-70b",
                messages = new List<ChatRequest.Message>
                { new ChatRequest.Message { role = "system", content = prompt } }
            };

            var response = await _chatService.SendChatRequestAsync("chat/completions", request);

            var botResponse = new BotResponse
            {
                UserRequestId = userRequest.Id,
                Response = response.Choices[0].Message.Content,
                Timestamp = DateTime.UtcNow
            };

            await _botResponseRepository.AddAsync(botResponse);

            await ReplyAsync(response.Choices[0].Message.Content, messageReference: MessageReference);
        }
        catch (Exception ex)
        {
            await ReplyAsync($"Ocorreu um erro. Entre em contato com o Administrador.", messageReference: MessageReference);
            _logger.LogError($"Segue erro: {ex.Message}");
        }
    }
}
