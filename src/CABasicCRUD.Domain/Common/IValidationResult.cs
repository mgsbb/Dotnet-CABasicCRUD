namespace CABasicCRUD.Domain.Common;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation error occurred"
    );

    List<Error> Errors { get; }
}
