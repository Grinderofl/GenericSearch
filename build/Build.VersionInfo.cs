using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Git;

partial class Build
{
    string Metadata;
    string Suffix;

    string PrereleaseTag => !string.IsNullOrWhiteSpace(Suffix) ? $"-{Suffix}" : "";

    string AssemblyVersion => $"{Version.Major}.{Version.Minor}.{Version.Build}";
    string FileVersion => $"{AssemblyVersion}.{Version.Revision}";
    string InformationalVersion => $"{AssemblyVersion}{PrereleaseTag}.{Version.Revision}+{Metadata}";
    
    readonly Regex VersionTagRegex = new Regex(@"^v(?<major>\d)+\.(?<minor>\d)+\.?(?<patch>\d)+(.*)");
    readonly Regex PreviewNumberRegex = new Regex(@"-preview(?<number>\d+)");

    Version CreateVersion()
    {
        //var nugetVersion = GitVersion.FullSemVer;
        var maj = GitVersion.Major;
        var min = GitVersion.Minor;
        var pat = GitVersion.Patch;
        
        return new Version(maj, min, pat);

        //var currentTag = GitTasks
        //    .Git("tag")
        //    .Where(x => x.Type == OutputType.Std)
        //    .FirstOrDefault(x => x.Text.StartsWith("v")).Text ?? "v1.0.0";

        //var versionTag = VersionTagRegex.Match(currentTag);

        //var versionMajor = int.TryParse(versionTag.Groups["major"].Value, out var major) ? major : 1;
        //var versionMinor = int.TryParse(versionTag.Groups["minor"].Value, out var minor) ? minor : 0;
        //var versionPatch = int.TryParse(versionTag.Groups["patch"].Value, out var patch) ? patch : 0;
        //var versionRevision = Convert.ToInt32(Pipelines?.BuildId ?? 1);

        //return new Version(versionMajor, versionMinor, versionPatch, versionRevision);
    }

    string CreateMetadata()
    {
        return GitTasks.Git("rev-parse HEAD").FirstOrDefault().Text;
    }

    string CreateSuffix()
    {
        if (IsLocalBuild)
        {
            return "dev";
        }

        if (Pipelines.PullRequestId != null)
        {
            return "pr";
        }

        if (GitRepository.IsOnMasterBranch())
        {
            var buildNumber = Pipelines.BuildNumber;
            return $"ci.{buildNumber}";
        }

        Debug.Assert(GitRepository.Branch != null, "GitRepository.Branch != null");

        var previewNumber = PreviewNumberRegex.Match(GitRepository.Branch);
        if (previewNumber.Success)
        {
            return $"preview.{previewNumber.Groups["number"].Value}";
        }

        return "";
    }
}
