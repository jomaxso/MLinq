using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using MLinq;

namespace System.Linq;

public static class RefExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<T> AsRef<T>(this T? value) => Ref<T>.Create(value);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<T> When<T>(this Ref<T> value, Func<T, bool> predicate) =>
        value.HasValue && predicate(value.Value) ? value : Ref<T>.None;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<Task<T?>> When<T>(this Ref<T> value, Func<T, ValueTask<bool>> predicate) =>
        WhenAsyncBuilder(value.HasValue, value.Value, predicate);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async Task<T?> WhenAsyncBuilder<T>(bool hasValue, T? value, Func<T, ValueTask<bool>> predicate) =>
        hasValue && await predicate(value!) ? value : default;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<Task<T?>> When<T>(this Ref<Task<T?>> value, Func<T, bool> predicate) =>
        WhenAsyncBuilder(value.HasValue, value.Value, predicate);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async Task<T?> WhenAsyncBuilder<T>(bool hasValue, Task<T?>? value, Func<T, bool> predicate)
    {
        var result = await value!;
        return hasValue && predicate(result!) ? result : default;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<TOut> Map<TIn, TOut>(this Ref<TIn> value, Func<TIn, TOut> selector) =>
        value.HasValue ? selector(value.Value) : Ref<TOut>.None;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<ValueTask<TOut?>> Map<TIn, TOut>(this Ref<ValueTask<TIn?>> value, Func<TIn, TOut> selector) =>
        MapBuilder(value.HasValue, value.Value, selector).Preserve();
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<TOut?> MapBuilder<TIn, TOut>(bool hasValue, ValueTask<TIn?> value, Func<TIn, TOut> selector) =>
        hasValue ? selector((await value)!) : default;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<ValueTask<TOut?>> Map<TIn, TOut>(this Ref<ValueTask<TIn?>> value, Func<TIn, ValueTask<TOut>> selector) =>
        MapBuilder(value.HasValue, value.Value, selector).Preserve();
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<TOut?> MapBuilder<TIn, TOut>(bool hasValue, ValueTask<TIn?> value, Func<TIn, ValueTask<TOut>> selector) =>
        hasValue ? await selector((await value)!) : default;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<ValueTask<TOut?>> Map<TIn, TOut>(this Ref<Task<TIn?>> value, Func<TIn, TOut> selector) =>
        MapBuilder(value.HasValue, value.Value, selector).Preserve();
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<TOut?> MapBuilder<TIn, TOut>(bool hasValue, Task<TIn?>? value, Func<TIn, TOut> selector) =>
        hasValue ? selector((await value!)!) : default;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<ValueTask<TOut?>> Map<TIn, TOut>(this Ref<Task<TIn?>> value, Func<TIn, ValueTask<TOut>> selector) =>
        MapBuilder(value.HasValue, value.Value, selector).Preserve();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<TOut?> MapBuilder<TIn, TOut>(bool hasValue, Task<TIn?>? value, Func<TIn, ValueTask<TOut>> selector) =>
        hasValue ? await selector((await value!)!) : default;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<ValueTask<TOut?>> Map<TIn, TOut>(this Ref<TIn> value, Func<TIn, ValueTask<TOut>> selector) =>
        (value.HasValue ? selector(value.Value).Preserve() : ValueTask.FromResult(default(TOut?)).Preserve()!)
        .Preserve()!;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<T?> ValueOrDefaultAsync<T>(this Ref<ValueTask<T>> value) => 
        ValueOrDefaultAsyncBuilder(value.HasValue, value.Value!);
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<T?> ValueOrDefaultAsyncBuilder<T>(bool hasValue, ValueTask<T?> value) =>
        hasValue ? await value : default(T?);
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<T> ValueOrDefaultAsync<T>(this Ref<ValueTask<T?>> value, T defaultValue) =>
        ValueOrDefaultAsyncBuilder(value.HasValue, value.Value, defaultValue);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<T> ValueOrDefaultAsyncBuilder<T>(bool hasValue, ValueTask<T?> value, T defaultValue) =>
        hasValue ? await value ?? defaultValue : defaultValue;
    
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<T> ValueOrDefaultAsync<T>(this Ref<ValueTask<T?>> value, Func<T> factory) =>
        ValueOrDefaultAsyncBuilder(value.HasValue, value.Value, factory);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async ValueTask<T> ValueOrDefaultAsyncBuilder<T>(bool hasValue, ValueTask<T?> value, Func<T> factory) =>
        hasValue ? await value ?? factory() : factory();
}