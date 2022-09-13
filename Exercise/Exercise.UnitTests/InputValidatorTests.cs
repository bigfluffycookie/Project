namespace Exercise.UnitTests
{
    [TestClass]
    public class InputValidatorTests
    {
        [TestMethod]
        public void FileHasCorrectExtension_WrongExtension_ReturnsFalse()
        {
            var filePath = "Test.txt";
            Console.WriteLine(filePath);
            var hasCorrectExtension = InputValidator.FileHasCorrectExtension(filePath, ".exr");
            Assert.IsFalse(hasCorrectExtension);
        }

        [TestMethod]
        public void FileHasCorrectExtension_CorrectExtension_ReturnsTrue()
        {
            var filePath =  "Test.txt";
            var hasCorrectExtension = InputValidator.FileHasCorrectExtension(filePath, ".txt");
            Assert.IsTrue(hasCorrectExtension);
        }

        [TestMethod]
        public void FileExists_EmptyPath_ReturnsFalse()
        {
            var filePath = "";
            var isValidTextFile = InputValidator.FileExists(filePath);
            Assert.IsFalse(isValidTextFile);
        }

        [TestMethod]
        public void FileExists_ExistingPath_ReturnsTrue()
        {
            var filePath = System.IO.Directory.GetCurrentDirectory() + @"\Test.txt";
            System.IO.File.Create(filePath);
            var isValidTextFile = InputValidator.FileExists(filePath);
            Assert.IsTrue(isValidTextFile);
        }
    }
}