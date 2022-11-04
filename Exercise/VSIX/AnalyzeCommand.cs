using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;
using System.Diagnostics;
using Microsoft.VisualStudio.ComponentModelHost;
using Exception = System.Exception;
using Exercise;
using EnvDTE80;
using EnvDTE;

namespace VSIX
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AnalyzeCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("9d169479-372d-4b21-a855-d4449ad5b742");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private IAnalysisController analyzeController;

        private DTE2 dte;

        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzeCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private AnalyzeCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            
            var menuItem = new MenuCommand(this.Analyze, menuCommandID);
            commandService.AddCommand(menuItem);
           
            dte = package.GetService<DTE, DTE2>();

            try
            {
                var comp = this.package.GetService<SComponentModel, IComponentModel>();
                analyzeController = comp.GetService<IAnalysisController>();
                logger = comp.GetService<ILogger>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AnalyzeCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
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
            Instance = new AnalyzeCommand(package, commandService);
        }

        private void Analyze(object sender, EventArgs e)
        {
            if (analyzeController == null)
            {
                logger.LogWithNewLine("Cannot analyze as the analyzer component is unavailable.");
                return;
            }

            var activeDoc = dte.ActiveDocument;

            if (activeDoc == null)
            {
                logger.LogWithNewLine("No file is currently active. Please open a document and try again.");
                return;
            }

            analyzeController.AnalyzeAndGetResult(activeDoc.FullName);
        }
    }
}
