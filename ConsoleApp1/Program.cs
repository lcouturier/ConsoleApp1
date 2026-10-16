namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var result = 1.Into(x => x + 2).Into(x => x * 3);
            Console.WriteLine(result());

            var o = Optional.Of(5);
            Console.WriteLine(o.HasValue);
        }
    }



    public static class Cleaner
    {
        /// <summary>
        /// Cleans the specified directory by deleting all files and subdirectories.
        /// </summary>
        /// <param name="path">The path of the directory to clean.</param>
        /// <exception cref="DirectoryNotFoundException">Thrown if the specified directory does not exist.</exception>
        // public static void Clean(String path)
        // {
        //     foreach (var file in Directory.GetFiles(path))
        //     {
        //         File.Delete(file);
        //     }
        //     foreach (var dir in Directory.GetDirectories(path))
        //     {
        //         Clean(dir);
        //         Directory.Delete(dir);
        //     }
        // }


        /// <summary>
        /// Version proposée par copilot
        /// </summary>
        public static void Clean(string path)
        {
            foreach (var entry in new DirectoryInfo(path).EnumerateFileSystemInfos())
            {
                if (entry is DirectoryInfo directory)
                {
                    directory.Delete(recursive: true);
                    continue;
                }

                entry.Delete();
            }
        }
    }
}