---
name: csharp-best-practices
description: "Appliquer les bonnes pratiques C# et .NET: écrire, relire, refactorer ou corriger du code en respectant les contrats, la nullabilité, les API, les collections, l’asynchronisme, les tests et les conventions du dépôt."
argument-hint: "Fichier, méthode ou changement C# à examiner"
---

# Bonnes pratiques C#

Utiliser cette skill pour écrire, relire, expliquer, refactorer ou corriger du code C# dans différents projets et versions de .NET. Adapter les recommandations au code, au contrat et à l’environnement détectés; ne pas appliquer mécaniquement une règle de style ni élargir le changement demandé.

## Détecter le contexte

- Lire les instructions du dépôt (`AGENTS.md`, `CONTRIBUTING.md` ou équivalent), les fichiers projet et les tests voisins avant de choisir une convention.
- Déterminer les frameworks cibles, le SDK et la version de langage à partir de `global.json`, des fichiers `.csproj`, `Directory.Build.props` et de la configuration pertinente. Ne pas supposer qu’une API .NET ou une syntaxe C# récente est disponible.
- Repérer l’état de la nullabilité, les analyseurs, le style de namespace, les conventions d’API, le framework de test et les commandes de validation déjà utilisés.
- En cas de conflit, privilégier les contrats publics et les contraintes explicites du dépôt; signaler les ambiguïtés qui empêchent une décision sûre.

## Procédure

1. Lire les conventions du dépôt et le contexte technique détecté, puis examiner l’implémentation concernée, les appelants directs et les tests existants.
2. Décrire le comportement attendu et le contrat actuel: entrées, valeurs nulles ou par défaut, effets de bord, exceptions, ordre, évaluation différée, durée de vie et compatibilité publique, selon le sujet.
3. Formuler une hypothèse locale vérifiable sur le défaut ou le besoin, puis choisir le contrôle le moins coûteux qui pourrait la réfuter.
4. Modifier le code à la frontière qui contrôle réellement le comportement. Préférer le plus petit changement cohérent, réutiliser les abstractions disponibles dans la cible et éviter les refactorisations sans lien.
5. Vérifier les risques pertinents: analyse de nullabilité, disposition des ressources, coût et réénumération des séquences, comportement des collections, concurrence, effets des callbacks et compatibilité de la syntaxe/API avec les frameworks cibles.
6. Ajouter ou ajuster des tests dans le framework déjà adopté, centrés sur le comportement public. Couvrir les cas limites applicables, par exemple entrée vide, absence de correspondance, null/default, évaluation différée et appels répétés.
7. Exécuter d’abord la validation ciblée, puis la compilation et les tests pertinents à la portée du changement. Utiliser les commandes documentées par le dépôt et cibler explicitement les projets de test si la solution ne les inclut pas.
8. Résumer le changement, ses garanties et les validations réellement exécutées. Signaler les hypothèses restantes au lieu de les présenter comme des garanties.

## Repères de conception

- Choisir des noms qui décrivent l’intention et des méthodes petites, avec une responsabilité cohérente. Extraire une abstraction seulement si elle clarifie une règle métier ou retire une duplication significative.
- Garder la nullabilité explicite. Corriger les avertissements à leur source; ne pas masquer un contrat incertain avec `!` sans justification.
- Traiter séparément `null`, `default(T)` et l’absence de valeur pour les types génériques. Ne pas supposer qu’une valeur par défaut est toujours `null`.
- Pour les séquences différées telles que `IEnumerable<T>`, préciser si l’énumération peut être répétée, si la source peut être coûteuse ou à usage unique, et à quel moment les exceptions surviennent. Matérialiser uniquement si le comportement le justifie, en tenant compte du coût mémoire.
- Employer les collections concurrentes selon leurs garanties réelles. Par exemple, une fabrique passée à `ConcurrentDictionary.GetOrAdd` peut être appelée plusieurs fois lors d’accès concurrents à une clé absente; ne pas promettre une exécution unique sans mécanisme supplémentaire.
- Pour les ressources possédées par le code, utiliser `using` ou `await using` selon le contrat de disposition. Ne pas disposer un objet dont la propriété appartient à l’appelant.
- Dans le code asynchrone, éviter le blocage synchrone (`.Result`, `.Wait()`) dans les chemins asynchrones; propager les exceptions et l’annulation lorsque le contrat et les API les exposent. Réserver `async void` aux gestionnaires d’événements requis par leur signature.
- Lever des exceptions adaptées au contrat et valider les arguments à la frontière appropriée. Éviter de convertir silencieusement une erreur en valeur par défaut.
- Analyser les coûts des boucles, des API de collections et des callbacks avant de proposer une optimisation. Les benchmarks servent à comparer des scénarios représentatifs, pas à prouver une classe asymptotique.

## Critères de fin

- Le changement respecte le comportement attendu et ne casse pas un contrat public sans l’indiquer.
- Les diagnostics de nullabilité et de compilation pertinents sont traités.
- Les tests ciblés couvrent le comportement modifié et les cas limites applicables.
- Les validations rapportées correspondent exactement aux commandes exécutées; les risques non vérifiés sont explicites.