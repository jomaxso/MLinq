using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace MLinq;

public readonly record struct Result
{
    public Result() : this(Error.None)
    {
    }

    private Result(Error? error) =>
        Error = error ?? Error.None;

    public Error Error { get; }

    [Pure]
    public bool IsSuccess => Error == Error.None;
    
    [Pure]
    public bool IsFailure => IsSuccess is false;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result(Error error) => Failure(error);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Success() => new();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result Failure(Error? error) => new(error);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsError(Error? error) =>
        error is not null && error != Error.None;
}