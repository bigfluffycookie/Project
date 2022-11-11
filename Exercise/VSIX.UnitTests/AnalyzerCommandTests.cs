using EnvDTE80;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Exercise;
using FluentAssertions;
using EnvDTE;
using System;

namespace VSIX.UnitTests
{
    [TestClass]
    public class AnalyzerCommandTests
    {
        [TestMethod]
        public void Ctor_CommandGetsRegistered()
        {
            var menuService = new Mock<IMenuCommandService>();
            var menuCommands = new List<MenuCommand>();
            menuService.Setup(p => p.AddCommand(Capture.In(menuCommands)));

            _ = new AnalyzeCommand(menuService.Object, Mock.Of<ILogger>(), Mock.Of<IAnalysisController>(), Mock.Of<IFileProvider>());

            menuCommands.Count.Should().Be(1);
            menuService.VerifyAll();

            var menuCommand = menuCommands[0];
            menuCommand.Should().NotBeNull();
            menuCommand.CommandID.ID.Should().Be(AnalyzeCommand.CommandId);
            menuCommand.CommandID.Guid.Should().Be(AnalyzeCommand.CommandSet);
        }

        [TestMethod]
        public void Analyze_AnalysisControllerIsNull_LogsCorrectMessageAndDoesNotContinue()
        {
            var logger = new Mock<ILogger>();
            var fileProvider = new Mock<IFileProvider>();

            var testSubject = new AnalyzeCommand(Mock.Of<IMenuCommandService>(), logger.Object, null, fileProvider.Object);
            testSubject.Analyze();

            logger.Verify(p => p.LogWithNewLine("Cannot analyze as the analyzer component is unavailable."), Times.Once);
            fileProvider.Verify(p => p.GetFile(), Times.Never);
        }

        [TestMethod]
        public void Analyze_FileIsNull_LogsCorrectMessageAndDoesNotContinue()
        {
            var logger = new Mock<ILogger>();
            var analysisController = new Mock<IAnalysisController>();
            var fileProvider = new Mock<IFileProvider>();
            fileProvider.Setup(p => p.GetFile()).Returns((IFile)null);

            var testSubject = new AnalyzeCommand(Mock.Of<IMenuCommandService>(), logger.Object, analysisController.Object, fileProvider.Object);
            testSubject.Analyze();

            fileProvider.Verify(p => p.GetFile(), Times.Once);
            logger.Verify(p => p.LogWithNewLine("Aborting Analyzing as file to analyze is not available"), Times.Once);
            analysisController.Verify(p => p.AnalyzeAndGetResult(It.IsAny<IFile>()), Times.Never);
        }

        [TestMethod]
        public void Analyze_AllValuesOkay_CallsAnalyzeAndGetResult()
        {
            var analysisController = new Mock<IAnalysisController>();
            var fileProvider = new Mock<IFileProvider>();
            var file = new File("", Array.Empty<string>());

            fileProvider.Setup(p => p.GetFile()).Returns(file);

            var testSubject = new AnalyzeCommand(Mock.Of<IMenuCommandService>(), Mock.Of<ILogger>(),
                                                 analysisController.Object, fileProvider.Object);
            testSubject.Analyze();

            analysisController.Verify(p => p.AnalyzeAndGetResult(file), Times.Once);
        }
    }
}
