using System;
using System.IO;
using System.Windows;
using LibGit2Sharp;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls.SidebarControl
{
    public partial class GitInfo : SidebarItem
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
        
            // TODO: Repository iteration causes AccessViolationException
            return new GitInfo();
        }
    }
}