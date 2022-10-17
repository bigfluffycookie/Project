using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();

            var input = GetInput(args);

            var filePath = input.GetPathForFileToAnalyze();
            var fileContent = GetFileContent(filePath);
            var file = new File(filePath, fileContent);

            var rules = GetAvailableRules();
            var rulesToExecute = input.GetRulesToExecute(rules);
            var ruleParameterConfig = input.GetRuleParmParameterConfig(rulesToExecute);

            var result = Analyzer.Analyze(file, rulesToExecute, ruleParameterConfig);

            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static IInput GetInput(string[] args)
        {
            if (args.Length != 0)
            {
                var inputValidator = new InputValidator();

                if (!inputValidator.FileExists(args[0]))
                {
                    throw new Exception("Json File at path: " + args[0] + " was not found.");
                }

                var inputFromJson = new InputFromJson(args[0]);

                return inputFromJson;
            }

            return new InputFromUser();
        }

        private static string[] GetFileContent(string filePath)
        {
            var fileContent = Array.Empty<string>();

            try
            {
                fileContent = System.IO.File.ReadAllLines(filePath);
            }
            catch (IOException e)
            {
                Console.WriteLine("File could not be read with error message: " + e.Message);
                Console.Write("Press any key to close App");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return fileContent;
        }

        private static List<IRule> GetAvailableRules()
        {
            var rules = new List<IRule>()
            {
                                new MaxLineLengthRule(),
                                new MaxFilePathLengthRule(),
                                new TodoRule()
            };

            return rules;
        }

        private static void PrintResult(List<IIssue> issues)
        {
            foreach (var issue in issues)
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
    }
}