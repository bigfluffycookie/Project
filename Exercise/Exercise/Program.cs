using System.IO.Abstractions;
using Exercise.Rules;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();

            var filePath = ReadUserInputForFilePath(".txt");
            var fileContent = GetFileContent(filePath);
            var file = new File(filePath, fileContent);

            var rules = GetAvailableRules();
            var rulesToExecute = GetRulesToExecute(rules);
            var ruleParameterConfig = InitializeRuleParameterConfig(rulesToExecute);

            var result = Analyzer.Analyze(file, rulesToExecute, ruleParameterConfig);
            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
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

        private static RuleParameterConfig InitializeRuleParameterConfig(List<IRule> rules)
        {
            var ruleParameterConfig = new RuleParameterConfig();
            var rulesWithParams = rules.Where(rule => rule.HasParameters);

            foreach (var rule in rulesWithParams)
            {
                var ruleId = rule.RuleId;
                var input = GetInputParams("Input: " + ruleId);
                ruleParameterConfig.AddRuleParam(ruleId, input);
            }

            return ruleParameterConfig;
        }

        private static List<IRule> GetRulesToExecute(List<IRule> availableRules)
        {
            var rulesToExecute = new List<IRule>();
            Console.WriteLine("Enter ctrl c to exit input.");

            foreach (var rule in availableRules)
            {
                Console.WriteLine("Add rule: " + rule.RuleId + " to analyzer? y for yes, any other key for no");

                var input = Console.ReadLine();

                if (input == null)
                {
                    break;
                }

                if (input == "y")
                {

                    rulesToExecute.Add(rule);
                }
            }

            return rulesToExecute;
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
            } while (!(inputValidator.FileHasCorrectExtension(filePath, fileExtension) && inputValidator.FileExists(filePath)));

            return filePath;
        }

        private static int GetInputParams(string displayText)
        {
            Console.WriteLine(displayText);
            string? input;
            int number;

            do
            {
                input = Console.ReadLine();
            } while (!int.TryParse(input, out number));

            return number;
        }
    }
}