namespace Marcia.Bot.Domain.Models;

public class ChatRequest
{
    public class Send
    {
        public string? model { get; set; }
        public List<Message>? messages { get; set; }
    }

    public class Message
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }
}
