using TempoIDE.Core.Environments;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Windows.SolutionCreation;

public partial class EmptySolution : SolutionCreationPanel
{
    public EmptySolution()
    {
        InitializeComponent();
    }

    public override void Create()
    {
        EnvironmentHelper.LoadEnvironment(CSharpSolutionEnvironment.CreateEmpty(v_Location.Text, v_SlnName.Text));
    } // TODO: Window is unfocused

    public override bool CanCreate()
    {
        return ValidateSolutionName(v_SlnName) && ValidateSolutionLocation(v_SlnName, v_Location);
    }
}