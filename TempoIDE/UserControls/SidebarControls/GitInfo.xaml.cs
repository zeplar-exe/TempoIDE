using System.Windows;
using System.Windows.Controls;
using LibGit2Sharp;

namespace TempoIDE.UserControls.SidebarControls
{
    public partial class GitInfo : SidebarControl
    {
        public IQueryableCommitLog Commits
        {
            get => (IQueryableCommitLog)GetValue(CommitsProperty);
            set => SetValue(CommitsProperty, value);
        }

        public static readonly DependencyProperty CommitsProperty =
            DependencyProperty.Register(
                "Commits", typeof(IQueryableCommitLog),
                typeof(GitInfo)
            );

        public Commit SelectedCommit
        {
            get => (Commit)GetValue(SelectedCommitProperty);
            set => SetValue(SelectedCommitProperty, value);
        }

        public static readonly DependencyProperty SelectedCommitProperty =
            DependencyProperty.Register(
                "SelectedCommit", typeof(Commit),
                typeof(GitInfo)
            );
        
        private GitInfo()
        {
            InitializeComponent();
        }

        public static GitInfo LoadRepository(string path)
        {
            using var repository = new Repository(path);
            
            return new GitInfo { Commits = repository.Commits };
        }
    }
}