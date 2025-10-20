using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var result = 1.into(x => x + 2).into(x => x * 3);
            Console.WriteLine(result());

            var o = Optional<int>.of(5);
            o.Match(
                some => Console.WriteLine($"Value: {some}"),
                () => Console.WriteLine("No value")
            );
        }
    }

    public class Person(string name, string firstName, int age, string? email)
    {
        public string Name { get; } = name;
        public string FirstName { get; } = firstName;
        public int Age { get; } = age;
        public string? Email { get; } = email;

        public override string ToString()
        {
            return $"Name: {Name}, FirstName: {FirstName}, Age: {Age}, Email: {Email ?? "N/A"}";
        }


        public static implicit operator int(Person data)
        {
            return data.Age;
        }

        public static implicit operator string(Person data)
        {
            return data.Name + " " + data.FirstName;
        }

        public string Category()
        {
            return Age switch
            {
                < 13 => "Child",
                < 20 => "Teenager",
                < 65 => "Adult",
                _ => "Senior"
            };
        }
    }

    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultValue)
        {
            if (dict.TryGetValue(key, out TValue value))
            {
                return value;
            }
            return defaultValue();
        }
    }


    public static class FunctionalExtensions
    {
        public static Func<T2, TResult> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1)
        {
            return (T2 arg2) => func(arg1, arg2);
        }

        public static Func<TResult> into<T1>(this T1, Func<T1, TResult> func)
        {
            return () => func(arg1);
        }

        public static Func<TResult> into<T1>(this Func<T1>, Func<T1, TResult> func)
        {
            return () => func(arg1);
        }



        public static Func<T1, TResult> memoize<T1, TResult>(this Func<T1, TResult> func)
        where T1 : notnull
        {
            var cache = new ConcurrentDictionary<T1, TResult>();
        
            return arg1 => cache.GetOrAdd(arg1, func);
        }
    }


    public class Optional<T>(T value)
    {
        public static Optional<T> of(T value) => new Optional<T>(value);
        public static Optional<T> none => new Optional<T>(default!);

        private readonly T _value = value;

        public bool HasValue => _value != null;

        public T ValueOrDefault(T defaultValue)
        {
            return HasValue ? _value : defaultValue;
        }

        public TResult Match<TResult>(Func<T, TResult> someFunc, Func<TResult> noneFunc)
        {
            return HasValue ? someFunc(_value) : noneFunc();
        }
    }
}