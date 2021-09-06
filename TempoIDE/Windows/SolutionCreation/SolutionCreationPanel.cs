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

        protected static bool ValidateSolutionLocation(StringField slnName, StringField location)
        {
            if (!Directory.Exists(location.Input.Text))
                return false;

            var fullPath = Path.Join(location.Input.Text, slnName.Input.Text);
            
            if (File.Exists(fullPath))
                return false;

            return true;
        }
    }
}