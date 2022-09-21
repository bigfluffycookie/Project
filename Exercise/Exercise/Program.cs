using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();
            var ruleConfigFile = ReadUserInputForFilePath();
            var ruleFileContent = System.IO.File.ReadAllText(ruleConfigFile);
            var ruleConfig = JsonConvert.DeserializeObject<SerializedRuleConfig>(ruleFileContent);
            if (ruleConfig == null)
            {
                throw new Exception("Rule Configeration could not be Serialized, Exiting Analyzer");
            }
            var filePath = ruleConfig.fileToAnalyze;
            if (System.IO.File.Exists(filePath))
            {
                throw new Exception("File to analyze does not exist: " + filePath);
            }
            File file = new File(filePath, System.IO.File.ReadAllLines(filePath));

            var ruleParameterConfig = InitializeRuleParameterConfig(ruleConfig);
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

        private static RuleParameterConfig InitializeRuleParameterConfig(SerializedRuleConfig ruleConfig)
        {
            var ruleParameterConfig = new RuleParameterConfig();
            foreach (var rule in ruleConfig.rules)
            {
                ruleParameterConfig.AddRuleParam(rule.id, rule.paramaters);
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

        private static string ReadUserInputForFilePath()
        {
            Console.WriteLine("Please Input the file path to the rule configuration");
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
            } while (!InputValidator.FileExists(filePath));
            return filePath;
        }
    }
}