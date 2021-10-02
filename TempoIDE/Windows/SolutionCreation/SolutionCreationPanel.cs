using System.IO;
using System.Windows.Controls;
using TempoIDE.Controls.InputFields;

namespace TempoIDE.Windows.SolutionCreation
{
    public abstract class SolutionCreationPanel : UserControl
    {
        public abstract void Create();
        public abstract bool CanCreate();

        
        protected static bool ValidateSolutionName(StringField slnName)
        {
            return !string.IsNullOrWhiteSpace(slnName.Text);
        }

        protected static bool ValidateSolutionLocation(StringField slnName, StringField location)
        {
            var fullPath = Path.Join(location.Text, slnName.Text + ".sln");
            
            return !File.Exists(fullPath);
        }
    }
}