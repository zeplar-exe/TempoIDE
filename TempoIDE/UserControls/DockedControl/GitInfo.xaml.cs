using System.Windows.Controls;
using LibGit2Sharp;

namespace TempoIDE.UserControls.DockedControl
{
    public partial class GitInfo : UserControl
    {
        private GitInfo()
        {
            InitializeComponent();
        }

        public static GitInfo LoadRepository(string path)
        {
            using var repository = new Repository(path);

            var control = new GitInfo();

            
            
            return control;
        }
    }
}