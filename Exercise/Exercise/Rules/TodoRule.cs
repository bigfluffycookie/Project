namespace Exercise.Rules
{
    public class TodoRule : IRule
    {
        public string RuleId => "todo";

        public bool HasParameters => false;

        public List<Issue> Execute(IFile file, RuleParameterConfig ruleParameterConfig)
        {
            var result = GetTodoLines(file.FileContent);

            return result;
        }

        private static List<Issue> GetTodoLines(IReadOnlyList<string> lines)
        {
            var result = new List<Issue>();

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var indexOfTodo = line.IndexOf("TODO", StringComparison.Ordinal);

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