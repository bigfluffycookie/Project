using System.IO.Abstractions;
using Exercise.Rules;
using Moq;

namespace Exercise.UnitTests;

[TestClass]
public class ConfigProviderJsonTests
{
    [TestMethod]
    public void GetConfiguration_JsonWithOneRule_ReturnsConfigWithOneRule()
    {
        var path = "path.json";
        var fileContent = "{\r\n  \"fileToAnalyze\": \"path\",\r\n  \"rules\":{\r\n      \"ruleID\" : []\r\n   }\r\n}";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllText(path)).Returns(fileContent);

        var configProvider = new ConfigProviderJson(fileSystem.Object, path);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(1, config.Rules.Count());
        Assert.AreEqual("path", config.FileToAnalyze);
    }

    [TestMethod]
    public void Constructor_WrongJson_ThrowsException()
    {
        var path = "path.json";
        var fileContent = "{}}";
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllText(path)).Returns(fileContent);

        Assert.ThrowsException<Exception>(() => new ConfigProviderJson(fileSystem.Object, path));
    }
}