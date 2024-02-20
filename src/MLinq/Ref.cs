using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace MLinq;

public readonly ref struct Ref<T>(T? value)
{
    public Ref() : this(default(T?))
    {
    }

    internal T? Value => value;

    [Pure] public static Ref<T> None => new();

    [Pure]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue => Value is not null;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? ValueOrDefault() => Value;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOrDefault(T defaultValue) => Value ?? defaultValue;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOrDefault(Func<T> factory) => Value ?? factory();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Ref<T>(T? value) => Create(value);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Ref<T> Create([AllowNull] T? value) =>
        value is null ? new Ref<T>() : new Ref<T>(value);
}