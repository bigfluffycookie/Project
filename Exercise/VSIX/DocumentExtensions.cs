using System;
using EnvDTE;

namespace VSIX
{
    internal static class DocumentExtensions
    {
        public static string[] GetText(this Document doc)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var textDocument = (TextDocument)doc.Object(null);
            var startPoint = textDocument.StartPoint.CreateEditPoint();
            return startPoint.GetText(textDocument.EndPoint).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
