namespace Exercise.Rules
{
    public class MaxFilePathLengthRule : IRule
    {
        private const string paramId = "maxPathLength";

        public List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();

            var maxAllowedFilePathLength = ruleParameterConfig.GetRuleParam(paramId);

            if (file.FilePath.Length > maxAllowedFilePathLength)
            {
                var text = "File path length is too large: " + file.FilePath.Length.ToString() +
                           " which is greater than max specified: " + maxAllowedFilePathLength.ToString();
                var issue = new Issue(text: text, line: 0, column: 0);

                result.Add(issue);
            }

            return result;
        }
    }
}