using System.Net;

namespace AvaBot.Domain.Common;

public static class ErrorCodes
{
    private const string Root = "CUS";

    public static class Common
    {
        public const string Category = Root + "_" + "COM";

        public static readonly ErrorCodeModel ValidationError =
            new ErrorCodeModel($"{Category}_VALIDATION_ERROR", HttpStatusCode.UnprocessableEntity);

        public static readonly ErrorCodeModel PayloadTooLarge =
            new ErrorCodeModel($"PAYLOAD_TOO_LARGE",
                HttpStatusCode.RequestEntityTooLarge,
                "Payload too large.");

        public static readonly ErrorCodeModel EmptyField =
            new ErrorCodeModel($"FIELD_EMPTY",
                HttpStatusCode.RequestEntityTooLarge,
                "This field is required!");
    }
}