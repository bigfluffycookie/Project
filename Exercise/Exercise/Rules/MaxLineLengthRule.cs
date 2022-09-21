using System.Collections.Generic;
using System;
namespace Exercise.Rules
{
    public class MaxLineLengthRule : IRule
    {
        private const string paramId = "lineLength";

        public bool ShouldExecute(RuleParameterConfig ruleParameterConfig)
        {
            return ruleParameterConfig.HasRule(paramId);
        }

        public List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig)
        {
            var result = new List<Issue>();
            var param = ruleParameterConfig.GetRuleParam(paramId);
            if (!Int32.TryParse(param, out int nr))
            {
                Console.WriteLine("Parameter for rule: " + paramId + " is not of type int.");
                return result;
            }
            if (nr < file.FileContent.Length)
            {
                var text = "Number of lines in file is " + file.FileContent.Length.ToString() +
                           " which is greater than max specified: " + nr.ToString();
                var issue = new Issue(text: text, line: file.FileContent.Length, column: 1);
                result.Add(issue);
            }
            return result;
        }
    }
}
