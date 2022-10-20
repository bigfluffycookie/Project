using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    internal class ConfigProviderJson : IConfigProvider
    {
        private Configuration configuration;

        public ConfigProviderJson(string filePath, IEnumerable<IRule> availableRules)
        {
            var userConfiguration = CreateUserConfiguration(filePath);
            InitializeConfig(userConfiguration, availableRules);
        }

        private UserConfiguration CreateUserConfiguration(string filePath)
        {
            var configFileContent = System.IO.File.ReadAllText(filePath);
            var userConfiguration = JsonConvert.DeserializeObject<UserConfiguration>(configFileContent);

            if (userConfiguration == null)
            {
                throw new Exception("Rule Configuration could not be Deserialized");
            }

            return userConfiguration;
        }

        private void InitializeConfig(UserConfiguration userConfiguration, IEnumerable<IRule> availableRules)
        {
            var ruleConfigs = new List<IRuleConfig>();
            var rulesToExecute = availableRules.Where(rule => userConfiguration.rules.ContainsKey(rule.RuleId)).ToList();

            foreach (var rule in rulesToExecute)
            {
                var ruleConfigJson = rule.HasParameters
                                     ? new RuleConfig(rule.RuleId,userConfiguration.rules[rule.RuleId][0])
                                     : new RuleConfig(rule.RuleId);
                ruleConfigs.Add(ruleConfigJson);
            }

            configuration = new Configuration(userConfiguration.fileToAnalyze,ruleConfigs);
        }

        public IConfiguration GetConfiguration()
        {
            return configuration;
        }
    }
}
