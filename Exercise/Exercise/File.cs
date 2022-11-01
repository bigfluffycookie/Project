using System.IO;
using System;
using System.IO.Abstractions;
using System.Diagnostics;

namespace Exercise
{
    public class File : IFile
    {
        public string FilePath { get; }
        public string[] FileContent { get; }

        public File(string filePath) : this(filePath , new FileSystem()) { }

        public File(string filePath, IFileSystem fileSystem)
        {
            FilePath = filePath;
            FileContent = GetFileContent(filePath, fileSystem);
        }

        private string[] GetFileContent(string filePath, IFileSystem fileSystem)
        {
            var fileContent = Array.Empty<string>();

            try
            {
                fileContent = fileSystem.File.ReadAllLines(filePath);
            }
            catch (IOException e)
            {
                Debug.Write("File could not be read with error message: " + e.Message);
            }

            return fileContent;
        }
    }
}