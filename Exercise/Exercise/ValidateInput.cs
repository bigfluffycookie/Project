namespace Exercise
{
    internal class ValidateInput
    {
        public static bool IsUserInputValidFilePath(string filePath, string fileExtension)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File does not Exist. Please Try Again.");
            }
            else if (System.IO.Path.GetExtension(filePath) != fileExtension)
            {
                Console.WriteLine("File does not have the requested extension: " + fileExtension);
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}
