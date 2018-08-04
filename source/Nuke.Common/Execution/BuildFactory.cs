// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Nuke.Common.Utilities;

[assembly: InternalsVisibleTo("Nuke.Common.Tests")]

namespace Nuke.Common.Execution
{
    internal class BuildFactory
    {
        public T Create<T>(Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            var constructors = typeof(T).GetConstructors();
            ControlFlow.Assert(constructors.Length == 1 && constructors.Single().GetParameters().Length == 0,
                $"Type '{typeof(T).Name}' must declare a single parameterless constructor.");

            var build = Activator.CreateInstance<T>();
            var defaultTarget = defaultTargetExpression.Compile().Invoke(build);
            build.ExecutableTargets = GetTargetDefinitions(build, defaultTarget);

            return build;
        }

        private IReadOnlyCollection<ExecutableTarget> GetTargetDefinitions(NukeBuild build, Target defaultTarget)
        {
            var properties = build.GetType()
                .GetProperties(ReflectionService.Instance)
                .Where(x => x.PropertyType == typeof(Target)).ToList();

            var executables = new List<ExecutableTarget>();
            var dependencyDictionary = new Dictionary<ExecutableTarget, List<ExecutableTarget>>();

            foreach (var property in properties)
            {
                var factory = (Target) property.GetValue(build);
                var definition = new TargetDefinition();
                factory.Invoke(definition);
                var isDefault = factory == defaultTarget;
                var dependencies = new List<ExecutableTarget>();
                
                var executable = new ExecutableTarget(property, factory, definition, isDefault, dependencies);

                executables.Add(executable);
                dependencyDictionary.Add(executable, dependencies);
            }

            foreach (var executable in executables)
            {
                var dependencies = GetDependencies(executable, executables).ToList();
                dependencyDictionary[executable].AddRange(dependencies);
            }

            return executables;
        }
        
        private IEnumerable<ExecutableTarget> GetDependencies(ExecutableTarget executable, IReadOnlyCollection<ExecutableTarget> executables)
        {
            foreach (var namedDependency in executable.Definition.NamedDependencies)
            {
                yield return executables.SingleOrDefault(x => x.Name.EqualsOrdinalIgnoreCase(namedDependency))
                             ?? new ExecutableTarget { Status = ExecutionStatus.Absent };
            }

            foreach (var factoryDependency in executable.Definition.FactoryDependencies)
                yield return executables.Single(x => x.Factory == factoryDependency);
                
            foreach (var runAfterDependency in executable.Definition.RunAfterTargets)
                yield return executables.Single(x => x.Factory == runAfterDependency);
                
            foreach (var potentialRunBeforeDependency in executables.Where(x => x != executable))
            {
                if (potentialRunBeforeDependency.Definition.RunBeforeTargets.Any(x => x == executable.Factory))
                    yield return potentialRunBeforeDependency;
            }
        }
    }
}
