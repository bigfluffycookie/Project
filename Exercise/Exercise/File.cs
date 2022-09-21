namespace Exercise
{
    public class File
    {
        public string FilePath { get; private set; }
        public string[] FileContent { get; private set; }

        public File(string filePath, string[] fileContent)
        {
            FilePath = filePath;
            FileContent = fileContent;
        }
    }
}
