namespace Exercise.Rules
{
    public interface IRule
    {
        string RuleId { get; }

        bool HasParameters { get; }

        List<Issue> Execute(IFile file, RuleParameterConfig ruleParameterConfig);
    }
}