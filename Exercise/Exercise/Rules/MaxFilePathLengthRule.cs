namespace Exercise.Rules
{
    public class MaxFilePathLengthRule : IRule
    {
        public string RuleId => "maxPathLength";

        public bool HasParameters => true;

        public List<Issue> Execute(IFile file, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();

            var maxAllowedFilePathLength = ruleParameterConfig.GetRuleParam(RuleId);

            if (file.FilePath.Length > maxAllowedFilePathLength)
            {
                var text = "File path length is too large: " + file.FilePath.Length +
                           " which is greater than max specified: " + maxAllowedFilePathLength;

                var issue = new Issue(text: text, line: 0, column: 0);

                result.Add(issue);
            }

            return result;
        }
    }
}