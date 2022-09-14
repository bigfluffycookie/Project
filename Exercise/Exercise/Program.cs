using System.Diagnostics.Contracts;
using Exercise.Rules;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            var filePath = ReadUserInputForFilePath(".txt");
            File file = new File(filePath);
            var rules = GetRules();
            var result = Analyzer.Analyze(file, rules);
            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static List<IRule> GetRules()
        {
            var rules = new List<IRule>();
            var maxLineLengthRule = new MaxLineLengthRule();
            rules.Add(maxLineLengthRule);
            var maxPathLengthRule = new MaxFilePathLengthRule();
            rules.Add(maxPathLengthRule);
            var todoRule = new TodoRule();
            rules.Add(todoRule);

            return rules;
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
        }

        private static string ReadUserInputForFilePath(string fileExtension)
        {
            Console.WriteLine("Please Input Text File path to Analyze");
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
    }
}