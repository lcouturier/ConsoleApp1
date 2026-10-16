using ConsoleApp1;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class CleanerTest : IDisposable
    {
        private readonly string directoryPath = Path.Combine(
            Path.GetTempPath(),
            $"ConsoleApp1.Tests-{Guid.NewGuid():N}");

        [Fact]
        public void Clean_WithFilesAndSubdirectories_RemovesContentsAndPreservesRoot()
        {
            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(Path.Combine(directoryPath, "root.txt"), "root");

            var nestedDirectoryPath = Directory.CreateDirectory(
                Path.Combine(directoryPath, "nested", "child")).FullName;
            File.WriteAllText(Path.Combine(nestedDirectoryPath, "nested.txt"), "nested");

            Cleaner.Clean(directoryPath);

            Assert.True(Directory.Exists(directoryPath));
            Assert.Empty(Directory.EnumerateFileSystemEntries(directoryPath));
        }

        [Fact]
        public void Clean_EmptyDirectory_PreservesDirectory()
        {
            Directory.CreateDirectory(directoryPath);

            Cleaner.Clean(directoryPath);

            Assert.True(Directory.Exists(directoryPath));
            Assert.Empty(Directory.EnumerateFileSystemEntries(directoryPath));
        }

        [Fact]
        public void Clean_MissingDirectory_ThrowsDirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() => Cleaner.Clean(directoryPath));
        }

        public void Dispose()
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, recursive: true);
            }
        }
    }
}