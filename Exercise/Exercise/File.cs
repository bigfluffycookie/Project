namespace Exercise
{
    public class File : IFile
    {
        public string FilePath { get; }
        public string[] FileContent { get; }

        public File(string filePath, string[] fileContent)
        {
            FilePath = filePath;
            FileContent = fileContent;
        }
    }
}