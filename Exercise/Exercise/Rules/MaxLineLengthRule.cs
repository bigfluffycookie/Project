namespace Exercise.Rules
{
    public class MaxLineLengthRule : IRule
    {
        public string RuleId => "maxLineLength";

        public bool HasParameters => true;

        public List<IIssue> Execute(IFile file, IRuleConfig ruleConfig)
        {
            var result = new List<IIssue>();
            var maxAllowedLineLength = ruleConfig.RuleParam;

            if (file.FileContent.Length > maxAllowedLineLength)
            {
                var text = "Number of lines in file is " + file.FileContent.Length +
                           " which is greater than max specified: " + maxAllowedLineLength;

                var issue = new Issue(text: text, line: file.FileContent.Length, column: 1);
                result.Add(issue);
            }

            return result;
        }
    }
}