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
using Microsoft.VisualStudio.Shell.Interop;

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

        private readonly IAnalysisController analysisController;

        private readonly DTE2 dte;

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzeCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        internal AnalyzeCommand(IMenuCommandService commandService, ILogger logger,
                                IAnalysisController analysisController, DTE2 dte)
        {
            var menuCommandID = new CommandID(CommandSet, CommandId);
            
            var menuItem = new MenuCommand(this.Analyze, menuCommandID);
            commandService.AddCommand(menuItem);

            this.logger = logger;
            this.analysisController = analysisController;
            this.dte = dte;
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
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in AnalyzeCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            var dte = package.GetService<DTE, DTE2>();
            IAnalysisController analysisController = null;
            ILogger logger = null;

            try
            {
                var comp = package.GetService<SComponentModel, IComponentModel>();
                analysisController = comp.GetService<IAnalysisController>();
                logger = comp.GetService<ILogger>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            Instance = new AnalyzeCommand(commandService, logger, analysisController, dte);
        }

        internal void Analyze(object sender, EventArgs e)
        {
            if (analysisController == null)
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

            analysisController.AnalyzeAndGetResult(activeDoc.FullName);
        }
    }
}
