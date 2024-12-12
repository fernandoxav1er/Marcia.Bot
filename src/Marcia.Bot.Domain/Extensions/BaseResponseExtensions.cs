using Marcia.Bot.Domain.Models;
using System.Net;

namespace Marcia.Bot.Domain.Extensions;

public static class BaseResponseExtensions
{
    public static BaseResponse<T> ToBaseResponse<T>(this T dto, HttpStatusCode statusCode, string? erroRetorno = null) where T : class
    {
        return new BaseResponse<T>
        {
            CodigoHttp = statusCode,
            DadosRetorno = statusCode == HttpStatusCode.OK ? dto : null,
            ErroRetorno = statusCode == HttpStatusCode.OK ? null : erroRetorno
        };
    }
}
