namespace Exercise
{
    public class Issue
    {
        public string text { get; private set; }
        public int line { get; private set; }
        public int column { get; private set; }

        public Issue(string text, int line, int column)
        {
            this.text = text;
            this.line = line;
            this.column = column;
        }
    }
}