// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuke.Common.Tests")]

namespace Nuke.Common.Execution
{
    internal class BuildFactory
    {
        private readonly IExecutableTargetLoader _executableTargetLoader;
        private readonly IExecutionPlanner _executionPlanner;

        public BuildFactory(IExecutableTargetLoader executableTargetLoader, IExecutionPlanner executionPlanner)
        {
            _executableTargetLoader = executableTargetLoader;
            _executionPlanner = executionPlanner;
        }

        public T Create<T>(Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            var constructors = typeof(T).GetConstructors();
            ControlFlow.Assert(constructors.Length == 1 && constructors.Single().GetParameters().Length == 0,
                $"Type '{typeof(T).Name}' must declare a single parameterless constructor.");

            var build = Activator.CreateInstance<T>();

            build.ExecutableTargets = _executableTargetLoader.GetExecutableTargets(build, defaultTargetExpression);
            build.ExecutionPlan = _executionPlanner.GetExecutionPlan(build, EnvironmentInfo.InvokedTargets);

            return build;
        }
    }
}
