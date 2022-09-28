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

            var rules = GetRules();
            var ruleParameterConfig = InitializeRuleParameterConfig(rules);

            var result = Analyzer.Analyze(file, rules, ruleParameterConfig);
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
                Console.WriteLine("File could not be read with exception: " + e.Message);
                Console.Write("Press any key to close App");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return fileContent;
        }

        private static RuleParameterConfig InitializeRuleParameterConfig(List<IRule> rules)
        {
            var ruleParameterConfig = new RuleParameterConfig();
            var rulesWithParams = rules.Where(rule => rule.HasParameters());

            foreach (var rule in rulesWithParams)
            {
                var ruleId = rule.GetRuleId();
                var input = GetInputParams("Input: " + ruleId);
                ruleParameterConfig.AddRuleParam(ruleId, input);
            }

            return ruleParameterConfig;
        }

        private static List<IRule> GetRules()
        {
            var rules = new List<IRule>()
            {
                new MaxLineLengthRule(),
                new MaxFilePathLengthRule(),
                new TodoRule()
            };

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

        private static int GetInputParams(string displayText)
        {
            Console.WriteLine(displayText);
            var input = "";
            int nr;

            do
            {
                input = Console.ReadLine();
            } while (!Int32.TryParse(input, out nr));

            return nr;
        }
    }
}