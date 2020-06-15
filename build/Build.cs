using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Nuke.GitHub;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.GitHub.ChangeLogExtensions;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
[ShutdownDotNetBuildServerOnFinish]
[AzurePipelines(AzurePipelinesImage.WindowsLatest,
                TriggerBranchesInclude = new []{"master", "release/*"},
                AutoGenerate = false,
                InvokedTargets = new[] {nameof(Test), nameof(Pack)},
                NonEntryTargets = new []{nameof(Restore), nameof(UpdateBuildNumber)},
                PullRequestsAutoCancel = true)]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [CI] readonly AzurePipelines Pipelines;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    string ChangeLogFile => RootDirectory / "CHANGELOG.md";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                              .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                            .SetProjectFile(Solution)
                            .SetConfiguration(Configuration)
                            .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                           .SetProjectFile(Solution)
                           .SetConfiguration(Configuration)
                           .EnableCollectCoverage());
        });

    string PreReleaseTag => IsLocalBuild
        ? "dev"
        : GitVersion.PreReleaseTag;

    Target Pack => _ => _
        .DependsOn(Compile, Test)
        .Consumes(Compile)
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            var changeLog = GetCompleteChangeLog(ChangeLogFile).EscapeStringPropertyForMsBuild();

            DotNetPack(s => s
                           .SetProject(Solution.GetProject("GenericSearch"))
                           .SetOutputDirectory(ArtifactsDirectory)
                           .SetConfiguration(Configuration)
                           .SetAssemblyVersion(GitVersion.AssemblySemVer)
                           .SetFileVersion(GitVersion.AssemblySemFileVer)
                           .SetVersionSuffix(PreReleaseTag)
                           .SetInformationalVersion(GitVersion.InformationalVersion)
                           .SetPackageReleaseNotes(changeLog)
                       );
        });

    Target UpdateBuildNumber => _ => _
        .TriggeredBy(Pack)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            Pipelines.UpdateBuildNumber($"v{GitVersion.FullSemVer}");
        });
}