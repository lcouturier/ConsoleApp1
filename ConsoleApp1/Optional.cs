namespace ConsoleApp1;

public class Optional<T>(T value)
{
  public static Optional<T> Of(T value) => new(value);
  public static readonly Optional<T> None = new(default!);

  public bool HasValue => value != null;


  public TResult Match<TResult>(Func<T, TResult> someFunc, Func<TResult> noneFunc)
  {
    return HasValue ? someFunc(value) : noneFunc();
  }
}