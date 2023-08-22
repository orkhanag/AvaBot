using AvaBot.Application.Common.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace AvaBot.API.Extensions;

public static class EitherOneExtensions
{
    public static IActionResult MatchResponse<T1, T2>(this EitherOne<T1, T2> eitherOne)
    {
        return eitherOne.Match(
            MatchPotentialErrorResponse,
            MatchPotentialErrorResponse);
    }
    private static IActionResult MatchPotentialErrorResponse<T>(T t)
    {
        if (t is APIError apiError)
        {
            return new ObjectResult(apiError) { StatusCode = (int)apiError.Status };
        }
        return new OkObjectResult(t);
    }
}