// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuke.Common.Tests")]

namespace Nuke.Common.Execution
{
    internal class BuildFactory
    {
        private readonly Action<NukeBuild> _singletonSetter;

        public BuildFactory(Action<NukeBuild> singletonSetter)
        {
            _singletonSetter = singletonSetter;
        }

        public T Create<T>(Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            var constructors = typeof(T).GetConstructors();
            ControlFlow.Assert(constructors.Length == 1 && constructors.Single().GetParameters().Length == 0,
                $"Type '{typeof(T).Name}' must declare a single parameterless constructor.");

            var build = Activator.CreateInstance<T>();
            var defaultTarget = defaultTargetExpression.Compile().Invoke(build);
            build.TargetDefinitions = GetTargetDefinitions(build, defaultTarget);
            _singletonSetter(build);

            return build;
        }

        private IReadOnlyCollection<TargetDefinition> GetTargetDefinitions(NukeBuild build, Target defaultTarget)
        {
            var targetDefinitions = build.GetType()
                .GetProperties(ReflectionService.Instance)
                .Where(x => x.PropertyType == typeof(Target))
                .Select(x => LoadTargetDefinition(build, x)).ToList();

            var nameDictionary = targetDefinitions.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
            var factoryDictionary = targetDefinitions.ToDictionary(x => x.Factory, x => x);

            foreach (var targetDefinition in targetDefinitions)
            {
                var dependencies = GetDependencies(targetDefinition, nameDictionary, factoryDictionary);
                targetDefinition.TargetDefinitionDependencies.AddRange(dependencies);
                targetDefinition.IsDefault = targetDefinition.Factory == defaultTarget;
                
                targetDefinition.TargetDefinitionDependencies.AddRange(targetDefinition.RunAfterTargets.Select(x => factoryDictionary[x]));
                targetDefinition.TargetDefinitionDependencies.AddRange(
                    targetDefinitions.Where(x => x.RunBeforeTargets.Any(y => y == targetDefinition.Factory)));
            }

            return targetDefinitions;
        }

        private TargetDefinition LoadTargetDefinition(NukeBuild build, PropertyInfo property)
        {
            var targetFactory = (Target) property.GetValue(build);
            return TargetDefinition.Create(property.Name, targetFactory);
        }

        private IEnumerable<TargetDefinition> GetDependencies(
            TargetDefinition targetDefinition,
            IReadOnlyDictionary<string, TargetDefinition> nameDictionary,
            IReadOnlyDictionary<Target, TargetDefinition> factoryDictionary)
        {
            return targetDefinition.NamedDependencies
                .Select(x => nameDictionary.TryGetValue(x, out var namedTarget)
                    ? namedTarget
                    : TargetDefinition.Create(x))
                .Concat(targetDefinition.FactoryDependencies.Select(x => factoryDictionary[x]));
        }
    }
}
