using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise.Rules;
using Microsoft.VisualStudio.Setup.Configuration;

namespace Exercise.UnitTests
{
    [TestClass]
    public class AnalysisControllerTests
    {
        [TestMethod]
        public void AnalyzeAndGetResult_MethodsGetCalledCorrectly()
        {
            var logger = new Mock<ILogger>();
            var availableRulesProvider = new Mock<IAvailableRulesProvider>();
            availableRulesProvider.Setup(p => p.AvailableRules).Returns(new List<IRule>().ToImmutableArray);

            string filePath = "filePath";
            var file = SetupFile(filePath);

            var configPath = "ConfigPath";
            var configProvider = SetUpConfigProvider(configPath);

            var testSubject = new AnalysisController(logger.Object, availableRulesProvider.Object, configProvider.Object);
            testSubject.AnalyzeAndGetResult(file);

            configProvider.Verify(p => p.GetConfiguration(), Times.Once);
            logger.Verify(p => p.LogWithNewLine($"Analyzing File:  {file.FilePath}"), Times.Once);
            logger.Verify(p => p.LogWithNewLine($"Using rule configuration from path: {configPath}"), Times.Once);
            logger.Verify(p => p.LogWithNewLine("No Issues found"), Times.Once);
        }

        private static Mock<IConfigProvider> SetUpConfigProvider(string configPath)
        {
            var config = new Mock<IConfiguration>();
            config.Setup(p => p.ConfigurationPath).Returns(configPath);
            var configProvider = new Mock<IConfigProvider>();
            configProvider.Setup(p => p.GetConfiguration()).Returns(config.Object);

            return configProvider;
        }

        private static IFile SetupFile(string filePath)
        {
            var file = new Mock<IFile>();
            file.Setup(p => p.FilePath).Returns(filePath);

            return file.Object;
        }
    }
}
