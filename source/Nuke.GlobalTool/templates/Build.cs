using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Git;                                                                          // GIT
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;                                                                 // DOTNET
using Nuke.Common.Tools.GitVersion;                                                             // GITVERSION
using Nuke.Common.Tools.MSBuild;                                                                // MSBUILD
using static Nuke.Common.ChangeLog.ChangelogTasks;                                              // CHANGELOG
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;                                              // DOTNET
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;                                            // MSBUILD

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter] readonly string Source = "https://api.nuget.org/v3/index.json";                 // NUGET
    [Parameter] readonly string SymbolSource = "https://nuget.smbsrc.net/";                     // NUGET
    [Parameter] readonly string ApiKey;                                                         // NUGET

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;                                       // GIT
    [GitVersion] readonly GitVersion GitVersion;                                                // GITVERSION

    AbsolutePath SourceDirectory => RootDirectory / "source";                                   // SOURCE_DIR
    AbsolutePath SourceDirectory => RootDirectory / "src";                                      // SRC_DIR
    AbsolutePath TestsDirectory => RootDirectory / "tests";                                     // TESTS_DIR
    AbsolutePath OutputDirectory => RootDirectory / "output";                                   // OUTPUT_DIR
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";                             // ARTIFACTS_DIR

    Target Clean => _ => _
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));            // SOURCE_DIR
            DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));            // SRC_DIR
            DeleteDirectories(GlobDirectories(TestsDirectory, "**/bin", "**/obj"));             // TESTS_DIR
            EnsureCleanDirectory(OutputDirectory);                                              // OUTPUT_DIR
            EnsureCleanDirectory(ArtifactsDirectory);                                           // ARTIFACTS_DIR
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            MSBuild(s => s                                                                      // MSBUILD
                .SetTargetPath(SolutionFile)                                                    // MSBUILD
                .SetTargets("Restore"));                                                        // MSBUILD
            DotNetRestore(s => s                                                                // DOTNET
                .SetProjectFile(SolutionFile));                                                 // DOTNET
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => s                                                                      // MSBUILD
                .SetTargetPath(SolutionFile)                                                    // MSBUILD
                .SetTargets("Rebuild")                                                          // MSBUILD
                .SetConfiguration(Configuration)                                                // MSBUILD
                .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())                  // MSBUILD && GITVERSION
                .SetFileVersion(GitVersion.GetNormalizedFileVersion())                          // MSBUILD && GITVERSION
                .SetInformationalVersion(GitVersion.InformationalVersion)                       // MSBUILD && GITVERSION
                .SetMaxCpuCount(Environment.ProcessorCount)                                     // MSBUILD
                .SetNodeReuse(IsLocalBuild));                                                   // MSBUILD
            DotNetBuild(s => s                                                                  // DOTNET
                .SetProjectFile(SolutionFile)                                                   // DOTNET
                .SetConfiguration(Configuration)                                                // DOTNET
                .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())                  // DOTNET && GITVERSION
                .SetFileVersion(GitVersion.GetNormalizedFileVersion())                          // DOTNET && GITVERSION
                .SetInformationalVersion(GitVersion.InformationalVersion)                       // DOTNET && GITVERSION
                .EnableNoRestore());                                                            // DOTNET
        });
    
    string ChangelogFile => RootDirectory / "CHANGELOG.md";                                     // CHANGELOG

    private Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            MSBuild(s => s                                                                      // MSBUILD
                .SetTargetPath(SolutionFile)                                                    // MSBUILD
                .SetTargets("Restore", "Pack")                                                  // MSBUILD
                .SetPackageVersion(GitVersion.NuGetVersionV2)                                   // MSBUILD && GITVERSION
                .SetPackageReleaseNotes(GetNuGetReleaseNotes(ChangelogFile, GitRepository))     // MSBUILD && CHANGELOG
                .SetPackageOutputPath(ArtifactsDirectory)                                       // MSBUILD && ARTIFACTS_DIR
                .SetPackageOutputPath(OutputDirectory)                                          // MSBUILD && OUTPUT_DIR
                .SetConfiguration(Configuration)                                                // MSBUILD
                .EnableIncludeSymbols());                                                       // MSBUILD
            DotNetPack(s => s                                                                   // DOTNET
                .SetProject(SolutionFile)                                                       // DOTNET
                .SetVersion(GitVersion.NuGetVersionV2)                                          // DOTNET && GITVERSION
                .SetPackageReleaseNotes(GetNuGetReleaseNotes(ChangelogFile, GitRepository))     // DOTNET && CHANGELOG
                .SetOutputDirectory(ArtifactsDirectory)                                         // DOTNET && ARTIFACTS_DIR
                .SetOutputDirectory(OutputDirectory)                                            // DOTNET && OUTPUT_DIR
                .SetConfiguration(Configuration)                                                // DOTNET
                .EnableNoBuild()                                                                // DOTNET
                .EnableIncludeSymbols());                                                       // DOTNET
        });
    
    

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var releaseNotes = ChangelogSectionNotes
                .Select(x => x.Replace("- ", "\u2022 ").Replace("`", string.Empty).Replace(",", "%2C"))
                .Concat(string.Empty)
                .Concat($"Full changelog at {GitRepository.GetGitHubBrowseUrl(ChangelogFile)}")
                .JoinNewLine();

            DotNetPack(s => s
                .SetProject(Solution)
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .EnableIncludeSymbols()
                .SetOutputDirectory(OutputDirectory)
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetPackageReleaseNotes(releaseNotes));

            if (InstallGlobalTool)
            {
                SuppressErrors(() => DotNet($"tool uninstall -g {GlobalToolProject.Name}"));
                DotNet($"tool install -g {GlobalToolProject.Name} --add-source {OutputDirectory} --version {GitVersion.NuGetVersionV2}");
            }
        });
}
