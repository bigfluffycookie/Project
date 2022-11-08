using System.Windows.Forms;

namespace VSIX
{
    public interface IFileDialogWindow
    {
        string FileName { get; }

        string Filter { get; set; }

        DialogResult ShowDialog();
    }

    public class FileDialogWindow : IFileDialogWindow
    {
        private readonly OpenFileDialog openFileDialog;

        public string FileName { get { return openFileDialog.FileName; } }

        public string Filter
        {
            get { return openFileDialog.Filter; }
            set { openFileDialog.Filter = value; }
        }

        public FileDialogWindow()
        {
            openFileDialog = new OpenFileDialog();
        }

        public DialogResult ShowDialog()
        {
            return openFileDialog.ShowDialog();
        }
    }
}
