using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
    }
}