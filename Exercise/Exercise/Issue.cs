namespace Exercise
{
    public class Issue
    {
        public string Text { get; }
        public int Line { get; }
        public int Column { get; }

        public Issue(string text, int line, int column)
        {
            Text = text;
            Line = line;
            Column = column;
        }
    }
}