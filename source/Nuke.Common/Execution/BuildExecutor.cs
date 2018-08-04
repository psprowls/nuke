// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nuke.Common.Execution
{
    internal static class BuildExecutor
    {
        public static void Execute(IEnumerable<ExecutableTarget> executableTargets)
        {
            foreach (var target in executableTargets)
            {
                if (target.Factory == null)
                {
                    target.Status = ExecutionStatus.Absent;
                    continue;
                }

                if (target.Skip || target.Conditions.Any(x => !x()))
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
                    catch
                    {
                        target.Status = ExecutionStatus.Failed;
                        throw;
                    }
                    finally
                    {
                        target.Duration = stopwatch.Elapsed;
                    }
                }
            }
        }
    }
}
