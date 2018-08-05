// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuke.Common.Utilities;

namespace Nuke.Common.Execution
{
    internal interface IExecutionPlanner
    {
        IReadOnlyCollection<ExecutableTarget> GetExecutionPlan(NukeBuild build, IReadOnlyCollection<string> invokedTargetNames);
    }

    internal class ExecutionPlanner : IExecutionPlanner
    {
        public IReadOnlyCollection<ExecutableTarget> GetExecutionPlan(NukeBuild build, IReadOnlyCollection<string> invokedTargetNames)
        {
            ControlFlow.Assert(build.ExecutableTargets.All(x => !x.Name.EqualsOrdinalIgnoreCase(BuildManager.DefaultTarget)),
                $"The name '{BuildManager.DefaultTarget}' cannot be used as target name.");

            var invokedTargets = invokedTargetNames.Select(x => GetExecutableTarget(x, build)).ToList();
            var vertexDictionary = build.ExecutableTargets.ToDictionary(x => x, x => new Vertex<ExecutableTarget>(x));
            foreach (var pair in vertexDictionary)
                pair.Value.Dependencies = pair.Key.Dependencies.Select(x => vertexDictionary[x]).ToList();

            var graphAsList = vertexDictionary.Values.ToList();
            var executingTargets = new List<ExecutableTarget>();

            while (graphAsList.Any())
            {
                var independents = graphAsList.Where(x => !graphAsList.Any(y => y.Dependencies.Contains(x))).ToList();
                if (EnvironmentInfo.ArgumentSwitch("strict") && independents.Count > 1)
                {
                    ControlFlow.Fail(
                        new[] { "Incomplete target definition order." }
                            .Concat(independents.Select(x => $"  - {x.Value.Name}"))
                            .JoinNewLine());
                }

                var independent = independents.FirstOrDefault();
                if (independent == null)
                {
                    var scc = new StronglyConnectedComponentFinder<ExecutableTarget>();
                    var cycles = scc.DetectCycle(graphAsList)
                        .Cycles()
                        .Select(x => string.Join(" -> ", x.Select(y => y.Value.Name)));

                    ControlFlow.Fail(
                        new[] { "Circular dependencies between target definitions." }
                            .Concat(independents.Select(x => $"  - {cycles}"))
                            .JoinNewLine());
                }

                graphAsList.Remove(independent);

                var executableTarget = independent.Value;
                if (!invokedTargets.Contains(executableTarget) &&
                    !executingTargets.SelectMany(x => x.Dependencies).Contains(executableTarget))
                    continue;

                executableTarget.Status = ExecutionStatus.NotRun;
                executingTargets.Add(executableTarget);
            }

            executingTargets.Reverse();

            return executingTargets;
        }

        private ExecutableTarget GetExecutableTarget(string targetName, NukeBuild build)
        {
            if (targetName.EqualsOrdinalIgnoreCase(BuildManager.DefaultTarget))
                return build.ExecutableTargets.Single(x => x.IsDefault);

            var targetDefinition = build.ExecutableTargets.SingleOrDefault(x => x.Name.EqualsOrdinalIgnoreCase(targetName));
            if (targetDefinition == null)
            {
                var stringBuilder = new StringBuilder()
                    .AppendLine($"Target with name '{targetName}' is not available.")
                    .AppendLine()
                    .AppendLine(HelpTextService.GetTargetsText(build));

                ControlFlow.Fail(stringBuilder.ToString());
            }

            return targetDefinition;
        }
    }
}
