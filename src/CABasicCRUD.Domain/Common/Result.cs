namespace CABasicCRUD.Domain.Common;

public class Result<TValue>
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; private set; }
    public TValue? Value { get; private set; }

    private Result(bool isSuccess, TValue? value, Error? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException();

        if (isSuccess && value == null)
            throw new InvalidOperationException();

        if (!isSuccess && error == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<TValue> Success(TValue? value) => new(true, value, null);

    public static Result<TValue> Failure(Error? error) => new(false, default, error);

    public static implicit operator Result<TValue>(TValue? value) => Success(value);
}
