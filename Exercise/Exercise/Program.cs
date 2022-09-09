namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            string filePath = ReadUserInputForFilePath(".txt");
            string[] fileContent = GetFileContentFromPath(filePath);
            Analyzer.Analyze(fileContent);

            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static void DisplayWelcomeText()
        {
            Console.WriteLine("Welcome to the Analyzer");
            Console.WriteLine("Please Input Text File path to Analyze");
        }

        private static string ReadUserInputForFilePath(string fileExtension)
        {
            string filePath;
            do
            {
                // TODO: If FilePathIsNullOfEmpty should loop be broken? Should check fo null?
                filePath = Console.ReadLine();
            } while (!ValidateInput.IsUserInputValidFilePath(filePath, fileExtension));
            return filePath;
        }

        private static string[] GetFileContentFromPath(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}