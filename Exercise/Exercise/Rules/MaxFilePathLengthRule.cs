namespace Exercise.Rules
{
    public class MaxFilePathLengthRule : IRule
    {
        private const string paramId = "pathLength";

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
            if (nr < file.FilePath.Length)
            {
                var text = "File path length is too large: " + file.FilePath.Length.ToString() +
                           " which is greater than max specified: " + nr.ToString();
                var issue = new Issue(text: text, line: 0, column: 0);

                result.Add(issue);
            }
            return result;
        }
    }
}