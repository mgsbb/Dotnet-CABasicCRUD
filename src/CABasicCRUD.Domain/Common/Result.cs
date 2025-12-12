namespace CABasicCRUD.Domain.Common;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; protected set; }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException();

        if (!isSuccess && error == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(Error? error) => new(false, error);
}

public class Result<TValue> : Result
{
    public TValue? Value { get; private set; }

    private Result(bool isSuccess, TValue? value, Error? error)
        : base(isSuccess, error)
    {
        if (isSuccess && value == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<TValue> Success(TValue? value) => new(true, value, null);

    public static new Result<TValue> Failure(Error? error) => new(false, default, error);

    public static implicit operator Result<TValue>(TValue? value) => Success(value);
}
