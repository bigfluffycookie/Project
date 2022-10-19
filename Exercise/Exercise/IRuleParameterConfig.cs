namespace Exercise
{
    public interface IRuleParameterConfig
    {
        int GetRuleParam(string key);
    }

    // HACK - to avoid breaking all of the rules and tests, we could add
    // an adapter class that translates between the new IConfiguration class
    // and the old IRuleParameterConfig class.
    //
    // That way we don't have to refactor everything at once; we can do it in
    // a number of smaller PRs, rather than in one big bang.
    public class ConfigToRuleParameterConfigAdapter : IRuleParameterConfig
    {
        private readonly IConfiguration configuration;

        public ConfigToRuleParameterConfigAdapter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int GetRuleParam(string key)
        {
            return configuration.Rules.FirstOrDefault(x => x.RuleId == key)?.RuleParam ?? 0;
        }
    }
}
