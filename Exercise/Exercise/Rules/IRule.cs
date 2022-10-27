using System.Collections.Generic;

namespace Exercise.Rules
{
    public interface IRule
    {
        string RuleId { get; }

        bool HasParameters { get; }

        List<IIssue> Execute(IFile file, IRuleConfig ruleConfig);
    }
}