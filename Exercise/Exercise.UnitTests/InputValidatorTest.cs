namespace Exercise.UnitTests
{
    [TestClass]
    public class InputValidatorTest
    {
        [TestMethod]
        public void FileHasCorrectExtension_WithWrongExtension_ReturnsFalse()
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + @"\TestProject1\Test.txt";
            bool hasCorrectExtension = InputValidator.FileHasCorrectExtension(filePath, ".exr");
            Assert.IsFalse(hasCorrectExtension);
        }

        [TestMethod]
        public void FileExists_WithEmptyPath_ReturnsFalse()
        {
            string filePath = "";
            bool isValidTextFile = InputValidator.FileExists(filePath);
            Assert.IsFalse(isValidTextFile);
        }
    }
}