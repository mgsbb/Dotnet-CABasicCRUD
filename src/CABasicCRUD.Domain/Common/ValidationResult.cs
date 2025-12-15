namespace CABasicCRUD.Domain.Common;

public class ValidationResult : Result, IValidationResult
{
    private ValidationResult(List<Error> errors)
        : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public List<Error> Errors { get; }

    public static ValidationResult WithErrors(List<Error> errors) => new(errors);
}

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(List<Error> errors)
        : base(false, default, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public List<Error> Errors { get; }

    public static ValidationResult<TValue> WithErrors(List<Error> errors) => new(errors);
}
