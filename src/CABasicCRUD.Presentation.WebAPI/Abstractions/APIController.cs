using CABasicCRUD.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CABasicCRUD.Presentation.WebAPI.Abstractions;

public abstract class APIController : ControllerBase
{
    protected BadRequestObjectResult HandleBadRequest(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => BadRequest(
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation error",
                    Detail = result.Error?.Message,
                    Type = result.Error?.Code,
                    Extensions =
                    {
                        { nameof(validationResult.Errors).ToLower(), validationResult.Errors },
                    },
                }
            ),
            _ => throw new NotImplementedException(),
        };
    }

    protected ObjectResult HandleProblem(
        int statusCode,
        string? title = null,
        string? detail = null,
        string? type = null,
        string? instance = null
    )
    {
        ProblemDetailsFactory factory =
            HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        ProblemDetails problem = factory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            title: title,
            type: type,
            detail: detail,
            instance: instance
        );

        return new ObjectResult(problem);
    }
}
