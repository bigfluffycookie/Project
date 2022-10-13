namespace Exercise
{
    public interface IIssue
    {
        string Text { get; }
        int Line { get; }
        int Column { get; }
    }
}
