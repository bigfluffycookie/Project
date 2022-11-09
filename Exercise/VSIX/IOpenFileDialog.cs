using System.Windows.Forms;

namespace VSIX
{
    /// <summary>
    /// Interface to wrap <see cref="System.Windows.Forms.OpenFileDialog" /> for testing
    /// </summary>
    public interface IOpenFileDialog
    {
        string FileName { get; }

        string Filter { get; set; }

        DialogResult ShowDialog();
    }

    public class OpenFileDialog : IOpenFileDialog
    {
        private readonly System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

        public string FileName => openFileDialog.FileName;

        public string Filter
        {
            get { return openFileDialog.Filter; }
            set { openFileDialog.Filter = value; }
        }

        public DialogResult ShowDialog() => openFileDialog.ShowDialog();
    }
}
