using System;
using System.IO;
using Jammo.TextAnalysis.DotNet.MsBuild.Projects;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;
using TempoIDE.Core.Environments;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Windows.SolutionCreation.NetCore;

public partial class ConsoleApp : SolutionCreationPanel
{
    public ConsoleApp()
    {
        InitializeComponent();
    }

    public override void Create()
    {
        var env = CSharpSolutionEnvironment.CreateEmpty(v_Location.Text, v_SlnName.Text);
        using var projStream = new ProjectStream();
            
        projStream.Properties.Add("OutputType", "Exe");
        projStream.Properties.Add("TargetFramework", "net5.0");

        Directory.CreateDirectory(Path.Join(v_Location.Text, v_ProjectName.Text));
        var projectPath = Path.Join(v_ProjectName.Text, v_ProjectName.Text + ".csproj");
            
        projStream.WriteTo(Path.Join(v_Location.Text, projectPath));
        env.SolutionFile.Stream.Projects.Add(new ProjectDefinition
        {
            ProjectGuid = Guid.NewGuid().ToString("B").ToUpper(),
            GlobalGuid = Guid.NewGuid().ToString("B").ToUpper(),
            Name = v_ProjectName.Text,
            RelativePath = Path.Join(v_ProjectName.Text, v_ProjectName.Text + ".csproj")
        }); // Courtesy of https://stackoverflow.com/a/29840377/16324801
            
        env.SolutionFile.Stream.WriteTo(Path.Join(v_Location.Text, v_SlnName.Text + ".sln"));
            
        EnvironmentHelper.LoadEnvironment(env);
    }

    public override bool CanCreate()
    {
        if (!ValidateSolutionName(v_SlnName))
            return false;

        if (!ValidateSolutionLocation(v_SlnName, v_Location))
            return false;

        var projectDirectoryPath = Path.Join(v_Location.Text, v_SlnName.Text, v_ProjectName.Text);
        var projectPath = Path.Join(projectDirectoryPath, v_ProjectName.Text + ".csproj");
            
        return !File.Exists(projectPath);
    }
}