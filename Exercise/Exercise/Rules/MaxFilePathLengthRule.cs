namespace Exercise.Rules
{
    public class MaxFilePathLengthRule : IRule
    {
        public string RuleId => "maxPathLength";

        public bool HasParameters => true;

        public List<IIssue> Execute(IFile file, IRuleParameterConfig ruleParameterConfig)
        {
            var result = new List<IIssue>();

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