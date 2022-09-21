namespace Exercise.Rules
{
    public class MaxFilePathLengthRule : IRule
    {
        private const string paramId = "maxPathLength";

        public bool ShouldExecute(RuleParameterConfig ruleParameterConfig)
        {
            return ruleParameterConfig.HasRule(paramId);
        }

        public List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();
            var param = ruleParameterConfig.GetRuleParam(paramId);
            if (param < file.FilePath.Length)
            {
                var text = "File path length is too large: " + file.FilePath.Length.ToString() +
                           " which is greater than max specified: " + param.ToString();
                var issue = new Issue(text: text, line: 0, column: 0);

                result.Add(issue);
            }
            return result;
        }
    }
}