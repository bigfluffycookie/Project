using Exercise.Rules;

namespace Exercise
{
    public class Analyzer
    {
        public static List<Issue> Analyze(File file, List<IRule> rules)
        {
            var result = new List<Issue>();
            foreach (var rule in rules)
            {
                var resultFromRule = rule.Execute(file);
                result.AddRange(resultFromRule);
            }
            return result;
        }
    }
}