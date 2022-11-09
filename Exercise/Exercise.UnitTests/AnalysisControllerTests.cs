using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise.Rules;

namespace Exercise.UnitTests
{
    [TestClass]
    public class AnalysisControllerTests
    {
        [TestMethod]
        public void AnalyzeAndGetResult_WorksAsIntended()
        {
            var logger = new Mock<ILogger>();
            var availableRulesProvider = new Mock<IAvailableRulesProvider>();
            availableRulesProvider.Setup(p => p.AvailableRules).Returns(new List<IRule>().ToImmutableArray);
            var configPath = "ConfigPath";
            var config = new Mock<IConfiguration>();
            config.Setup(p => p.ConfigurationPath).Returns(configPath);
            var configProvider = new Mock<IConfigProvider>();
            configProvider.Setup(p => p.GetConfiguration()).Returns(config.Object);
            var file = SetupFile();

            var testSubject = new AnalysisController(logger.Object, availableRulesProvider.Object, configProvider.Object);
            testSubject.AnalyzeAndGetResult(file);

            configProvider.Verify(p => p.GetConfiguration(), Times.Once);
            config.Verify(p => p.ConfigurationPath, Times.Once);
            logger.Verify(p => p.LogWithNewLine($"Analyzing File:  {file.FilePath}"), Times.Once);
            logger.Verify(p => p.LogWithNewLine($"Using rule configuration from path: {configPath}"), Times.Once);
        }

        private static IFile SetupFile()
        {
            var lines = Array.Empty<string>();
            var file = new Mock<IFile>();
            file.Setup(p => p.FileContent).Returns(lines);

            return file.Object;
        }
    }
}
