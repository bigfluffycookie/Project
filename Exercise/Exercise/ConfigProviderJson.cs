using System.IO.Abstractions;
using System.Linq.Expressions;
using Exercise.Rules;
using Newtonsoft.Json;

namespace Exercise
{
    internal class ConfigProviderJson : IConfigProvider
    {
        private Configuration configuration;

        private IFileSystem fileSystem;

        public ConfigProviderJson(string filePath)
        {
            this.fileSystem = new FileSystem();
            var userConfiguration = CreateUserConfiguration(filePath);
            InitializeConfig(userConfiguration);
        }

        internal ConfigProviderJson(IFileSystem fileSystem, string filePath)
        {
            this.fileSystem = fileSystem;
            var userConfiguration = CreateUserConfiguration(filePath);
            InitializeConfig(userConfiguration);
        }

        private UserConfiguration CreateUserConfiguration(string filePath)
        {
            var configFileContent = fileSystem.File.ReadAllText(filePath);
            UserConfiguration userConfiguration;
            
            try
            { 
                userConfiguration = JsonConvert.DeserializeObject<UserConfiguration>(configFileContent);
            }
            catch
            {
                throw new Exception("Rule Configuration could not be Deserialized");
            }

            return userConfiguration;
        }

        private void InitializeConfig(UserConfiguration userConfiguration)
        {
            var ruleConfigs = new List<IRuleConfig>();

            foreach (var rule in userConfiguration.rules)
            {
                var ruleConfigJson = rule.Value.Length > 0
                                     ? new RuleConfig(rule.Key, rule.Value[0])
                                     : new RuleConfig(rule.Key);
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
