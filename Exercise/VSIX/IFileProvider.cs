using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Exercise;
using Microsoft.VisualStudio.Shell;

namespace VSIX
{
    internal interface IFileProvider
    {
        IFile GetFile();
    }

    [Export(typeof(IFileProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class FileProvider : IFileProvider
    {
        private readonly DTE2 dte;

        private readonly ILogger logger;

        [ImportingConstructor]
        public FileProvider([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider, ILogger logger)
        {
            dte = serviceProvider.GetService(typeof(DTE)) as DTE2;
            this.logger = logger;
        }

        public IFile GetFile()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var activeDoc = dte.ActiveDocument;

            if (activeDoc == null)
            {
                logger.LogWithNewLine("No file is currently active. Please open a document and try again.");
                return null;
            }

            var file = new File(activeDoc.FullName, activeDoc.GetText());

            return file;
        }
    }
}
