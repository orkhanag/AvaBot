using AvaBot.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Application.Common.Models.Wrappers;
public class APIError : APIResponse
{
    private Domain.Common.ErrorCodeModel payloadTooLarge;

    public string? Code { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; }
    public string? TraceId { get; set; }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }

    public APIError(HttpStatusCode status) : base(false, status)
    {
    }

    public APIError(ErrorCodeModel errorCode) : base(false, errorCode.StatusCode)
    {
        Code = errorCode.Code;
        Message = errorCode.Message;
    }
}
