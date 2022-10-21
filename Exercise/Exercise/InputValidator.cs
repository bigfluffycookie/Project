using System.IO.Abstractions;
using System.Runtime.CompilerServices;

namespace Exercise
{
    public class InputValidator
    {
        private readonly IFileSystem fileSystem;

        public InputValidator() : this(new FileSystem()) { }

        public InputValidator(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool FileHasCorrectExtension(string filePath, string fileExtension)
        {
            if (fileSystem.Path.GetExtension(filePath) == fileExtension)
            {
                return true;
            }

            Console.WriteLine("File does not have the requested extension: " + fileExtension);

            return false;
        }

        public bool FileExists(string filePath)
        {
            if (fileSystem.File.Exists(filePath))
            {
                return true;
            }

            Console.WriteLine("File does not Exist. Please Try Again.");

            return false;
        }
    }
}