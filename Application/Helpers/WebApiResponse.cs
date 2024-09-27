using Application.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Helpers;

public class WebApiResponse
{
    /// <summary>
    /// Creates an ActionResult from a service Result
    /// </summary>
    /// <returns>The action result.</returns>
    /// <param name="result">Service Result.</param>
    /// <typeparam name="T">The data type of the Result.</typeparam>
    public static ActionResult GetErrorResponse<T>(Result<T> result) where T : class
    {
        switch (result.ResultType)
        {
            case ResultType.Unexpected:
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            case ResultType.NotFound:
                return new NotFoundObjectResult(new { Success = false, result.Errors });
            case ResultType.Unauthorized:
                return new UnauthorizedObjectResult(new { Success = false, result.Errors });
            case ResultType.Invalid:
                return new BadRequestObjectResult(new { Success = false, result.Errors });
            default:
                throw new Exception("An unhandled result has occurred as a result of a service call.");
        }
    }
}