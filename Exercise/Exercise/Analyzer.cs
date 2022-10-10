using Exercise.Rules;

namespace Exercise
{
    public class Analyzer
    {
        public static List<IIssue> Analyze(IFile file, List<IRule> rules, IRuleParameterConfig ruleParameterConfig)
        {
            var result = new List<IIssue>();

            foreach (var rule in rules)
            {
                var resultFromRule = rule.Execute(file, ruleParameterConfig);
                result.AddRange(resultFromRule);
            }

            return result;
        }
    }
}