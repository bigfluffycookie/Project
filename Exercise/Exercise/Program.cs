using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            var input = GetInput();
            var filePath = input.GetPathForFileToAnalyze();
            var fileContent = GetFileContent(filePath);
            var file = new File(filePath, fileContent);

            var rules = GetAvailableRules();
            var rulesToExecute = input.RulesToExecute(rules);
            var ruleParameterConfig = input.InitializeRuleParmParameterConfig(rulesToExecute);

            var result = Analyzer.Analyze(file, rulesToExecute, ruleParameterConfig);

            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static IInput GetInput()
        {
            Console.WriteLine("If you have a predefined rule configuration Json please enter 'j', for manual input enter 'm'");
            var input = "";
            do
            {
                input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Exiting Analyzer");
                    Environment.Exit(0);
                }

                if (input == "j")
                {
                    Console.WriteLine("Enter Path to rule configuration json");
                    var inputFromJson = new InputFromJson(ReadUserInputForFilePath(".json"));

                    return inputFromJson;
                }
                else if (input == "m")
                {
                    var inputFromUser = new InputFromUser();
                    return inputFromUser;
                }

            } while (true);
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

        private static string ReadUserInputForFilePath(string fileExtension)
        {
            var inputValidator = new InputValidator();
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
            } while (!(inputValidator.FileHasCorrectExtension(filePath, fileExtension) && inputValidator.FileExists(filePath)));

            return filePath;
        }
    }
}