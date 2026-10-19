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

    /// Implémentation d'une jointure gauche (Left Join) entre deux séquences.
    /// Attention : cette implémentation utilise `FirstOrDefault` pour chaque élément de la séquence de gauche,
    /// ce qui la rend pas assez performante pour de grandes séquences.
    public static IEnumerable<(T1, T2?)> LeftJoin<T1, T2, T3>(this IEnumerable<T1> items, IEnumerable<T2> others, Func<T1, T2, bool> predicate)
    {
        foreach (var item in items)
        {
            var matched = others.FirstOrDefault(o => predicate(item, o));
            yield return (item, matched);
        }
    }

    /// Implémentation d'une jointure gauche (Left Join) entre deux séquences basée sur une clé.
    /// Attention : cette implémentation garde uniquement le premier élément correspondant de la séquence de droite pour chaque clé.
    /// @param items La séquence de gauche.
    /// @param others La séquence de droite.
    /// @param leftKeySelector Fonction pour extraire la clé de la séquence de gauche.
    /// @param rightKeySelector Fonction pour extraire la clé de la séquence de droite.
    /// @returns Une séquence de tuples contenant l'élément de gauche et le premier élément correspondant de droite (ou null si aucun correspondant).
    public static IEnumerable<(TLeft Left, TRight? Right)> LeftJoinBy<TLeft, TRight, TKey>(
        this IEnumerable<TLeft> items,
        IEnumerable<TRight> others,
        Func<TLeft, TKey> leftKeySelector,
        Func<TRight, TKey> rightKeySelector)
        where TKey : notnull
    {
        Dictionary<TKey, TRight>? firstByKey = null;

        foreach (var item in items)
        {
            if (firstByKey is null)
            {
                firstByKey = new Dictionary<TKey, TRight>();
                foreach (var other in others)
                    firstByKey.TryAdd(rightKeySelector(other), other);
            }

            firstByKey.TryGetValue(leftKeySelector(item), out var matched);
            yield return (item, matched);
        }
    }

}