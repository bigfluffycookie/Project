namespace Exercise
{
    public class InputValidator
    {
        public static bool IsValidFilePath(string filePath, string fileExtension)
        {
            return FileExists(filePath) && FileHasCorrectExtension(filePath, fileExtension);
        }

        public static bool FileHasCorrectExtension(string filePath, string fileExtension)
        {
            if (Path.GetExtension(filePath) == fileExtension)
            {
                return true;
            }

            Console.WriteLine("File does not have the requested extension: " + fileExtension);

            return false;
        }

        public static bool FileExists(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                return true;
            }

            Console.WriteLine("File does not Exist. Please Try Again.");

            return false;
        }
    }
}