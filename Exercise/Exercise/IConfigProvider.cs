namespace Exercise
{
    public interface IConfiguration
    {
        string FileToAnalyze { get; }

        IEnumerable<IRuleConfig> Rules { get; }
    }

    public interface IRuleConfig
    {
        string RuleId { get; }

        int RuleParam { get; }
    }

    public interface IConfigProvider
    {
        IConfiguration GetConfiguration();
    }

    public class Configuration : IConfiguration
    {
        public Configuration(string fileToAnalyze, IEnumerable<IRuleConfig> rules)
        {
            this.FileToAnalyze = fileToAnalyze;
            this.Rules = rules;
        }

        public string FileToAnalyze { get; }

        public IEnumerable<IRuleConfig> Rules { get; }
    }

    public class RuleConfig: IRuleConfig
    {
        public RuleConfig(string ruleId)
        {
            this.RuleId = ruleId;
        }

        public RuleConfig(string ruleId, int param)
        {
            this.RuleId = ruleId;
            this.RuleParam = param;
        }

        public string RuleId { get; }
        public int RuleParam { get; }
    }
}
