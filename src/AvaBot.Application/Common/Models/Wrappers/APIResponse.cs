using System.Net;

namespace AvaBot.Application.Common.Models.Wrappers;
public abstract class APIResponse
{
    public bool Success { get; }

    public HttpStatusCode Status { get; }

    public APIResponse(bool success, HttpStatusCode status)
    {
        Success = success;
        Status = status;
    }
}
