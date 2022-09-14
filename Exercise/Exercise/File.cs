namespace Exercise
{
    public class File
    {
        public string FilePath { get; private set; }
        public string[] FileContent { get; private set; }

        public File(string filePath)
        {
            FilePath = filePath;
            FileContent = System.IO.File.ReadAllLines(filePath);
        }
    }
}
