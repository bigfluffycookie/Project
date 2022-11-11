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
using System.IO.Abstractions;

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

        private readonly ILogger logger;

        private readonly IFileProvider fileProvider;

        internal AnalyzeCommand(IMenuCommandService commandService, ILogger logger,
                                IAnalysisController analysisController, IFileProvider fileProvider)
        {
            var menuCommandID = new CommandID(CommandSet, CommandId);

            var menuItem = new MenuCommand((object sender, EventArgs e) => { this.Analyze(); }, menuCommandID);
            commandService.AddCommand(menuItem);

            this.logger = logger;
            this.analysisController = analysisController;
            this.fileProvider = fileProvider;
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

            IAnalysisController analysisControllerComp = null;
            ILogger loggerComp = null;
            IFileProvider fileProviderComp = null;

            try
            {
                var comp = package.GetService<SComponentModel, IComponentModel>();
                analysisControllerComp = comp.GetService<IAnalysisController>();
                loggerComp = comp.GetService<ILogger>();
                fileProviderComp = comp.GetService<IFileProvider>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            Instance = new AnalyzeCommand(commandService, loggerComp, analysisControllerComp, fileProviderComp);
        }

        internal void Analyze()
        {
            if (analysisController == null || fileProvider == null)
            {
                logger.LogWithNewLine("Cannot analyze as one of the components was unable to load.");
                return;
            }

            var file = fileProvider.GetFile();

            if (file == null)
            {
                logger.LogWithNewLine("Aborting Analyzing as file to analyze is not available");
                return;
            }

            analysisController.AnalyzeAndGetResult(file);
        }
    }
}
