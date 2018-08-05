// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuke.Common.Execution
{
    internal interface IRequirementService
    {
        void ValidateRequirements(NukeBuild build);
    }

    internal class RequirementService : IRequirementService
    {
        public void ValidateRequirements(NukeBuild build)
        {
            foreach (var target in build.ExecutionPlan)
            foreach (var requirement in target.Requirements)
            {
                if (requirement is Expression<Func<bool>> boolExpression)
                {
                    ControlFlow.Assert(boolExpression.Compile().Invoke(), $"Target '{target.Name}' requires '{requirement.Body.ToString()}'.");
                }
                else
                {
                    var memberExpression = requirement.Body is UnaryExpression unaryExpression
                        ? (MemberExpression) unaryExpression.Operand
                        : (MemberExpression) requirement.Body;
                    var field = (FieldInfo) memberExpression.Member;

                    ControlFlow.Assert(field.GetValue(build) != null,
                        $"Target '{target.Name}' requires that field '{field.Name}' must be not null.");
                }
            }
        }
    }
}
