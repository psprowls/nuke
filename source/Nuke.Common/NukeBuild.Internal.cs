// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Nuke.Common.BuildServers;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

namespace Nuke.Common
{
    partial class NukeBuild
    {
        private static string[] GetInvokedTargets()
        {
            var argument = ParameterService.Instance.GetCommandLineArgument<string>(position: 1);
            argument = argument == null || argument.StartsWith("-") ? null : argument;

            if (argument != null)
                return argument.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            var parameter = EnvironmentInfo.ParameterSet<string>("target", separator: '+');
            if (parameter != null)
            {
                Logger.Warn(new[]
                            {
                                "The 'Target' parameter is deprecated.",
                                "Starting with the next version, targets need to be specified positional:",
                                string.Empty,
                                "  Usage: build <target1[+target2]> [-parameter value]",
                                string.Empty
                            }.JoinNewLine());
                return parameter;
            }

            return new[] { BuildExecutor.DefaultTarget };
        }
        
        private static string[] GetSkippedTargets()
        {
            if (EnvironmentInfo.ParameterSwitch("nodeps"))
            {
                Logger.Warn(new[]
                            {
                                "The 'NoDeps' switch is deprecated.",
                                "Starting with the next version, you can use the 'Skip' parameter to skip all dependencies or only specified ones:",
                                string.Empty,
                                "  Usage: build -skip",
                                "         build -skip Clean+Restore",
                                string.Empty
                            }.JoinNewLine());
                return new string[0];
            }

            return null;
        }
    }
}
