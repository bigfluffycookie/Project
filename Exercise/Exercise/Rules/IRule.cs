namespace Exercise.Rules
{
    public interface IRule
    {
        List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig);
    }
}
