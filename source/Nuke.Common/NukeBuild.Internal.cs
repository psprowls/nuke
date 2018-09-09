// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Common
{
    partial class NukeBuild
    {
        public static PathUtility.AbsolutePath BuildAssemblyDirectory
        {
            get
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                ControlFlow.Assert(entryAssembly.GetTypes().Any(x => x.IsSubclassOf(typeof(NukeBuild))),
                    $"{entryAssembly} doesn't contain a NukeBuild class.");
                return (PathUtility.AbsolutePath) Path.GetDirectoryName(entryAssembly.Location).NotNull();
            }
        }

        public static PathUtility.AbsolutePath BuildProjectDirectory
        {
            get
            {
                var buildProjectDirectory = new DirectoryInfo(BuildAssemblyDirectory)
                    .DescendantsAndSelf(x => x.Parent)
                    .Select(x => x.GetFiles("*.csproj", SearchOption.TopDirectoryOnly)
                        .SingleOrDefaultOrError($"Found multiple project files in '{x}'."))
                    .FirstOrDefault(x => x != null)
                    ?.DirectoryName;
                return (PathUtility.AbsolutePath) buildProjectDirectory.NotNull("buildProjectDirectory != null");
            }
        }
        
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
