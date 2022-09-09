using System.Runtime.CompilerServices;

namespace Exercise
{
    public class Analyzer
    {
        private List<string> result;

        public Analyzer()
        {
            result = new List<string>();
        }

        // TODO: Could add another parameter "rule" to filter
        public void Analyze(string[] lines)
        {
            AddTODOLines(lines);
        }

        public List<string> GetResult()
        {
            return result;
        }

        private void AddTODOLines(string[] lines)
        {
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
        }
    }
}