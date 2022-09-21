using Exercise.Rules;

namespace Exercise
{
    public class Analyzer
    {
        public static List<Issue> Analyze(File file, List<IRule> rules, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();
            foreach (var rule in rules)
            {
                var resultFromRule = rule.Execute(file, ruleParameterConfig);
                result.AddRange(resultFromRule);
            }
            return result;
        }
    }
}