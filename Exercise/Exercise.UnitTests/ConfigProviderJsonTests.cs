using System;
using System.Linq;
using System.IO.Abstractions;
using Moq;

namespace Exercise.UnitTests;

[TestClass]
public class ConfigProviderJsonTests
{
    [TestMethod]
    public void GetConfiguration_JsonWithOneRuleHasParameter_ReturnsConfigWithOneRule()
    {
        var path = "path.json";
        var fileContent = @"{
             'rules' : {
                'ruleID' : [0]
             }
        }";

        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        var configProvider = new ConfigProviderJson(path, fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(1, config.Rules.Count());
        Assert.AreEqual(0, config.Rules.First().RuleParam);
        Assert.AreEqual("ruleID", config.Rules.First().RuleId);
    }

    [TestMethod]
    public void GetConfiguration_JsonWithTwoRules_ReturnsConfigWithTwoRules()
    {
        var path = "path.json";
        var fileContent = @"{
             'rules' : {
                'ruleID' : [0],
                'ruleID2' : [0]
             }
        }";

        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        var configProvider = new ConfigProviderJson(path, fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(2, config.Rules.Count());
    }

    [TestMethod]
    public void GetConfiguration_JsonWithOneRuleNoParameter_ReturnsConfigWithOneRule()
    {
        var path = "path.json";
        var fileContent = @"{
             'rules' : {
                'ruleID' : []
             }
        }";

        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        var configProvider = new ConfigProviderJson(path, fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(1, config.Rules.Count());
        Assert.AreEqual(null, config.Rules.First().RuleParam);
        Assert.AreEqual("ruleID", config.Rules.First().RuleId);
    }

    [TestMethod]
    public void GetConfiguration_JsonWithNoRule_ReturnsConfigWithNoRules()
    {
        var path = "path.json";
        var fileContent = @"{
             'rules' : {}
        }";

        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        var configProvider = new ConfigProviderJson(path, fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(0, config.Rules.Count());
    }

    [TestMethod]
    public void Constructor_InvalidJson_ThrowsException()
    {
        var path = "path.json";
        var fileContent = "{}}";
        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        Assert.ThrowsException<Exception>(() => new ConfigProviderJson(path, fileSystem));
    }

    [TestMethod]
    public void Constructor_WrongFormatJson_ThrowsException()
    {
        var path = "path.json";
        var fileContent = "{}";
        var fileSystem = CreateFileSystemWithFile(path, fileContent);

        Assert.ThrowsException<Exception>(() => new ConfigProviderJson(path, fileSystem));
    }

    private static IFileSystem CreateFileSystemWithFile(string path, string fileContent)
    {
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.Setup(p => p.File.ReadAllText(path)).Returns(fileContent);

        return fileSystem.Object;
    }
}