using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    internal class InputFromJson : IInput
    {
        private UserConfiguration userConfiguration;

        public InputFromJson(string filePath)
        {
            CreateUserConfiguration(filePath);
        }

        private void CreateUserConfiguration(string filePath)
        {
            var configFileContent = System.IO.File.ReadAllText(filePath);
            userConfiguration = JsonConvert.DeserializeObject<UserConfiguration>(configFileContent);

            if (userConfiguration == null)
            {
                throw new Exception("Rule Configuration could not be Serialized");
            }
        }

        public string GetPathForFileToAnalyze()
        {
            return userConfiguration.fileToAnalyze;
        }

        public List<IRule> GetRulesToExecute(List<IRule> availableRules)
        {
            return availableRules.Where(rule => userConfiguration.rules.ContainsKey(rule.RuleId)).ToList();
        }

        public IRuleParameterConfig GetRuleParmParameterConfig(List<IRule> rules)
        {
            var ruleParameterConfig = new RuleParameterConfig();

            var rulesWithParams = rules.Where(rule => rule.HasParameters);

            foreach (var rule in rulesWithParams)
            {
                ruleParameterConfig.AddRuleParam(rule.RuleId, userConfiguration.rules[rule.RuleId][0]);
            }

            return ruleParameterConfig;
        }
    }
}
