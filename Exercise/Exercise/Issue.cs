namespace Exercise
{
    public class Issue
    {
        public string Text { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }

        public Issue(string text, int line, int column)
        {
            Text = text;
            Line = line;
            Column = column;
        }
    }
}