using System.IO;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            string filePath = ReadUserInput();
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

        private static string ReadUserInput()
        {
            // TODO(leyla.buechel): Check if input is correct,eg exists
            return Console.ReadLine();
        }

        private static string[] GetFileContentFromPath(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}