// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nuke.Common.OutputSinks;
using Nuke.Common.Utilities;

namespace Nuke.Common.Execution
{
    internal class BuildManager
    {
        public const string DefaultTarget = "default";

        private readonly IBuildExecutor _buildExecutor;
        private readonly IRequirementService _requirementService;

        public BuildManager(IBuildExecutor buildExecutor, IRequirementService requirementService)
        {
            _buildExecutor = buildExecutor;
            _requirementService = requirementService;
        }

        public int Execute(NukeBuild build)
        {
            Logger.Log(FigletTransform.GetText("NUKE"));
            Logger.Log($"Version: {typeof(BuildManager).GetTypeInfo().Assembly.GetVersionText()}");
            Logger.Log($"Host: {EnvironmentInfo.HostType}");
            Logger.Log();

            try
            {
                HandleEarlyExits(build);

                _requirementService.ValidateRequirements(build);
                _buildExecutor.Execute(build);

                return 0;
            }
            catch
            {
                return 1;
            }
            finally
            {
                OutputSink.WriteSummary(build.ExecutionPlan);
            }
        }

        private static void HandleEarlyExits(NukeBuild build)
        {
            if (build.Help)
            {
                Logger.Log(HelpTextService.GetTargetsText(build));
                Logger.Log(HelpTextService.GetParametersText(build));
            }

            if (build.Graph)
                GraphService.ShowGraph(build);

            if (build.Help || build.Graph)
                Environment.Exit(exitCode: 0);
        }
    }
}
