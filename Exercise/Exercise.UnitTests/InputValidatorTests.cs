namespace Exercise.UnitTests
{
    [TestClass]
    public class InputValidatorTests
    {
        [TestMethod]
        public void FileHasCorrectExtension_WrongExtension_ReturnsFalse()
        {
            string filePath = "Test.txt";
            Console.WriteLine(filePath);
            bool hasCorrectExtension = InputValidator.FileHasCorrectExtension(filePath, ".exr");
            Assert.IsFalse(hasCorrectExtension);
        }

        [TestMethod]
        public void FileHasCorrectExtension_CorrectExtension_ReturnsTrue()
        {
            string filePath =  "Test.txt";
            bool hasCorrectExtension = InputValidator.FileHasCorrectExtension(filePath, ".txt");
            Assert.IsTrue(hasCorrectExtension);
        }

        [TestMethod]
        public void FileExists_EmptyPath_ReturnsFalse()
        {
            string filePath = "";
            bool isValidTextFile = InputValidator.FileExists(filePath);
            Assert.IsFalse(isValidTextFile);
        }

        [TestMethod]
        public void FileExists_ExistingPath_ReturnsTrue()
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + @"\Test.txt";
            System.IO.File.Create(filePath);
            bool isValidTextFile = InputValidator.FileExists(filePath);
            Assert.IsTrue(isValidTextFile);
        }
    }
}