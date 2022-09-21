namespace Exercise.Rules
{
    public class TodoRule : IRule
    {
        private const string paramId = "todoLines";

        public bool ShouldExecute(RuleParameterConfig ruleParameterConfig)
        {
            return ruleParameterConfig.HasRule(paramId);
        }

        public List<Issue> Execute(File file, RuleParameterConfig ruleParameterConfig)
        {
            var result = GetTodoLines(file.FileContent);
            return result;
        }

        private static List<Issue> GetTodoLines(string[] lines)
        {
            List<Issue> result = new List<Issue>();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var indexOfTodo = line.IndexOf("TODO");
                if (indexOfTodo == -1)
                {
                    continue;
                }
                var issue = new Issue(text: line[indexOfTodo..], line: i + 1, column: indexOfTodo + 1);
                result.Add(issue);
            }

            return result;
        }
    }
}
