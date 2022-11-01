using System.IO;
using System.IO.Abstractions;
using Moq;

namespace Exercise.UnitTests;

[TestClass]
public class FileTests
{
    [TestMethod]
    public void Ctor_FileDoesNotExist_HasDebugLog()
    {
        const string filePath = "Test.txt";
        var file = new File(filePath);

        Assert.AreEqual(0, file.FileContent.Length);
    }

    [TestMethod]
    public void Ctor_FileExistsÊmptyFile_FileContentEmpty()
    {
        const string filePath = "Test.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllLines(filePath)).Returns(System.Array.Empty<string>());

        var file = new File(filePath, fileSystem.Object);

        Assert.AreEqual(0, file.FileContent.Length);
    }

    [TestMethod]
    public void Ctor_FileExistsOneLineOfContent_LoadsCorrectContent()
    {
        const string filePath = "Test.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllLines(filePath)).Returns(new[] { "Content" });

        var file = new File(filePath, fileSystem.Object);

        Assert.AreEqual(1, file.FileContent.Length);
        Assert.AreEqual("Content", file.FileContent[0]);
    }

    [TestMethod]
    public void Ctor_FileExistsTwoLinesOfContent_LoadsCorrectContent()
    {
        const string filePath = "Test.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllLines(filePath)).Returns(new[] { "Content1", "Content2" });

        var file = new File(filePath, fileSystem.Object);

        Assert.AreEqual(2, file.FileContent.Length);
        Assert.AreEqual("Content1", file.FileContent[0]);
        Assert.AreEqual("Content2", file.FileContent[1]);
    }
}