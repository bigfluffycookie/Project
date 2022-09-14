namespace Exercise.Rules
{
    internal class MaxFilePathLengthRule : IRule
    {
        public List<Issue> Execute(File file)
        {
            var result = new List<Issue>();

            var param = GetInputParams();

            if (param < file.FilePath.Length)
            {
                var text = "File path length is " + file.FilePath.Length.ToString() +
                           " which is greater than max specified: " + param.ToString();
                var issue = new Issue(text: text, line: 0, column: 0);
                result.Add(issue);
            }
            return result;
        }

        // TODO(leyla.buechel): Later can be replaced with json parsing.
        private static int GetInputParams()
        {
            Console.WriteLine("Enter the max number of characters for the file path");
            var param = 0;
            string? input;
            do
            {
                input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine("Exiting max file path length analyzer");
                    break;
                }

            } while (!Int32.TryParse(input, out param));

            return param;
        }
    }
}