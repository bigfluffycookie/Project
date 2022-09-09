using System.Runtime.CompilerServices;

namespace Exercise
{
    public class Analyzer
    {
        public static void Analyze(string[] lines)
        {
            PrintTodos(lines);
        }

        private static void PrintTodos(string[] lines)
        {
            for(int i = 0; i<lines.Length;i++)
            {
                string line = lines[i];
                int indexOfTodo = line.IndexOf("TODO");
                if (indexOfTodo == -1)
                {
                    continue;
                }
                // TODO stringbuilder
                string print = "";
                print += "Line: " + i.ToString() + ", ";
                print += "Column: " + indexOfTodo.ToString() + ", ";
                print += "'" + line[indexOfTodo..] + "'"; 
                Console.WriteLine(print);
            }
        }
    }
}