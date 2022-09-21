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

            var ruleParameterConfig = ReadUserInputForRules();
            var rules = GetRules();
            var result = Analyzer.Analyze(file, rules, ruleParameterConfig);
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

        private static RuleParameterConfig ReadUserInputForRules()
        {
            Console.WriteLine("Please input rules to run with parameters like rule=param" +
                              "if there are no parameters just input rule name.");
            Console.WriteLine("To finish inputing rules press 'x'");
            Console.WriteLine("Available rules are:");
            Console.WriteLine("todoLines");
            Console.WriteLine("maxPathLength");
            Console.WriteLine("maxLineLength");

            var ruleParameterConfig = new RuleParameterConfig();
            while (true)
            {
                var input = Console.ReadLine();
                if (input == null) { break; }
                var ruleAndParam = input.Split('=');
                if (ruleAndParam[0] == "x")
                {
                    Console.WriteLine("Finished adding rules, starting analyzer.");
                    break;
                }
                if (ruleAndParam.Length == 1)
                {
                    ruleParameterConfig.AddRuleParam(ruleAndParam[0], null);
                    continue;
                }
                if (!Int32.TryParse(ruleAndParam[1], out int nr))
                {
                    Console.WriteLine("Parameter for rule: " + ruleAndParam[0] + " needs to be an integer. " +
                                      " please try again.");
                    continue;
                }
                ruleParameterConfig.AddRuleParam(ruleAndParam[0], nr);
                Console.WriteLine("Succesfully added rule");
            }

            return ruleParameterConfig;
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