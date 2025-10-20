namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var result = 1.Into(x => x + 2).Into(x => x * 3);
            Console.WriteLine(result());

            var o = Optional<int>.Of(5);
            Console.WriteLine(o.HasValue);
        }
    }
}