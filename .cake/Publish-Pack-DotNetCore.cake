#load "Configuration.cake"

Task("Publish:Pack:DotNetCore")
    .IsDependeeOf("Publish")
    .WithCriteria<Configuration>((ctx, config) => config.Solution.NuGetProjects.Any())
    .Does<Configuration>(config => 
{
    var projectArtifactDirectory = config.Artifacts.GetRootFor(ArtifactTypeOption.NuGet);
    var settings = new DotNetCorePackSettings
    {
        NoBuild = true,
        NoRestore = true,
        IncludeSymbols = true,
        Configuration = config.Solution.BuildConfiguration,
        OutputDirectory = projectArtifactDirectory
    };

    if(!string.IsNullOrWhiteSpace(config.Version.FileVersion))
    {
        Information($"Using File Version '{config.Version.FileVersion}'");
        settings.MSBuildSettings.SetFileVersion(config.Version.FileVersion);
    }

    if(!string.IsNullOrWhiteSpace(config.Version.PackageVersion))
    {
        Information($"Using Package Version '{config.Version.PackageVersion}'");
        settings.MSBuildSettings.WithProperty("PackageVersion", config.Version.PackageVersion);
    }

    Information($"Using Assembly Version '{config.Version.Version}'");

    settings.MSBuildSettings = new DotNetCoreMSBuildSettings();
    settings.MSBuildSettings
        .SetVersion(config.Version.Version)
        .SetConfiguration(config.Solution.BuildConfiguration);
    settings.MSBuildSettings.NoLogo = true;

    foreach(var nugetProject in config.Solution.NuGetProjects) {
        DotNetCorePack(nugetProject.ProjectFilePath.ToString(), settings);
    }

    foreach(var package in GetFiles($"{projectArtifactDirectory}/*.nupkg")) 
    {    
        config.Artifacts.Add(ArtifactTypeOption.NuGet, package.GetFilename().ToString(), package.FullPath);
    }
});
