---
name: csharp-pattern-matching
description: "Concevoir, refactorer, expliquer et tester le pattern matching C# : expressions et instructions switch, type/declaration, constant, relational, logical, property, positional, var, discard, list et slice patterns, gardes when, nullabilité, exhaustivité et ordre des branches. Use when working on C# pattern matching or switch logic."
argument-hint: "Décrivez le code C# ou la logique de pattern matching à traiter"
---

# C# Pattern Matching

Utiliser cette skill pour écrire, modifier, relire ou expliquer du pattern matching dans ce projet .NET 8. Le niveau de langage par défaut est C# 12 ; ne pas introduire une syntaxe plus récente sans vérifier explicitement `<LangVersion>` et la compatibilité du SDK.

## Objectifs

- Exprimer clairement une décision fondée sur la forme, le type ou la valeur d'une donnée.
- Préserver la sémantique, la nullabilité et l'ordre de priorité des cas.
- Préférer une forme exhaustive et testable à une succession fragile de conditions.
- Garder les patterns lisibles et déplacer les calculs complexes dans des méthodes nommées.

## Procédure

1. Identifier le type statique de la valeur testée, sa nullabilité et toutes les formes d'entrée attendues.
2. Relever l'ordre métier des cas. Placer les cas les plus spécifiques avant les cas plus larges.
3. Choisir le pattern le plus direct à l'aide du guide ci-dessous.
4. Décider entre une expression `switch`, une instruction `switch` ou `is` selon le résultat attendu.
5. Vérifier les cas `null`, les bornes, les branches inaccessibles et l'exhaustivité.
6. Ajouter des tests xUnit pour chaque catégorie, frontière et fallback.
7. Exécuter le test ciblé, puis `dotnet test ConsoleApp1.Tests/ConsoleApp1.Tests.csproj` pour une modification partagée.

## Choisir la construction

### Expression `is`

Utiliser `is` pour un prédicat local ou pour extraire une valeur dans un flux de contrôle court.

```csharp
if (value is string { Length: > 0 } text)
{
    Console.WriteLine(text);
}
```

Une déclaration comme `value is string text` implique que `value` n'est pas `null` dans la branche vraie.

### Expression `switch`

Utiliser une expression `switch` lorsqu'une valeur unique doit être calculée. C'est la forme privilégiée pour les méthodes de classification pures, comme `Person.Category()`.

```csharp
return age switch
{
    < 0 => throw new ArgumentOutOfRangeException(nameof(age)),
    < 13 => "Child",
    < 20 => "Teenager",
    < 65 => "Adult",
    _ => "Senior"
};
```

### Instruction `switch`

Utiliser une instruction `switch` lorsque chaque branche exécute plusieurs actions, modifie un état ou nécessite un contrôle de flux explicite. Ne pas la choisir uniquement pour éviter de retourner une valeur depuis une expression `switch`.

## Catalogue des patterns

### Constant et `null`

```csharp
value is null
value is not null
status is Status.Active
```

Préférer `is null` et `is not null` pour exprimer un test de nullité dans une logique de pattern matching. Ne pas utiliser `{ }` comme substitut cryptique de `not null`.

### Type et déclaration

```csharp
input switch
{
    int number => number.ToString(),
    string text => text,
    null => "",
    _ => throw new ArgumentException("Unsupported input", nameof(input))
};
```

Ne déclarer une variable de pattern que si elle est utilisée. Sinon, employer uniquement le type, par exemple `input is string`.

### Relationnel

```csharp
temperature switch
{
    < 0 => "Freezing",
    <= 20 => "Cool",
    <= 30 => "Warm",
    _ => "Hot"
};
```

Traiter explicitement les valeurs invalides lorsque le domaine en possède. Tester exactement les bornes et leurs valeurs adjacentes.

### Logique : `and`, `or`, `not`

```csharp
score is >= 0 and <= 100
day is DayOfWeek.Saturday or DayOfWeek.Sunday
value is not null
```

La précédence est `not`, puis `and`, puis `or`. Ajouter des parenthèses dès que leur absence rend l'intention discutable.

Une variable ne peut pas être supposée assignée après des alternatives `or` qui ne la produisent pas de manière identique. Scinder les branches si chaque alternative extrait des données différentes.

### Propriété

```csharp
person switch
{
    { Email: not null, Age: >= 18 } => "Contactable adult",
    { Age: < 18 } => "Minor",
    null => "Missing",
    _ => "Adult without email"
};
```

Utiliser les extended property patterns pour une propriété imbriquée simple, par exemple `{ Address.Country: "FR" }`. Préférer des patterns imbriqués explicites lorsque plusieurs niveaux doivent être lus ensemble.

