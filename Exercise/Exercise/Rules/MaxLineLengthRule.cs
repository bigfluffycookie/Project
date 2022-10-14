﻿namespace Exercise.Rules
{
    public class MaxLineLengthRule : IRule
    {
        public string RuleId => "maxLineLength";

        public bool HasParameters => true;

        public List<IIssue> Execute(IFile file, IRuleParameterConfig ruleParameterConfig)
        {
            var result = new List<IIssue>();
            var maxAllowedLineLength = ruleParameterConfig.GetRuleParam(RuleId);

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