namespace AvaBot.Application.Common.Models.Wrappers;
public class ValidationError
{
    public string? Code { get; }
    public string? Field { get; }
    public string Message { get; }

    public ValidationError(string field, string message, string? code = null)
    {
        Field = field != string.Empty ? field : null;
        Message = message;
        Code = code;
    }
}
