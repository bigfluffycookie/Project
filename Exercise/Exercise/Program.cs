namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            string filePath = ReadUserInputForFilePath(".txt");
            string[] fileContent = GetFileContentFromPath(filePath);
            
            List<Issue> result = Analyzer.Analyze(fileContent);
            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static void PrintResult(List<Issue> issues)
        {
            foreach (Issue issue in issues)
            {
                string print = "";
                print += "Line: " + issue.line + ", ";
                print += "Column: " + issue.column + ", ";
                print += "'" + issue.text + "'";
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
            string filePath = "";
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