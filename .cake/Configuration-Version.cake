#addin "Cake.FileHelpers"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0&prerelease"

using System.Text.RegularExpressions;

public class BuildVersion {
    public FilePath VersionAssemblyInfo { get; private set; }
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string CakeVersion { get; private set; } = typeof(ICakeContext).Assembly.GetName().Version.ToString();
    public string InformationalVersion { get; private set; }
    public string FullSemVersion { get; private set; }

    public string FileVersion { get; private set; }
    public string PackageVersion { get; private set; }

    private static readonly Regex regex = 
        new Regex(@"^refs/heads/release/(?<major>\d)+\.(?<minor>\d)+\.?(?<patch>\d)*(?<preview>-preview)?(?<number>\d)*$");

    private const string EmptyVersion = "0.0.0.0";
    private const string AssemblyFileVersion = "AssemblyFileVersion(\"{0}\")";
    private const string AssemblyVersion = "AssemblyVersion(\"{0}\")";
    private const string AssemblyInformationalVersion = "AssemblyInformationalVersion(\"{0}\")";
        
    private void UseBranchVersion(ICakeContext context) {
        var branchName = context.EnvironmentVariable("SourceBranch") ?? "";
        context.Information($"Source branch: {branchName}");

        var match = regex.Match(branchName);
        if(match.Success) {
            context.Information($"Calculating semantic version...");

            var buildId = context.EnvironmentVariable("BuildId");
            context.Information($"Build id: {buildId}");
            
            var major = match.Groups["major"].Value;
            var minor = match.Groups["minor"].Value;
            var patch = match.Groups["patch"].Success ? match.Groups["patch"].Value : "0";
            var preview = match.Groups["preview"].Success;
            var number = match.Groups["number"].Value;
            var revision = buildId.Split('.')[1];
            var hash = context.EnvironmentVariable("Hash");

            var preReleaseTag = preview ? $"-preview.{number}" : "";

            var fileVersion = $"{major}.{minor}.{patch}.{revision}";
            var assemblyVersion = $"{major}.{minor}.{patch}";
            var informationalVersion = $"{major}.{minor}.{patch}{preReleaseTag}.{revision}+{hash}";
            var packageVersion = $"{major}.{minor}.{patch}{preReleaseTag}";

            var assemblyInfos = new Dictionary<string, string>()
            {
                [AssemblyFileVersion] = fileVersion,
                [AssemblyVersion] = assemblyVersion,
                [AssemblyInformationalVersion] = informationalVersion
            };

            var assemblyInfo = "AssemblyInfo.cs";
            foreach(var info in assemblyInfos)
            {
                var source = string.Format(info.Key, EmptyVersion);
                var dest = string.Format(info.Key, info.Value);
                context.Information($"Replacing [{source}] with [{dest}]");
                context.ReplaceTextInFiles(assemblyInfo, source, dest);
            }
            Version = assemblyVersion;
            FileVersion = fileVersion;
            PackageVersion = packageVersion;

            FullSemVersion = $"{major}.{minor}.{patch}{preReleaseTag}.{revision}";
        }
        else if(context.DirectoryExists(".git")) {
                UseGitVersion(context);
        }
        else {
            UseVersionAssemblyInfo(context);
        }
    }

    public static BuildVersion DetermineBuildVersion(ICakeContext context) {
        return new BuildVersion(context);
    }

    private BuildVersion(ICakeContext context) {
        // VersionAssemblyInfo = context.Argument("versionAssemblyInfo", "VersionAssemblyInfo.cs");
        VersionAssemblyInfo = context.Argument("versionAssemblyInfo", "AssemblyInfo.cs");

        UseBranchVersion(context);
        LogVersionInformation(context);
    }

    private void LogVersionInformation(ICakeContext context) {
        if(context.Log.Verbosity == Verbosity.Diagnostic) {
            context.Verbose("--- ENVIRONMENT ---");
            foreach(var s in context.EnvironmentVariables()) {
                context.Verbose("{0} = {1}", s.Key, s.Value);
            } 
            context.Verbose("-------------------");
        }

        context.Information($"{nameof(Version)} = {Version}");
        //context.Information($"{nameof(SemVersion)} = {SemVersion}");
        //context.Information($"{nameof(CakeVersion)} = {CakeVersion}");
        context.Information($"{nameof(InformationalVersion)} = {InformationalVersion}");
        context.Information($"{nameof(FullSemVersion)} = {FullSemVersion}");        
        context.Information($"{nameof(FileVersion)} = {FileVersion}");        
        context.Information($"{nameof(PackageVersion)} = {PackageVersion}");        
    }

    private void UseVersionAssemblyInfo(ICakeContext context) {
        if(context.FileExists(VersionAssemblyInfo)) {
            context.Verbose($"Looking up semantic version from {VersionAssemblyInfo}");

            var assemblyInfo = context.ParseAssemblyInfo(VersionAssemblyInfo);
            Version = assemblyInfo.AssemblyVersion;
            SemVersion = assemblyInfo.AssemblyInformationalVersion;
            InformationalVersion = assemblyInfo.AssemblyInformationalVersion;
            FullSemVersion = assemblyInfo.AssemblyVersion;
        }
        else
        {
            context.Error("Unable to calculate or retrieve version information");
        }        
    }

    private void UseGitVersion(ICakeContext context) {
        var settings = new GitVersionSettings {
            UpdateAssemblyInfo = true,
            UpdateAssemblyInfoFilePath = VersionAssemblyInfo,         
            NoFetch = true
        };
        settings.ArgumentCustomization = args => args.Append("-ensureassemblyinfo");
        context.Verbose($"Calculating semantic version....");

        if(context.BuildSystem().IsLocalBuild || !context.HasArgument("GitVersionFromBuildServer"))
        {
            context.Verbose("Outputting semantic version as JSON");
            settings.OutputType = GitVersionOutput.Json;
            var gitVersion = context.GitVersion(settings);
            Version = gitVersion.MajorMinorPatch;
            SemVersion = gitVersion.LegacySemVerPadded;
            InformationalVersion = gitVersion.InformationalVersion;
            FullSemVersion = gitVersion.FullSemVer;
        }
        else
        {
            // not working properly
            context.Verbose("Outputting semantic version for BUILDSERVER");
            settings.OutputType = GitVersionOutput.BuildServer;
            context.GitVersion(settings);
            Version = context.EnvironmentVariable("GITVERSION_MAJORMINORPATCH");
            SemVersion = context.EnvironmentVariable("GITVERSION_LEGACYSEMVERPADDED");
            InformationalVersion = context.EnvironmentVariable("GITVERSION_INFORMATIONALVERSION");
            FullSemVersion = context.EnvironmentVariable("GITVERSION_FULLSEMVER");
        }        
    }
}
