// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.Common.Execution;
using Nuke.Common.Execution.Hosts;
using Nuke.Common.Utilities;

namespace Nuke.Common.Tools.MSBuild
{
    public static partial class MSBuildSettingsExtensions
    {
        /// <summary><em>Sets <see cref="MSBuildSettings.TargetPath" />.</em></summary>
        public static MSBuildSettings SetSolutionFile(this MSBuildSettings toolSettings, string solutionFile)
        {
            return toolSettings.SetTargetPath(solutionFile);
        }

        /// <summary><em>Sets <see cref="MSBuildSettings.TargetPath" />.</em></summary>
        public static MSBuildSettings SetProjectFile(this MSBuildSettings toolSettings, string projectFile)
        {
            return toolSettings.SetTargetPath(projectFile);
        }

        public static MSBuildSettings AddTeamCityLogger(this MSBuildSettings toolSettings)
        {
            var teamCity = (Host.Instance as TeamCity).NotNull("TeamCity.Instance != null");
            var teamCityLogger = teamCity.ConfigurationProperties["TEAMCITY_DOTNET_MSBUILD_EXTENSIONS4_0"];
            return toolSettings
                .AddLoggers($"JetBrains.BuildServer.MSBuildLoggers.MSBuildLogger,{teamCityLogger}")
                .EnableNoConsoleLogger();
        }
    }
}
