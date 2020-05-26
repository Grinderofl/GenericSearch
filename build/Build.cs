using System;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
[ShutdownDotNetBuildServerOnFinish]
[AzurePipelines(AzurePipelinesImage.WindowsLatest,
                TriggerBranchesInclude = new []{"master", "release/*"},
                InvokedTargets = new[] {nameof(Test), nameof(Pack)},
                NonEntryTargets = new []{nameof(Restore), nameof(VersionInfo), nameof(UpdateBuildNumber)},
                PullRequestsAutoCancel = true)]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [CI] readonly AzurePipelines Pipelines;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Version Version;

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
                           .SetConfiguration(Configuration));
        });

    Target Pack => _ => _
        .DependsOn(VersionInfo, Compile, Test)
        .Consumes(Compile)
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                           .SetProject(Solution.GetProject("GenericSearch"))
                           .SetOutputDirectory(ArtifactsDirectory)
                           .SetConfiguration(Configuration)
                           .SetAssemblyVersion(AssemblyVersion)
                           .SetFileVersion(FileVersion)
                           .SetVersionSuffix(Suffix)
                           .SetInformationalVersion(InformationalVersion)
                       );
        });

    Target VersionInfo => _ => _
        .Executes(() =>
        {
            Version = CreateVersion();
            Metadata = CreateMetadata();
            Suffix = CreateSuffix();
        });

    Target UpdateBuildNumber => _ => _
        .TriggeredBy(Pack)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            Pipelines.UpdateBuildNumber($"v{AssemblyVersion}{PrereleaseTag}.{Version.Revision}");
        });
}