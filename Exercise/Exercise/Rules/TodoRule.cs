namespace Exercise.Rules
{
    public class TodoRule : IRule
    {
        public string RuleId => "todo";

        public bool HasParameters => false;

        public List<IIssue> Execute(IFile file, IRuleConfig ruleConfig)
        {
            var result = GetTodoLines(file.FileContent);

            return result;
        }

        private static List<IIssue> GetTodoLines(IReadOnlyList<string> lines)
        {
            var result = new List<IIssue>();

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