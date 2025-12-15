using CABasicCRUD.Domain.Common;
using FluentValidation;
using MediatR;

namespace CABasicCRUD.Application.Common.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!_validators.Any())
        {
            return await next();
        }

        List<Error> errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToList();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();

        // var context = new ValidationContext<TRequest>(request);

        // var validationResults = await Task.WhenAll(
        //     _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        // );

        // var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        // if (failures.Count == 0)
        // {
        //     throw new ValidationException(failures);
        // }

        // return await next();
    }

    private static TResult CreateValidationResult<TResult>(List<Error> errors)
        where TResult : Result
    {
        // for Result (without <>)
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        Console.WriteLine(typeof(TResult));

        // TResult here is Result<TValue>.
        // We convert to ValidationResult<TValue> and invoke the WithErros static method with errors parameter.
        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}
