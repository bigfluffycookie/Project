using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace Exercise
{
    internal class ConfigProviderJson : IConfigProvider
    {
        private Configuration configuration;

        private IFileSystem fileSystem;

        public ConfigProviderJson() : this (new FileSystem()) { }

        internal ConfigProviderJson(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            var jsonFolder = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "LeylasAnalyzer");
            var jsonFilePath = Path.Combine(jsonFolder, "rules.json");
            EnsureRulesConfigFileExists(jsonFolder, jsonFilePath);
            var userConfiguration = LoadUserConfiguration(jsonFilePath);

            InitializeConfig(userConfiguration);
        }

        private void EnsureRulesConfigFileExists(string jsonFolder, string jsonFilePath)
        {
            if (!fileSystem.File.Exists(jsonFilePath))
            {
                fileSystem.Directory.CreateDirectory(jsonFolder);
                CreateDefaultJson(jsonFilePath);
            }
        }

        private void CreateDefaultJson(string path)
        {
            var userConfiguration = new UserConfiguration();
            userConfiguration.rules = new Dictionary<string, int[]>();

            userConfiguration.rules.Add("todo", new int[0]);
            userConfiguration.rules.Add("maxPathLength", new int[] { 50 });
            userConfiguration.rules.Add("maxLineLength", new int[] { 20 });

            string configFileContent = JsonConvert.SerializeObject(userConfiguration);
            fileSystem.File.WriteAllText(path, configFileContent);
        }

        private UserConfiguration LoadUserConfiguration(string path)
        {
            var configFileContent = fileSystem.File.ReadAllText(path);
            UserConfiguration? userConfiguration;
            
            try
            { 
                userConfiguration = JsonConvert.DeserializeObject<UserConfiguration>(configFileContent);
            }
            catch
            {
                throw new Exception("Rule Configuration could not be Deserialized");
            }

            if (userConfiguration?.rules == null)
            {
                throw new Exception("Rule Configuration could not be Deserialized into UserConfiguration");
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
                                     : new RuleConfig(rule.Key, null);
                ruleConfigs.Add(ruleConfigJson);
            }

            configuration = new Configuration(ruleConfigs);
        }

        public IConfiguration GetConfiguration()
        {
            return configuration;
        }
    }
}
