using System.Transactions;
using Exercise.Rules;

namespace Exercise
{
    internal class InputFromUser : IInput
    {
        public string GetPathForFileToAnalyze() => ReadUserInputForFilePath(".txt");

        public List<IRule> GetRulesToExecute(List<IRule> availableRules)
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

        public IRuleParameterConfig GetRuleParmParameterConfig(List<IRule> rules)
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
            } while (!(inputValidator.FileHasCorrectExtension(filePath, fileExtension) &&
                       inputValidator.FileExists(filePath)));

            return filePath;
        }
    }
}