using System.Diagnostics.CodeAnalysis;

namespace CABasicCRUD.Domain.Common;

public class Result
{
    public bool IsSuccess { get; init; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    public Error? Error { get; init; }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("Success result cannot have an error.");

        if (!isSuccess && error == null)
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(Error? error) => new(false, error);
}

public class Result<TValue> : Result
{
    public TValue? Value { get; init; }

    [MemberNotNullWhen(false, nameof(Value))]
    public new bool IsFailure => base.IsFailure;

    protected Result(bool isSuccess, TValue? value, Error? error)
        : base(isSuccess, error)
    {
        if (isSuccess && value == null)
            throw new InvalidOperationException("Success result must have a value.");

        Value = value;
    }

    public static Result<TValue> Success(TValue? value) => new(true, value, null);

    public static new Result<TValue> Failure(Error? error) => new(false, default, error);

    public static implicit operator Result<TValue>(TValue? value) => Success(value);
}
