using System.IO;
using System.IO.Abstractions;
using FluentAssertions;
using Moq;

namespace Exercise.UnitTests;

[TestClass]
public class FileTests
{
    [TestMethod]
    public void Ctor_FileDoesNotExist_HasDebugLog()
    {
        const string filePath = "Test.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllText(filePath)).Throws(new IOException());

        Assert.ThrowsException<IOException>(() => new File(filePath));
    }

    [TestMethod]
    public void Ctor_FileExistsÊmptyFile_FileContentEmpty()
    {
        const string filePath = "Test.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllLines(filePath)).Returns(System.Array.Empty<string>());

        var file = new File(filePath, fileSystem.Object);

        file.FileContent.Length.Should().Be(1);
    }

    [TestMethod]
    public void Ctor_FileExistsTwoLinesOfContent_LoadsCorrectContent()
    {
        const string filePath = "Test.txt";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllLines(filePath)).Returns(new[] { "Content1", "Content2" });

        var file = new File(filePath, fileSystem.Object);

        file.FileContent.Length.Should().Be(2);
        file.FileContent[0].Should().Be("Content1");
        file.FileContent[1].Should().Be("Content2");
    }
}