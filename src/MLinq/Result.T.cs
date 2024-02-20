using System.Diagnostics.CodeAnalysis;

namespace MLinq;

public readonly record struct Result<T>
{
    private readonly T? _value;
    
    public Result()
    {
        IsSuccess = false;
        Error = Error.Unknown;
        _value = default;
    }

    public Result(T? value)
    {
        IsSuccess = true;
        Error = Error.None;
        _value = value;
    }

    public Result(Error error)
    {
        IsSuccess = false;
        Error = error == Error.None ? Error.Unknown : error;
        _value = default;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(_value))]
    public bool IsFailure => IsSuccess is false;

    public T? Value => _value;

    public Error Error { get; }

    public T ValueOrDefault(Func<Error, T> failureDefault) =>
        IsSuccess ? _value : failureDefault(Error);

    public T ValueOrDefault(Func<T> failureDefault) =>
        IsSuccess ? _value : failureDefault();

    public T ValueOrDefault(T failureDefault) =>
        IsSuccess ? _value : failureDefault;
    
    public static implicit operator Result(Result<T> result) =>
        result.IsSuccess ? Result.Success() : Result.Failure(result.Error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);

    public static implicit operator T?(Result<T> result) =>
        result.IsSuccess ? result.Value : throw new Exception();

    public static implicit operator Error(Result<T> result) => result.Error;
    
    public static implicit operator Ref<T>(Result<T> result) =>
        result.IsSuccess ? result.Value : Ref<T>.None;

    internal static Result<T> Success(T value) => new(value);

    internal static Result<T> Failure([DisallowNull] Error error) => new(error);
}