using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace Exercise.UnitTests;

[TestClass]
public class InputValidatorTests
{
    [TestMethod]
    public void FileHasCorrectExtension_WrongExtension_ReturnsFalse()
    {
        var filePath = "Test.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var inputValidator = InitializeInputValidator(fileSystem);

        var hasCorrectExtension = inputValidator.FileHasCorrectExtension(filePath, ".exr");

        Assert.IsFalse(hasCorrectExtension);
    }

    [TestMethod]
    public void FileHasCorrectExtension_CorrectExtension_ReturnsTrue()
    {
        var filePath =  "Test.txt";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var inputValidator = InitializeInputValidator(fileSystem);

        var hasCorrectExtension = inputValidator.FileHasCorrectExtension(filePath, ".txt");

        Assert.IsTrue(hasCorrectExtension);
    }

    [TestMethod]
    public void FileExists_FileDoesNotExist_ReturnsFalse()
    {
        var path = @"c:\\myfile.txt";

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var inputValidator = InitializeInputValidator(fileSystem);

        var isValidTextFile = inputValidator.FileExists(path);

        Assert.IsFalse(isValidTextFile);
    }

    [TestMethod]
    public void FileExists_ExistingPath_ReturnsTrue()
    {
        var path = @"c:\\myfile.txt";

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        { 
                            { path, new MockFileData("Test") }
        });

        var inputValidator = InitializeInputValidator(fileSystem);

        var isValidTextFile = inputValidator.FileExists(path);

        Assert.IsTrue(isValidTextFile);
    }

    private InputValidator InitializeInputValidator(IFileSystem fileSystem) => new(fileSystem);
}