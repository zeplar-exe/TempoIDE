using System.IO;
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
            EnvironmentHelper.CreateSolution(new DirectoryInfo(Location.Value), SlnName.Value);
        }

        public override bool CanCreate()
        {
            return ValidateSolutionName(SlnName) && ValidateSolutionLocation(SlnName, Location);
        }
    }
}