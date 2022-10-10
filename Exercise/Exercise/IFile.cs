namespace Exercise
{
    public interface IFile
    {
        string FilePath { get; }
        string[] FileContent { get; }
    }
}