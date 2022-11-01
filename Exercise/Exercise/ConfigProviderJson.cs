﻿using System;
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

        private string jsonFolder = Environment.GetEnvironmentVariable("localappdata") + @"\\LeylasAnalyzer";

        public ConfigProviderJson() : this (new FileSystem()) { }

        internal ConfigProviderJson(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;

            UserConfiguration userConfiguration;
            string jsonFilePath = jsonFolder + "\\" + "rules.json";

            if (fileSystem.File.Exists(jsonFilePath))
            {
                userConfiguration = CreateUserConfiguration(jsonFilePath);
            }
            else
            {
                fileSystem.Directory.CreateDirectory(jsonFolder);
                userConfiguration = CreateDefaultJson(jsonFilePath);
            }

            InitializeConfig(userConfiguration);
        }

        private UserConfiguration CreateDefaultJson(string path)
        {
            var userConfiguration = new UserConfiguration();
            userConfiguration.rules = new Dictionary<string, int[]>();

            userConfiguration.rules.Add("todo", new int[0]);
            userConfiguration.rules.Add("maxPathLength", new int[] { 50 });
            userConfiguration.rules.Add("maxLineLength", new int[] { 20 });

            string configFileContent = JsonConvert.SerializeObject(userConfiguration);
            fileSystem.File.WriteAllText(path, configFileContent);

            return userConfiguration;
        }

        private UserConfiguration CreateUserConfiguration(string path)
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
