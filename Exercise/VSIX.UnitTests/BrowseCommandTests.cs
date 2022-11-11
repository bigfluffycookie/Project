using Moq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Exercise;
using FluentAssertions;
using System.Windows.Forms;

namespace VSIX.UnitTests
{
    [TestClass]
    public class BrowseCommandTests
    {
        [TestMethod]
        public void Ctor_CommandGetsRegistered()
        {
            var menuService = new Mock<IMenuCommandService>();
            var menuCommands = new List<MenuCommand>();
            menuService.Setup(p => p.AddCommand(Capture.In(menuCommands)));

            _ = new BrowseCommand(menuService.Object, Mock.Of<ILogger>(), Mock.Of<IConfigProvider>());

            menuCommands.Count.Should().Be(1);
            menuService.VerifyAll();

            var menuCommand = menuCommands[0];
            menuCommand.Should().NotBeNull();
            menuCommand.CommandID.ID.Should().Be(BrowseCommand.CommandId);
            menuCommand.CommandID.Guid.Should().Be(BrowseCommand.CommandSet);
        }

        [TestMethod]
        public void BrowseFile_ConfigProviderIsNull_LogsCorrectMessageAndDoesNotContinue()
        {
            var logger = new Mock<ILogger>();
            var openFileDialog = new Mock<IOpenFileDialog>();

            var testSubject = new BrowseCommand(Mock.Of<IMenuCommandService>(), logger.Object, null);
            testSubject.BrowseFile(openFileDialog.Object);

            logger.Verify(p => p.LogWithNewLine("Can not update rule configuration as the Configuration Provider is unavailable."), Times.Once);
            openFileDialog.Verify(p => p.Filter, Times.Never);
        }

        [TestMethod]
        public void BrowseFile_OpenFileDialogReturnsCanceled_DoesNotContinue()
        {
            var openFileDialog = new Mock<IOpenFileDialog>();
            openFileDialog.Setup(p => p.ShowDialog()).Returns(DialogResult.Cancel);
            openFileDialog.SetupProperty(p => p.Filter);
            var configProvider = new Mock<IConfigProvider>();

            var testSubject = new BrowseCommand(Mock.Of<IMenuCommandService>(), Mock.Of<ILogger>(), configProvider.Object);
            testSubject.BrowseFile(openFileDialog.Object);

            openFileDialog.Verify(p => p.ShowDialog(), Times.Once);
            openFileDialog.Object.Filter.Should().Be("Json (.json)|*.json");
            configProvider.Verify(p => p.UpdateConfiguration(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void BrowseFile_OpenFileDialogReturnsOk_CallsUpdateConfiguration()
        {
            var openFileDialog = new Mock<IOpenFileDialog>();
            openFileDialog.Setup(p => p.ShowDialog()).Returns(DialogResult.OK);
            var fileName = "Test.json";
            openFileDialog.Setup(p => p.FileName).Returns(fileName);
            openFileDialog.SetupProperty(p => p.Filter);

            var configProvider = new Mock<IConfigProvider>();

            var testSubject = new BrowseCommand(Mock.Of<IMenuCommandService>(), Mock.Of<ILogger>(), configProvider.Object);
            testSubject.BrowseFile(openFileDialog.Object);

            openFileDialog.Verify(p => p.ShowDialog(), Times.Once);
            openFileDialog.Object.Filter.Should().Be("Json (.json)|*.json");
            configProvider.Verify(p => p.UpdateConfiguration(fileName), Times.Once);
        }
    }
}
