namespace Exercise
{
    public class Analyzer
    {
        public static List<Issue> Analyze(string[] lines)
        {
            var result = new List<Issue>();
            var resultToDo = GetTodoLines(lines);
            result.AddRange(resultToDo);
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
                var issue = new Issue(text:line[indexOfTodo..],line:i + 1, column:indexOfTodo + 1) ;
                result.Add(issue);
            }

            return result;
        }
    }
}