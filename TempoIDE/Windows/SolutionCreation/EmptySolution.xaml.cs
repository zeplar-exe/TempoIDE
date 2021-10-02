using TempoIDE.Core.Environments;
using TempoIDE.Core.Helpers;

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
            EnvironmentHelper.LoadEnvironment(SolutionEnvironment.CreateEmpty(Location.Text, SlnName.Text));
        } // TODO: Window is unfocused

        public override bool CanCreate()
        {
            return ValidateSolutionName(SlnName) && ValidateSolutionLocation(SlnName, Location);
        }
    }
}