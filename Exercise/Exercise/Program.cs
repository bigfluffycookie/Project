namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            var filePath = ReadUserInputForFilePath(".txt");
            var fileContent = GetFileContentFromPath(filePath);

            var result = Analyzer.Analyze(fileContent);
            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static void PrintResult(List<Issue> issues)
        {
            foreach (Issue issue in issues)
            {
                var print = "";
                print += "Line: " + issue.Line + ", ";
                print += "Column: " + issue.Column + ", ";
                print += "'" + issue.Text + "'";
                Console.WriteLine(print);
            }
        }

        private static void DisplayWelcomeText()
        {
            Console.WriteLine("Welcome to the Analyzer");
            Console.WriteLine("Please Input Text File path to Analyze");
        }

        private static string ReadUserInputForFilePath(string fileExtension)
        {
            var filePath = "";
            do
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine("Exiting Analyzer");
                    break;
                }
                filePath = input;
            } while (!InputValidator.IsValidFilePath(filePath, fileExtension));
            return filePath;
        }

        private static string[] GetFileContentFromPath(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}