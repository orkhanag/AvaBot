using System.Net;

namespace AvaBot.Domain.Common;

public class ErrorCodeModel
{
    public string Code { get; set; }
    
    public HttpStatusCode StatusCode { get; set; }
    
    public string? Message { get; set; }

    public ErrorCodeModel(string code, HttpStatusCode statusCode, string message = null)
    {
        Code = code;
        Message = message;
        StatusCode = statusCode;
    }
}