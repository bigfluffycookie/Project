namespace Exercise.Rules
{
    public interface IRule
    {
        bool ShouldExecute(RuleParameterConfig ruleParameterConfig);
        List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig);
    }
}
