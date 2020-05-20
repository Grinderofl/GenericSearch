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
    public string Major { get; private set; }
    public string Minor { get; private set; }
    public string Patch { get; private set; }
    public bool Preview { get; private set; }
    public string Commits { get; private set; }
    public string Number { get; private set; }
    public string Build { get; private set; }
    public string Tag { get; private set; }

    public static BuildVersion DetermineBuildVersion(ICakeContext context) {
        return new BuildVersion(context);
    }

    private BuildVersion(ICakeContext context) {
        // VersionAssemblyInfo = context.Argument("versionAssemblyInfo", "VersionAssemblyInfo.cs");
        VersionAssemblyInfo = context.Argument("versionAssemblyInfo", "AssemblyInfo.cs");

        UseBranchVersion(context);
        LogVersionInformation(context);
        EnsureVersionInformation();
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
        context.Information($"{nameof(SemVersion)} = {SemVersion}");
        context.Information($"{nameof(CakeVersion)} = {CakeVersion}");
        context.Information($"{nameof(InformationalVersion)} = {InformationalVersion}");
        context.Information($"{nameof(FullSemVersion)} = {FullSemVersion}");        
    }

    private static readonly Regex branchRegex = 
        new Regex(@"^refs/heads/release/(?<major>\d)+\.(?<minor>\d)+\.?(?<patch>\d)*(?<preview>-preview)?(?<number>\d)*$");

    private const string EmptyVersion = "0.0.0.0";
    private const string AssemblyFileVersion = "AssemblyFileVersion(\"{0}\")";
    private const string AssemblyVersion = "AssemblyVersion(\"{0}\")";
    private const string AssemblyInformationalVersion = "AssemblyInformationalVersion(\"{0}\")";
        
    private void UseBranchVersion(ICakeContext context) {
        var branchName = context.EnvironmentVariable("SourceBranch") ?? "";
        context.Information($"Current branch: {branchName}");

        var branchMatch = branchRegex.Match(branchName);
        if(branchMatch.Success) {
            context.Information($"Outputting semantic version from branch");
            Major = branchMatch.Groups["major"].Value;
            Minor = branchMatch.Groups["minor"].Value;
            Patch = branchMatch.Groups["patch"].Success ? branchMatch.Groups["patch"].Value : "0";
            Preview = branchMatch.Groups["preview"].Success;
            Number = branchMatch.Groups["number"].Value;
            Build = context.EnvironmentVariable("Build");

            if(Preview)
            {
                Version = $"{Major}.{Minor}.{Patch}-preview{Number}";
                SemVersion = $"{Major}.{Minor}.{Patch}-preview{Number}.{Commits}";
                InformationalVersion = $"{Major}.{Minor}.{Patch}-preview{Number}.{Commits}.{Build}";
                FullSemVersion = $"{Major}.{Minor}.{Patch}-preview{Number}.{Commits}";
                Tag = $"v{Major}.{Minor}.{Patch}-preview{Number}";
            }
            else
            {
                Version = $"{Major}.{Minor}.{Patch}";
                SemVersion = $"{Major}.{Minor}.{Patch}.{Commits}";
                InformationalVersion = $"{Major}.{Minor}.{Patch}.{Commits}.{Build}";
                FullSemVersion = $"{Major}.{Minor}.{Patch}.{Commits}";
                Tag = $"v{Major}.{Minor}.{Patch}";
            }

            var versions = new Dictionary<string, string>()
            {
                [AssemblyFileVersion] = SemVersion,
                [AssemblyVersion] = SemVersion,
                [AssemblyInformationalVersion] = InformationalVersion
            };

            var assemblyInfo = "AssemblyInfo.cs";
            foreach(var version in versions)
            {
                var source = string.Format(version.Key, EmptyVersion);
                var dest = string.Format(version.Key, version.Value);
                context.ReplaceTextInFiles(assemblyInfo, source, dest);
            }
        }
        else if(context.DirectoryExists(".git")) {
                UseGitVersion(context);
        }
        else {
            UseVersionAssemblyInfo(context);
        }
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

    private void EnsureVersionInformation() {
        if(new [] { Version, SemVersion, FullSemVersion, InformationalVersion }.Any(string.IsNullOrWhiteSpace)) {
            throw new Exception("Version information has not been determined, build failed!");
        }
    }
}
