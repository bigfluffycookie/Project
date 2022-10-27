using System;
using System.Collections.Generic;
using Exercise.Rules;

namespace Exercise
{
    internal class ConfigProviderUser : IConfigProvider
    {
        private Configuration configuration;

        public ConfigProviderUser(IEnumerable<IRule> rules)
        {
            InitializeConfig(rules);
        }
        public IConfiguration GetConfiguration()
        {
            return configuration;
        }

        private void InitializeConfig(IEnumerable<IRule> availableRules)
        {
            var fileToAnalyze = ReadUserInputForFilePath(".txt");
            var ruleConfigs = CreateRuleConfigsWithUserInput(availableRules);

            configuration = new Configuration(fileToAnalyze, ruleConfigs);
        }


        private IEnumerable<IRuleConfig> CreateRuleConfigsWithUserInput(IEnumerable<IRule> availableRules)
        {
            var ruleConfigs = new List<IRuleConfig>();
            Console.WriteLine("Enter ctrl c to exit input.");

            foreach (var rule in availableRules)
            {
                Console.WriteLine("Add rule: " + rule.RuleId + " to analyzer? y for yes, any other key for no");

                var input = Console.ReadLine();

                if (input == null)
                {
                    break;
                }

                if (input != "y") continue;

                int? ruleParam = null;

                if (rule.HasParameters)
                {
                    ruleParam = GetInputParams("Input rule param");
                }

                var ruleConfig = new RuleConfig(rule.RuleId, ruleParam);
                ruleConfigs.Add(ruleConfig);
            }

            return ruleConfigs;
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
            Console.WriteLine("Please input path for file to be analyzed");
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