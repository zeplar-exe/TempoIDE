using TempoIDE.Core.Environments;

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
            SolutionEnvironment.CreateEmpty(Location.Text, SlnName.Text);
        }

        public override bool CanCreate()
        {
            return ValidateSolutionName(SlnName) && ValidateSolutionLocation(SlnName.Text, Location.Text);
        }
    }
}