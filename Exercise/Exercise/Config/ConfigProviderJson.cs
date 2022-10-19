using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    internal class ConfigProviderJson : IConfigProvider
    {
        private ConfigurationJson configuration;

        public ConfigProviderJson(string filePath, List<IRule> availableRules)
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

        private void InitializeConfig(UserConfiguration userConfiguration, List<IRule> availableRules)
        {
            var ruleConfigs = new List<IRuleConfig>();
            var rulesToExecute = availableRules.Where(rule => userConfiguration.rules.ContainsKey(rule.RuleId)).ToList();

            foreach (var rule in rulesToExecute)
            {
                var ruleConfigJson = rule.HasParameters
                                     ? new RuleConfigJson(rule.RuleId, userConfiguration.rules[rule.RuleId][0])
                                     : new RuleConfigJson(rule.RuleId);
                ruleConfigs.Add(ruleConfigJson);
            }

            configuration = new ConfigurationJson(userConfiguration.fileToAnalyze, ruleConfigs);
        }

        public IConfiguration GetConfiguration()
        {
            return configuration;
        }
    }

    public class ConfigurationJson : IConfiguration
    {
        public ConfigurationJson(string fileToAnalyze, IEnumerable<IRuleConfig> rules)
        {
            this.FileToAnalyze = fileToAnalyze;
            this.Rules = rules;
        }

        public string FileToAnalyze { get; }

        public IEnumerable<IRuleConfig> Rules { get; }
    }

    public class RuleConfigJson : IRuleConfig
    {
        public RuleConfigJson(string ruleId)
        {
            this.RuleId = ruleId;
        }

        public RuleConfigJson(string ruleId, int param)
        {
            this.RuleId = ruleId;
            this.RuleParam = param;
        }

        public string RuleId { get; }
        public int RuleParam { get; }
    }
}