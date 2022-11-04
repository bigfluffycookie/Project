using System.Collections.Generic;

namespace Exercise
{
    public interface IConfiguration
    {
        string ConfigurationPath { get; }

        IEnumerable<IRuleConfig> Rules { get; }
    }

    public interface IRuleConfig
    {
        string RuleId { get; }

        int? RuleParam { get; }
    }

    public interface IConfigProvider
    {
        void UpdateConfiguration(string path);

        IConfiguration GetConfiguration();
    }

    public class Configuration : IConfiguration
    {
        public string ConfigurationPath { get; }

        public IEnumerable<IRuleConfig> Rules { get; }

        public Configuration(IEnumerable<IRuleConfig> rules, string path)
        {
            this.Rules = rules;
            this.ConfigurationPath = path;
        }
    }

    public class RuleConfig: IRuleConfig
    {
        public RuleConfig(string ruleId, int? ruleParam)
        {
            this.RuleId = ruleId;
            this.RuleParam = ruleParam;
        }

        public string RuleId { get; }
        public int? RuleParam { get; }
    }
}
