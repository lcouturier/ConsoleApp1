---
name: big-o-analysis
description: 'Analyser la complexité Big O, temporelle et spatiale, des algorithmes C# de ce projet. Utiliser pour examiner une méthode, comparer des implémentations, repérer des boucles imbriquées, des parcours LINQ répétés, de la récursion ou des coûts cachés liés aux collections et à l’évaluation différée.'
argument-hint: 'Méthode, fichier ou algorithmes à analyser'
---

# Analyse de complexité des algorithmes

Produire une analyse justifiée par le code du projet, en distinguant les coûts certains des hypothèses sur les entrées, les collections, les délégués et les API .NET. Ne pas modifier le code sauf demande explicite.

## Procédure

1. Délimiter la méthode et son chemin d’appel pertinent. Analyser le code exécutable, pas les blocs commentés. Utiliser les tests comme indices de scénarios et de comportement, pas comme preuve de complexité.
2. Nommer les tailles d’entrée indépendantes (`n`, `m`, etc.) et préciser ce qu’elles mesurent. Si une séquence est différée, analyser le coût au moment de son énumération et indiquer si plusieurs énumérations relancent le calcul.
3. Compter les opérations dominantes: passages de boucles, comparaisons de prédicats, appels récursifs, allocations et structures de données. Examiner les opérateurs LINQ, les méthodes de collection et les callbacks au lieu de traiter chaque appel comme un coût constant.
4. Donner la complexité temporelle asymptotique, généralement au pire cas. Donner les cas moyen ou meilleur seulement si le code et les hypothèses les justifient. Utiliser `O` pour une borne supérieure et `Theta` lorsque la borne est serrée et démontrée.
5. Donner l’espace auxiliaire séparément de la mémoire des entrées et du résultat. Pour les itérateurs différés, signaler l’état conservé par l’itérateur; pour les résultats matérialisés, inclure leur taille.
6. Exposer les hypothèses et les cas limites qui changent l’analyse: séquences vides, arrêt anticipé, collection réénumérable ou à usage unique, prédicat coûteux, collisions de hachage, et coûts délégués à une API .NET.
7. Présenter les résultats dans un tableau concis: méthode, tailles, temps, espace auxiliaire et hypothèses. Terminer par les goulots d’étranglement avérés et, si utile, une piste d’amélioration avec son compromis. Ne pas inférer une amélioration à partir d’un benchmark isolé.

## Règles d’analyse

- Les boucles séquentielles s’additionnent; des parcours imbriqués ou répétés se multiplient selon leurs tailles. Pour une boucle dont le nombre d’itérations dépend de l’indice, calculer la somme plutôt que supposer automatiquement un carré.
- Pour `FirstOrDefault` et les autres recherches, distinguer arrêt anticipé et parcours complet. Dans une boucle externe, compter le nombre de fois que la recherche interne est relancée.
- Inclure le coût des fonctions passées en argument. Si ce coût est inconnu, le noter `C_predicate` ou `C_f` et donner aussi la complexité hors callback.
- Une recherche par clé dans un dictionnaire ou un ensemble est généralement attendue en O(1), sous les hypothèses usuelles de hachage; mentionner le pire cas et le coût de la fonction de hachage si cela importe.
- Pour la récursion, écrire la récurrence ou compter le travail par niveau et la profondeur. Ne pas attribuer au code appelant la récursion d’une API déléguée sans l’établir.
- Distinguer le nombre d’éléments parcourus du coût des opérations d’E/S. Pour le système de fichiers, exprimer le travail en nombre d’entrées et préciser que les latences réelles dépendent du stockage et de l’API.
- Éviter les mesures de temps comme preuve d’une classe asymptotique. Si une affirmation de performance doit être vérifiée, préférer un test ciblé qui compte les énumérations ou les appels au prédicat; utiliser un benchmark seulement pour comparer des implémentations représentatives.

## Qualité attendue

- Chaque classe asymptotique découle d’une opération ou d’une structure visible dans le code.
- Les entrées de tailles différentes ne sont pas fusionnées en un seul `n` sans justification.
- Temps, espace auxiliaire, résultat et coûts d’API/callback sont clairement séparés.
- Les hypothèses, l’arrêt anticipé et l’évaluation différée sont explicites.
- La conclusion est proportionnée aux preuves; aucune mesure de performance ou optimisation proposée n’est présentée comme une certitude asymptotique sans raisonnement.