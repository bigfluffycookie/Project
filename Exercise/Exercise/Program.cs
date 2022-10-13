using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DisplayWelcomeText();

            var userConfiguration = GetUserConfiguration();

            var filePath = userConfiguration.fileToAnalyze;
            var fileContent = GetFileContent(filePath);
            var file = new File(filePath, fileContent);

            var rules = GetAvailableRules();
            var rulesToExecute = GetRulesToExecute(rules, userConfiguration);
            var ruleParameterConfig = InitializeRuleParameterConfig(rulesToExecute, userConfiguration);

            var result = Analyzer.Analyze(file, rulesToExecute, ruleParameterConfig);

            PrintResult(result);
            Console.Write("Press any key to close App");
            Console.ReadKey();
        }

        private static UserConfiguration GetUserConfiguration()
        {
            var configFilePath = ReadUserInputForFilePath(".json");
            var configFileContent = System.IO.File.ReadAllText(configFilePath);
            var userConfiguration = JsonConvert.DeserializeObject<UserConfiguration>(configFileContent);

            if (userConfiguration == null)
            {
                throw new Exception("Rule Configuration could not be Serialized");
            }

            return userConfiguration;
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

        private static RuleParameterConfig InitializeRuleParameterConfig(List<IRule> rules, UserConfiguration userConfiguration)
        {
            var ruleParameterConfig = new RuleParameterConfig();

            var rulesWithParams = rules.Where(rule => rule.HasParameters);

            foreach (var rule in rulesWithParams)
            {
                ruleParameterConfig.AddRuleParam(rule.RuleId, userConfiguration.rules[rule.RuleId]);
            }

            return ruleParameterConfig;
        }

        private static List<IRule> GetRulesToExecute(List<IRule> availableRules, UserConfiguration userConfiguration)
        {
            return availableRules.Where(rule => userConfiguration.rules.ContainsKey(rule.RuleId)).ToList();
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
            Console.WriteLine("Please Input the file path to the rule configuration file");
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