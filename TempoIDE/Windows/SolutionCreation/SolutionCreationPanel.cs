using System.IO;
using System.Windows.Controls;
using TempoIDE.UserControls.InputFields;

namespace TempoIDE.Windows.SolutionCreation
{
    public abstract class SolutionCreationPanel : UserControl
    {
        public abstract void Create();
        public abstract bool CanCreate();

        
        protected static bool ValidateSolutionName(StringField slnName)
        {
            return !string.IsNullOrWhiteSpace(slnName.Input.Text);
        }

        protected static bool ValidateSolutionLocation(string slnName, string location)
        {
            var fullPath = Path.Join(location, slnName + ".sln");
            
            return !File.Exists(fullPath);
        }
    }
}