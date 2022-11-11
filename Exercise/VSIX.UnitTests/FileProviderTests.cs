using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Exercise;
using FluentAssertions;
using System.Windows.Controls;

namespace VSIX.UnitTests
{
    [TestClass]
    public class FileProviderTests
    {
        [TestInitialize]
        public void Initialize()
        {
            SetCurrentThreadAsUIThread();
        }

        [TestMethod]
        public void GetFile_ActiveDocumentIsNull_LogsCorrectMessageAndReturnsNull()
        {
            var dte = new Mock<DTE>();
            var logger = new Mock<ILogger>();

            var testSubject = new FileProvider(dte.Object, logger.Object);
            var file = testSubject.GetFile();

            file.Should().BeNull();
            dte.Verify(p => p.ActiveDocument, Times.Once);
            logger.Verify(p => p.LogWithNewLine("No file is currently active. Please open a document and try again."), Times.Once);
        }

        [TestMethod]
        public void GetFile_ActiveDocumentGetText_ReturnsCorrectFile()
        {
            var endPoint = new Mock<EditPoint>();
            string fileText = "Hello";
            var startPoint = new Mock<EditPoint>();
            startPoint.Setup(p => p.GetText(endPoint.Object)).Returns(fileText);

            var textDocument = new Mock<TextDocument>();
            textDocument.Setup(p => p.StartPoint.CreateEditPoint()).Returns(startPoint.Object);
            textDocument.Setup(p => p.EndPoint).Returns(endPoint.Object);

            var document = new Mock<Document>();
            document.Setup(p => p.Object(null)).Returns(textDocument.Object);
            var dte = new Mock<DTE>();
            dte.Setup(p => p.ActiveDocument).Returns(document.Object);
            string path = "path";
            dte.Setup(p => p.ActiveDocument.FullName).Returns(path);

            var testSubject = new FileProvider(dte.Object, Mock.Of<ILogger>());
            var file = testSubject.GetFile();

            file.FileContent[0].Should().Be(fileText);
            file.FilePath.Should().Be(path);
            dte.Verify(p => p.ActiveDocument, Times.Once);
        }

        public static void SetCurrentThreadAsUIThread()
        {
            var methodInfo = typeof(Microsoft.VisualStudio.Shell.ThreadHelper).GetMethod("SetUIThread", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            methodInfo.Should().NotBeNull("Could not find ThreadHelper.SetUIThread");
            methodInfo.Invoke(null, null);
        }
    }
}
