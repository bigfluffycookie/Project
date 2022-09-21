using Exercise.Rules;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            var filePath = ReadUserInputForFilePath(".txt");
            File file = new File(filePath, System.IO.File.ReadAllLines(filePath));

            var ruleParameterConfig = InitializeRuleParameterConfig();
            var rules = GetRules();
            var result = Analyzer.Analyze(file, rules, ruleParameterConfig);
            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static RuleParameterConfig InitializeRuleParameterConfig()
        {
            var ruleParameterConfig = new RuleParameterConfig();
            var maxLines = GetInputParams("Input the maximum number of lines");
            ruleParameterConfig.AddRuleParam("maxLineLength", maxLines);
            var maxPathLength = GetInputParams("Enter the max number of characters for the file path");
            ruleParameterConfig.AddRuleParam("maxPathLength", maxPathLength);
            return ruleParameterConfig;
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