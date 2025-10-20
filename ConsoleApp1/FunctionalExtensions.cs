using System.Collections.Concurrent;

namespace ConsoleApp1;

public static class FunctionalExtensions
{
  public static Func<T2, TResult> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1)
  {
    return arg2 => func(arg1, arg2);
  }

  

  public static Func<TResult> Into<T1, TResult>(this T1 value, Func<T1, TResult> func)
  {
    return () => func(value);
  }

  public static Func<TResult> Into<T1, TResult>(this Func<T1> value, Func<T1, TResult> func)
  {
    return () => func(value());
  }

  public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> func)
    where T1 : notnull
  {
    var cache = new ConcurrentDictionary<T1, TResult>();
        
    return arg1 => cache.GetOrAdd(arg1, func);
  }
}