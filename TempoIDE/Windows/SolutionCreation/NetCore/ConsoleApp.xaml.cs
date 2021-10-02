using System;
using System.IO;
using Jammo.TextAnalysis.DotNet.MsBuild.Projects;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;
using TempoIDE.Core.Environments;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Windows.SolutionCreation.NetCore
{
    public partial class ConsoleApp : SolutionCreationPanel
    {
        public ConsoleApp()
        {
            InitializeComponent();
        }

        public override void Create()
        {
            var env = SolutionEnvironment.CreateEmpty(Location.Text, SlnName.Text);
            using var projStream = new ProjectStream();
            
            projStream.Properties.Add("OutputType", "Exe");
            projStream.Properties.Add("TargetFramework", "net5.0");

            Directory.CreateDirectory(Path.Join(Location.Text, ProjectName.Text));
            var projectPath = Path.Join(ProjectName.Text, ProjectName.Text + ".csproj");
            
            projStream.WriteTo(Path.Join(Location.Text, projectPath));
            env.Stream.Projects.Add(new ProjectDefinition
            {
                ProjectGuid = Guid.NewGuid().ToString("B").ToUpper(),
                GlobalGuid = Guid.NewGuid().ToString("B").ToUpper(),
                Name = ProjectName.Text,
                RelativePath = Path.Join(ProjectName.Text, ProjectName.Text + ".csproj")
            }); // Courtesy of https://stackoverflow.com/a/29840377/16324801
            
            env.Stream.WriteTo(Path.Join(Location.Text, SlnName.Text + ".sln"));
            
            EnvironmentHelper.LoadEnvironment(env);
        }

        public override bool CanCreate()
        {
            if (!ValidateSolutionName(SlnName))
                return false;

            if (!ValidateSolutionLocation(SlnName, Location))
                return false;

            var projectDirectoryPath = Path.Join(Location.Text, SlnName.Text, ProjectName.Text);
            var projectPath = Path.Join(projectDirectoryPath, ProjectName.Text + ".csproj");
            
            return !File.Exists(projectPath);
        }
    }
}