using System.Net;

namespace Marcia.Bot.Domain.Models;

public class BaseResponse<T> where T : class
{
    public HttpStatusCode CodigoHttp { get; set; }
    public T? DadosRetorno { get; set; }
    public string? ErroRetorno { get; set; }
}
