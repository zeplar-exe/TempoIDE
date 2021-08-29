using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Core.Static;

namespace TempoIDE.Windows.SolutionCreation
{
    public partial class EmptySolution : SolutionCreationPanel
    {
        public EmptySolution()
        {
            InitializeComponent();
        }

        public override void Create()
        {
            EnvironmentHelper.CreateSolution(new DirectoryInfo(Location.Input.Text), SlnName.Input.Text);
        }

        public override bool CanCreate()
        {
            return ValidateSolutionName(SlnName) && ValidateSolutionLocation(SlnName, Location);
        }

        private void SlnName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CreateSolutionWindow.CreateRoutedCommand.RaiseCanExecuteChanged();
        }

        private void Location_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CreateSolutionWindow.CreateRoutedCommand.RaiseCanExecuteChanged();
        }
    }
}