using System.Runtime.CompilerServices;

namespace Exercise
{
    public class Analyzer
    {
        // TODO: Could add another parameter "rule" to filter
        public static List<string> Analyze(string[] lines)
        {
            List<string> result = new List<string>();
            List<string> resultToDo = GetTodoLines(lines);
            result.AddRange(resultToDo);
            return result;
        }

        private static List<string> GetTodoLines(string[] lines)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int indexOfTodo = line.IndexOf("TODO");
                if (indexOfTodo == -1)
                {
                    continue;
                }
                // TODO stringbuilder
                string todoLine = "";
                todoLine += "Line: " + i.ToString() + ", ";
                todoLine += "Column: " + indexOfTodo.ToString() + ", ";
                todoLine += "'" + line[indexOfTodo..] + "'";
                result.Add(todoLine);
            }

            return result;
        }
    }
}