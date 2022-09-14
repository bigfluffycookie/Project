namespace Exercise.Rules
{
    internal class MaxLineLengthRule : IRule
    {
        public List<Issue> Execute(File file)
        {
            var result = new List<Issue>();


            var param = GetInputParams();
            if (param < file.FileContent.Length)
            {
                var text = "Number of lines in file is " + file.FileContent.Length.ToString() +
                           " which is greater than max specified: " + param.ToString();
                var issue = new Issue(text: text, line: file.FileContent.Length, column: 1);
                result.Add(issue);
            }
            return result;
        }

        // TODO(leyla.buechel): Later can be replaced with json parsing.
        private static int GetInputParams()
        {
            Console.WriteLine("Input the maximum number of lines");
            var param = 0;
            string? input;
            do
            {
                input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine("Exiting max file line length analyzer");
                    break;
                }

            } while (!Int32.TryParse(input, out param));

            return param;
        }
    }
}
