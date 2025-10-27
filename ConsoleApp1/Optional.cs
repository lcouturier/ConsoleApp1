namespace ConsoleApp1;


public static class Optional
{
    public static Optional<T> Of<T>(T value)
    {
        return new Optional<T>(value);
    }

    public static Optional<T> None<T>()
    {
        return Optional<T>.None;
    }
}

public class Optional<T>(T value)
{
    public static readonly Optional<T> None = Optional.None<T>();

    public bool HasValue => value != null;

    public TResult Match<TResult>(Func<T, TResult> someFunc, Func<TResult> noneFunc)
    {
        return HasValue ? someFunc(value) : noneFunc();
    }

    public T OrElse(T other) => HasValue ? value : other;

    public Optional<TResult> Map<TResult>(Func<T, TResult> mapper) =>
        HasValue ? Optional.Of(mapper(value)) : Optional.None<TResult>();
}