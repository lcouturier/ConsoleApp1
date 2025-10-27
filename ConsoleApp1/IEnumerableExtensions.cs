namespace ConsoleApp1;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> SeparatedBy<T>(this IEnumerable<T> items, T separator)
    {
        foreach (var item in items.Indexed())
        {
            if (item.index == 0)
                yield return item.value;
            if (item.index > 0)
            {
                yield return separator;
                yield return item.value;
            }
        }
    }

    public static IEnumerable<(int index, T value)> Indexed<T>(this IEnumerable<T> items)
    {
        int index = 0;
        foreach (var item in items)
        {
            yield return (index: index++, value: item);
        }
    }

    public static IEnumerable<(T1, T2?)> LeftJoin<T1, T2, T3>(this IEnumerable<T1> items, IEnumerable<T2> others, Func<T1, T2, bool> predicate)
    {
       foreach (var item in items)
       {
            var matched = others.FirstOrDefault(o => predicate(item, o));
            yield return (item, matched); 
       }
    }
    
}