using System.Linq;
using System.Collections.Generic;
using Exercise.Rules;
using System.Collections.Immutable;

namespace Exercise
{
    public class Analyzer
    {
        public static List<IIssue> Analyze(IFile file, ImmutableArray<IRule> rules, IConfiguration configuration)
        {
            var result = new List<IIssue>();

            foreach (var rule in rules)
            {
                var ruleConfig = configuration.Rules.FirstOrDefault(x => x.RuleId == rule.RuleId);

                if(ruleConfig != null)
                {
                    var resultFromRule = rule.Execute(file, ruleConfig);
                    result.AddRange(resultFromRule);
                }
            }

            return result;
        }
    }
}