Les patterns de propriété échouent si la valeur testée, ou une propriété intermédiaire requise, vaut `null`. Garder tout de même une branche `null` explicite lorsque ce cas appartient au domaine.

### Positionnel

```csharp
point switch
{
    (0, 0) => "Origin",
    (0, _) => "Y axis",
    (_, 0) => "X axis",
    _ => "Plane"
};
```

Utiliser les patterns positionnels pour les tuples, records ou types dotés d'un `Deconstruct` stable et significatif. Préférer les property patterns si le nom des membres rend la règle métier plus claire.

### `var` et discard

```csharp
value switch
{
    int number when number > 0 => number,
    var other => Transform(other)
};
```

Le pattern `var` correspond aussi à `null`. Le discard `_` est le fallback sans capture. Préférer `_` lorsqu'aucune valeur n'est nécessaire.

### Liste et slice

```csharp
values switch
{
    [] => "Empty",
    [var single] => $"One: {single}",
    [0, .. var rest] => $"Starts with zero; remaining: {rest.Length}",
    [var first, .., var last] => $"From {first} to {last}"
};
```

Employer les list patterns pour reconnaître une forme de séquence, pas pour remplacer une recherche ou une agrégation LINQ. Vérifier que le type possède la prise en charge nécessaire pour la longueur et l'indexation. Un slice pattern `..` ne peut apparaître qu'une fois dans un list pattern.

### Garde `when`

```csharp
order switch
{
    { Total: > 0 } when order.IsPreferredCustomer => "Preferred",
    { Total: > 0 } => "Standard",
    _ => "Invalid"
};
```

Réserver `when` aux conditions qui ne s'expriment pas clairement comme patterns. Une garde peut appeler du code : vérifier son coût, ses effets de bord et les exceptions possibles.

## Ordre et exhaustivité

- Les branches sont évaluées dans l'ordre. Une branche générale ne doit pas masquer une branche spécifique.
- Les patterns relationnels doivent former des intervalles compréhensibles, sans trous involontaires.
- Pour un ensemble fermé connu, lister les cas afin que le compilateur aide lors de son évolution.
- Pour une entrée externe ou ouverte, ajouter un fallback explicite : valeur par défaut contrôlée ou exception métier pertinente.
- Ne pas ajouter `_` mécaniquement si une exception sur une nouvelle valeur d'enum est préférable à un résultat silencieux.
- Examiner les avertissements du compilateur concernant les branches inaccessibles et les expressions non exhaustives ; ne pas les supprimer sans corriger la logique.

## Nullabilité

- Modéliser `null` dans le type (`T?`) et dans le `switch` lorsque le domaine l'autorise.
- Après `value is SomeType item`, utiliser `item` plutôt qu'un cast répété.
- Ne pas employer l'opérateur null-forgiving `!` pour faire accepter un pattern incorrect au compilateur.
- Pour un générique non contraint, ne pas supposer que `default(T)` signifie toujours `null`.

## Lisibilité et effets de bord

- Garder les bras de `switch` courts. Extraire une méthode lorsque le résultat demande plusieurs opérations.
- Éviter les appels ayant des effets de bord dans les propriétés ou gardes évaluées par un pattern.
- Ne pas dépendre de l'ordre exact dans lequel les sous-patterns frères lisent les propriétés.
- Conserver les expressions de classification pures lorsque le code existant l'est déjà.
- Remplacer une chaîne de `if` seulement si le `switch` rend les catégories et leur priorité plus visibles.

## Tests requis

Pour chaque modification de pattern matching, couvrir selon le cas :

- chaque branche nominale ;
- les valeurs exactement sur les bornes ;
- une valeur de part et d'autre de chaque borne ;
- `null` et `default` lorsque le type les autorise ;
- le fallback ou l'exception attendue ;
- les séquences vide, à un élément et à plusieurs éléments pour les list patterns ;
- les combinaisons qui distinguent `and` de `or` ;
- tout cas auparavant masqué par une branche plus générale.

Nommer les tests selon le comportement et le scénario, par exemple `Category_NegativeAge_Throws` ou `Describe_EmptyList_ReturnsEmpty`.

## Revue finale

Avant de terminer :

1. Confirmer que la syntaxe est compatible avec C# 12 et .NET 8.
2. Relire les branches de la plus spécifique à la plus générale.
3. Vérifier `null`, les bornes et le fallback.
4. Compiler afin de détecter les branches inaccessibles et les problèmes d'exhaustivité.
5. Exécuter les tests ciblés puis le projet de tests complet.