using EnvDTE80;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Exercise;
using FluentAssertions;
using EnvDTE;

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

            _ = new AnalyzeCommand(menuService.Object, Mock.Of<ILogger>(), Mock.Of<IAnalysisController>(), Mock.Of<DTE2>());

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
            var dte = new Mock<DTE2>();

            var testSubject = new AnalyzeCommand(Mock.Of<IMenuCommandService>(), logger.Object, null, dte.Object);
            testSubject.Analyze();

            logger.Verify(p => p.LogWithNewLine("Cannot analyze as the analyzer component is unavailable."), Times.Once);
            dte.Verify(p => p.ActiveDocument, Times.Never);
        }

        [TestMethod]
        public void Analyze_ActiveDocumentIsNull_LogsCorrectMessageAndDoesNotContinue()
        {
            var logger = new Mock<ILogger>();
            var dte = new Mock<DTE2>();
            dte.Setup(p => p.ActiveDocument).Returns((Document)null);
            var analysisController = new Mock<IAnalysisController>();

            var testSubject = new AnalyzeCommand(Mock.Of<IMenuCommandService>(), logger.Object, analysisController.Object, dte.Object);
            testSubject.Analyze();

            logger.Verify(p => p.LogWithNewLine("No file is currently active. Please open a document and try again."), Times.Once);
            analysisController.Verify(p => p.AnalyzeAndGetResult(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Analyze_AllValuesOkay_CallsAnalyzeAndGetResult()
        {
            var dte = new Mock<DTE2>();
            var path = "TestPath";
            var document = new Mock<Document>();
            document.Setup(p => p.FullName).Returns(path);
            dte.Setup(p => p.ActiveDocument).Returns(document.Object);
            var analysisController = new Mock<IAnalysisController>();
            
            var testSubject = new AnalyzeCommand(Mock.Of<IMenuCommandService>(), Mock.Of<ILogger>(), analysisController.Object, dte.Object);
            testSubject.Analyze();
            
            analysisController.Verify(p => p.AnalyzeAndGetResult(path), Times.Once);
        }
    }
}
