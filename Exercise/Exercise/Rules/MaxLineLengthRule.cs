namespace Exercise.Rules
{
    public class MaxLineLengthRule : IRule
    {
        private const string ruleId = "maxLineLength";

        public bool HasParameters()
        {
            return true;
        }

        public string GetRuleId()
        {
            return ruleId;
        }

        public List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();
            var maxAllowedLineLength = ruleParameterConfig.GetRuleParam(ruleId);

            if (file.FileContent.Length > maxAllowedLineLength)
            {
                var text = "Number of lines in file is " + file.FileContent.Length.ToString() +
                           " which is greater than max specified: " + maxAllowedLineLength.ToString();
                var issue = new Issue(text: text, line: file.FileContent.Length, column: 1);
                result.Add(issue);
            }

            return result;
        }
    }
}
