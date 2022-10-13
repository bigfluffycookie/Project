using System.IO.Abstractions;
using Moq;

namespace Exercise.UnitTests;

[TestClass]
public class InputValidatorTests
{
    [TestMethod]
    public void FileHasCorrectExtension_WrongExtension_ReturnsFalse()
    {
        const string filePath = "Test.txt";
        var fileSystem = Mock.Of<IFileSystem>();
        var inputValidator = InitializeInputValidator(fileSystem);

        var hasCorrectExtension = inputValidator.FileHasCorrectExtension(filePath, ".exr");

        Assert.IsFalse(hasCorrectExtension);
    }

    [TestMethod]
    public void FileHasCorrectExtension_CorrectExtension_ReturnsTrue()
    {
        const string filePath = "Test.txt";
        var fileSystem = Mock.Of<IFileSystem>();
        var inputValidator = InitializeInputValidator(fileSystem);

        var hasCorrectExtension = inputValidator.FileHasCorrectExtension(filePath, ".txt");

        Assert.IsTrue(hasCorrectExtension);
    }

    [TestMethod]
    public void FileExists_FileDoesNotExist_ReturnsFalse()
    {
        const string path = @"c:\\myfile.txt";
        var fileSystem = Mock.Of<IFileSystem>();
        var inputValidator = InitializeInputValidator(fileSystem);

        var isValidTextFile = inputValidator.FileExists(path);

        Assert.IsFalse(isValidTextFile);
    }

    [TestMethod]
    public void FileExists_ExistingPath_ReturnsTrue()
    {
        const string path = @"c:\\myfile.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.Exists(path)).Returns(true);
        var inputValidator = InitializeInputValidator(fileSystem.Object);

        var isValidTextFile = inputValidator.FileExists(path);

        Assert.IsTrue(isValidTextFile);
    }

    private InputValidator InitializeInputValidator(IFileSystem fileSystem) => new(fileSystem);
}