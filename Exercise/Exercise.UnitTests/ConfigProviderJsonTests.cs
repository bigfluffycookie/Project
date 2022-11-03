using System;
using System.Linq;
using System.IO.Abstractions;
using Moq;
using System.IO;
using FluentAssertions;

namespace Exercise.UnitTests;

[TestClass]
public class ConfigProviderJsonTests
{
    [TestMethod]
    public void GetConfiguration_JsonWithOneRuleHasParameter_ReturnsConfigWithOneRule()
    {
        var fileContent = @"{
             'rules' : {
                'ruleID' : [0]
             }
        }";

        var fileSystem = CreateFileSystemWithFile(fileContent);

        var configProvider = new ConfigProviderJson(fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(1, config.Rules.Count());
        Assert.AreEqual(0, config.Rules.First().RuleParam);
        Assert.AreEqual("ruleID", config.Rules.First().RuleId);
    }

    [TestMethod]
    public void GetConfiguration_JsonWithTwoRules_ReturnsConfigWithTwoRules()
    {
        var fileContent = @"{
             'rules' : {
                'ruleID' : [0],
                'ruleID2' : [0]
             }
        }";

        var fileSystem = CreateFileSystemWithFile(fileContent);

        var configProvider = new ConfigProviderJson(fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(2, config.Rules.Count());
    }

    [TestMethod]
    public void GetConfiguration_JsonWithOneRuleNoParameter_ReturnsConfigWithOneRule()
    {
        var fileContent = @"{
             'rules' : {
                'ruleID' : []
             }
        }";

        var fileSystem = CreateFileSystemWithFile(fileContent);

        var configProvider = new ConfigProviderJson(fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(1, config.Rules.Count());
        Assert.AreEqual(null, config.Rules.First().RuleParam);
        Assert.AreEqual("ruleID", config.Rules.First().RuleId);
    }

    [TestMethod]
    public void GetConfiguration_JsonWithNoRule_ReturnsConfigWithNoRules()
    {
        var fileContent = @"{
             'rules' : {}
        }";

        var fileSystem = CreateFileSystemWithFile(fileContent);

        var configProvider = new ConfigProviderJson(fileSystem);
        var config = configProvider.GetConfiguration();

        Assert.AreEqual(0, config.Rules.Count());
    }

    [TestMethod]
    public void Constructor_InvalidJson_ThrowsException()
    {
        var fileContent = "{}}";
        var fileSystem = CreateFileSystemWithFile(fileContent);

        Assert.ThrowsException<Exception>(() => new ConfigProviderJson(fileSystem));
    }

    [TestMethod]
    public void Constructor_WrongFormatJson_ThrowsException()
    {
        var fileContent = "{}";
        var fileSystem = CreateFileSystemWithFile(fileContent);

        Assert.ThrowsException<Exception>(() => new ConfigProviderJson(fileSystem));
    }

    [TestMethod]
    public void Constructor_JsonDoesNotExist_CreatesJson()
    {
        var fileContent = @"{
             'rules' : {}
        }";

        var fileSystem = new Mock<IFileSystem>();

        var directoryPath = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "LeylasAnalyzer");
        var filePath = Path.Combine(directoryPath, "rules.json");

        fileSystem.Setup(p => p.File.Exists(filePath)).Returns(false);
        fileSystem.Setup(p => p.Directory.CreateDirectory(directoryPath));
        fileSystem.Setup(p => p.File.ReadAllText(filePath)).Returns(fileContent);

        _ = new ConfigProviderJson(fileSystem.Object);

        fileSystem.Verify(p => p.File.Exists(filePath), Times.Once);
        fileSystem.Verify(p => p.File.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
        fileSystem.Verify(p => p.Directory.CreateDirectory(directoryPath), Times.Once);
    }

    [TestMethod]
    public void Constructor_JsonExists_JsonIsNotCreated()
    {
        var fileContent = @"{
             'rules' : {}
        }";

        var fileSystem = new Mock<IFileSystem>();

        var directoryPath = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "LeylasAnalyzer");
        var filePath = Path.Combine(directoryPath, "rules.json");

        fileSystem.Setup(p => p.Directory.CreateDirectory(directoryPath));
        fileSystem.Setup(p => p.File.ReadAllText(filePath)).Returns(fileContent);

        _ = new ConfigProviderJson(fileSystem.Object);

        fileSystem.Verify(p => p.File.Exists(filePath), Times.Once);
        fileSystem.Verify(p => p.File.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
        fileSystem.Verify(p => p.Directory.CreateDirectory(directoryPath), Times.Once);
    }

    [TestMethod]
    public void Constructor_UpdateConfiguration_UpdatesCorrectly()
    {
        var fileContent = @"{
             'rules' : {}
        }";

        var updatedFileContent = @"{
             'rules' : 
                { 'ruleID' : [0] }
        }";

        var fileSystem = new Mock<IFileSystem>();
        var defaultPath = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "LeylasAnalyzer", "rules.json");
        fileSystem.Setup(p => p.File.Exists(defaultPath)).Returns(true);
        fileSystem.Setup(p => p.File.ReadAllText(defaultPath)).Returns(fileContent);

        var path = "Test.json";
        fileSystem.Setup(p => p.File.Exists(path)).Returns(true);
        fileSystem.Setup(p => p.File.ReadAllText(path)).Returns(updatedFileContent);

        var configProvider = new ConfigProviderJson(fileSystem.Object);
        configProvider.UpdateConfiguration(path);

        fileSystem.Verify(p => p.File.ReadAllText(path), Times.Once);
        var config = configProvider.GetConfiguration();

        config.Rules.Count().Should().Be(1);
        config.Rules.First().RuleId.Should().Be("ruleID");
        config.Rules.First().RuleParam.Should().Be(0);
    }

    private static IFileSystem CreateFileSystemWithFile(string fileContent)
    {
        var fileSystem = new Mock<IFileSystem>();
        var path = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "LeylasAnalyzer", "rules.json");
        fileSystem.Setup(p => p.File.Exists(path)).Returns(true);
        fileSystem.Setup(p => p.File.ReadAllText(path)).Returns(fileContent);

        return fileSystem.Object;
    }
}