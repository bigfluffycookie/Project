namespace Exercise.Rules
{
    public class MaxLineLengthRule : IRule
    {
        private const string paramId = "maxLineLength";

        public bool ShouldExecute(RuleParameterConfig ruleParameterConfig)
        {
            return ruleParameterConfig.HasRule(paramId);
        }

        public List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();
            if (!ruleParameterConfig.HasRule(paramId)) { return null; }
            var param = ruleParameterConfig.GetRuleParam(paramId);
            if (param < file.FileContent.Length)
            {
                var text = "Number of lines in file is " + file.FileContent.Length.ToString() +
                           " which is greater than max specified: " + param.ToString();
                var issue = new Issue(text: text, line: file.FileContent.Length, column: 1);
                result.Add(issue);
            }
            return result;
        }
    }
}
