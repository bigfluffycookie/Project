using System.Collections.Generic;

namespace Exercise
{
    public interface IConfiguration
    {
        IEnumerable<IRuleConfig> Rules { get; }
    }

    public interface IRuleConfig
    {
        string RuleId { get; }

        int? RuleParam { get; }
    }

    public interface IConfigProvider
    {
        IConfiguration GetConfiguration();
    }

    public class Configuration : IConfiguration
    {
        public Configuration(IEnumerable<IRuleConfig> rules)
        {
            this.Rules = rules;
        }

        public IEnumerable<IRuleConfig> Rules { get; }
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
