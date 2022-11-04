using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace Exercise
{
    [Export(typeof(IConfigProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class ConfigProviderJson : IConfigProvider
    {
        private IConfiguration configuration;

        private IFileSystem fileSystem;

        public ConfigProviderJson() : this (new FileSystem()) { }

        internal ConfigProviderJson(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            EnsureDefaultRulesConfigFileExists();
        }

        public void UpdateConfiguration(string path)
        {
            var userConfiguration = LoadUserConfiguration(path);
            configuration = InitializeConfig(userConfiguration, path);
        }

        private void EnsureDefaultRulesConfigFileExists()
        {
            var jsonFolder = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "LeylasAnalyzer");
            var defaultJsonPath = Path.Combine(jsonFolder, "rules.json");
            if (!fileSystem.File.Exists(defaultJsonPath))
            {
                fileSystem.Directory.CreateDirectory(jsonFolder);
                CreateDefaultJson(defaultJsonPath);
            }

            UpdateConfiguration(defaultJsonPath);
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

        private IConfiguration InitializeConfig(UserConfiguration userConfiguration, string path)
        {
            var ruleConfigs = new List<IRuleConfig>();

            foreach (var rule in userConfiguration.rules)
            {
                var ruleConfigJson = rule.Value.Length > 0
                                     ? new RuleConfig(rule.Key, rule.Value[0])
                                     : new RuleConfig(rule.Key, null);
                ruleConfigs.Add(ruleConfigJson);
            }

            return new Configuration(ruleConfigs, path);
        }

        public IConfiguration GetConfiguration()
        {
            return configuration;
        }
    }
}
