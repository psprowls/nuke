// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Nuke.Common.OutputSinks;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Common.Execution
{
    internal interface IBuildExecutor
    {
        void Execute(NukeBuild build);
    }

    internal class BuildExecutor : IBuildExecutor
    {
        public void Execute(NukeBuild build)
        {
            var skippedTargets = ParameterService.Instance.GetCommandLineArgument<string[]>("skip", separator: '+');
            foreach (var skippedTarget in ParameterService.Instance.)

            foreach (var target in build.ExecutionPlan)
            {
                if (build.SkippedTargets.Contains(target.Name, StringComparer.OrdinalIgnoreCase))
                    target.Status = ExecutionStatus.Skipped;
            }

            foreach (var target in build.ExecutionPlan)
            {
                if (target.Status == ExecutionStatus.Skipped ||
                    target.Status == ExecutionStatus.Absent)
                    continue;

                if (target.Conditions.Any(x => !x()))
                {
                    target.Status = ExecutionStatus.Skipped;
                    continue;
                }

                using (Logger.Block(target.Name))
                {
                    var stopwatch = Stopwatch.StartNew();
                    try
                    {
                        target.Actions.ToList().ForEach(x => x());
                        target.Duration = stopwatch.Elapsed;

                        target.Status = ExecutionStatus.Executed;
                    }
                    catch (Exception exception)
                    {
                        target.Status = ExecutionStatus.Failed;
                        HandleException(exception);
                        throw;
                    }
                    finally
                    {
                        target.Duration = stopwatch.Elapsed;
                    }
                }
            }
        }

        private void HandleException(Exception exception)
        {
            switch (exception)
            {
                case AggregateException ex:
                    ex.InnerExceptions.ForEach(HandleException);
                    break;
                case TargetInvocationException ex:
                    HandleException(ex.InnerException);
                    break;
                default:
                    OutputSink.Error(exception.Message, exception.StackTrace);
                    break;
            }
        }
    }
}
