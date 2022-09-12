namespace Exercise
{
    public class Analyzer
    {
        public static List<Issue> Analyze(string[] lines)
        {
            List<Issue> result = new List<Issue>();
            List<Issue> resultToDo = GetTodoLines(lines);
            result.AddRange(resultToDo);
            return result;
        }

        private static List<Issue> GetTodoLines(string[] lines)
        {
            List<Issue> result = new List<Issue>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int indexOfTodo = line.IndexOf("TODO");
                if (indexOfTodo == -1)
                {
                    continue;
                }
                Issue issue = new Issue(line[indexOfTodo..], i + 1, indexOfTodo + 1);
                result.Add(issue);
            }

            return result;
        }
    }
}