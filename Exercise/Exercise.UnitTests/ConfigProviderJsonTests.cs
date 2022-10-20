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
        var fileContent = @"{
             'fileToAnalyze': 'path',
             'rules' : {
                'ruleID' : [0]
             }
        }";

        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        var configProvider = new ConfigProviderJson(fileSystem, path);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(1, config.Rules.Count());
        Assert.AreEqual(0, config.Rules.First().RuleParam);
        Assert.AreEqual("ruleID", config.Rules.First().RuleId);
        Assert.AreEqual("path", config.FileToAnalyze);
    }

    [TestMethod]
    public void GetConfiguration_JsonWithNoRule_ReturnsConfigWithNoRules()
    {
        var path = "path.json";
        var fileContent = @"{
             'fileToAnalyze': 'path',
             'rules' : {}
        }";

        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        var configProvider = new ConfigProviderJson(fileSystem, path);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(0, config.Rules.Count());
    }

    [TestMethod]
    public void Constructor_WrongJson_ThrowsException()
    {
        var path = "path.json";
        var fileContent = "{}}";
        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        Assert.ThrowsException<Exception>(() => new ConfigProviderJson(fileSystem, path));
    }

    private static IFileSystem CreateFileSystemWithFile(string path, string fileContent)
    {
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllText(path)).Returns(fileContent);

        return fileSystem.Object;
    }
}