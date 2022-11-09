using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;
using System.Diagnostics;
using Microsoft.VisualStudio.ComponentModelHost;
using Exception = System.Exception;
using Exercise;
using Microsoft.VisualStudio.Shell.Interop;
using System.Globalization;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Windows.Forms;
namespace VSIX
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class BrowseCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0101;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("9d169479-372d-4b21-a855-d4449ad5b742");

        private IConfigProvider configProvider;

        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        internal BrowseCommand(IMenuCommandService commandService, ILogger logger, IConfigProvider configProvider)
        {
            var menuCommandID = new CommandID(CommandSet, CommandId);

            var menuItem = new MenuCommand((object sender, EventArgs e) => { this.BrowseFile(); }, menuCommandID);
            commandService.AddCommand(menuItem);

            this.logger = logger;
            this.configProvider = configProvider;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static BrowseCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in AnalyzeCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            IConfigProvider configProvider = null;
            ILogger logger = null;

            try
            {
                var comp = package.GetService<SComponentModel, IComponentModel>();
                configProvider = comp.GetService<IConfigProvider>();
                logger = comp.GetService<ILogger>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            Instance = new BrowseCommand(commandService, logger, configProvider);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        internal void BrowseFile(IOpenFileDialog fileDialogWindow = null)
        {
            if (configProvider == null)
            {
                logger.LogWithNewLine("Can not update rule configuration as the Configuration Provider is unavailable.");
                return;
            }

            if (fileDialogWindow == null)
            {
                fileDialogWindow = new OpenFileDialog();
            }

            fileDialogWindow.Filter = "Json (.json)|*.json";

            if (fileDialogWindow.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            configProvider.UpdateConfiguration(fileDialogWindow.FileName);
        }
    }
}
