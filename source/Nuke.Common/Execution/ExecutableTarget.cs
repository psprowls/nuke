// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuke.Common.Execution
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    internal class ExecutableTarget
    {
        public ExecutableTarget()
        {
        }

        public ExecutableTarget(
            PropertyInfo property,
            Target factory,
            TargetDefinition definition,
            bool isDefault,
            IReadOnlyList<ExecutableTarget> dependencies)
        {
            Property = property;
            Factory = factory;
            Definition = definition;
            Dependencies = dependencies;
            IsDefault = isDefault;
        }

        public PropertyInfo Property { get; }
        public string Name => Property.Name;
        public Target Factory { get; }
        public TargetDefinition Definition { get; }
        public string Description => Definition.Description;
        public List<Func<bool>> Conditions => Definition.Conditions;
        public IReadOnlyList<LambdaExpression> Requirements => Definition.Requirements;
        public IReadOnlyList<Action> Actions => Definition.Actions;
        public IReadOnlyList<ExecutableTarget> Dependencies { get; }
        public bool IsDefault { get; }

        public ExecutionStatus Status { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Skip { get; set; }
    }
}
