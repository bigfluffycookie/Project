namespace Exercise.Rules
{

    public interface IRule
    {
        bool HasParameters();
        string GetRuleId();
        List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig);
    }
}
