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
}
