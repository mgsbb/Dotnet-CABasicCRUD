using CABasicCRUD.Domain.Common;
using Microsoft.AspNetCore.Mvc;

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
}
