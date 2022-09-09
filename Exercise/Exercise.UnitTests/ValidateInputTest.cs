namespace Exercise.UnitTests
{
    [TestClass]
    public class ValidateInputTest
    {
        [TestMethod]
        public void InputValidator_WhenPathExtensionIsWrong()
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + @"\TestProject1\Test.txt";
            bool hasCorrectExtension = ValidateInput.FileHasCorrectExtension(filePath, ".exr");
            Assert.IsFalse(hasCorrectExtension);
        }

        [TestMethod]
        public void InputValidator_WhenPathDoesNotExist()
        {
            string filePath = "";
            bool isValidTextFile = ValidateInput.FileExists(filePath);
            Assert.IsFalse(isValidTextFile);
        }
    }
}