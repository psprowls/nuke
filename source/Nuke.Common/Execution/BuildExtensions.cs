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
    internal static class BuildExtensions
    {
        public static IReadOnlyCollection<MemberInfo> GetParameterMembers(this NukeBuild build)
        {
            return build.GetInjectionMembers()
                .Where(x => x.GetCustomAttribute<ParameterAttribute>() != null).ToList();
        }

        public static IReadOnlyCollection<MemberInfo> GetInjectionMembers(this NukeBuild build)
        {
            var members = build.GetType()
                .GetMembers(ReflectionService.All)
                .Where(x => x.GetCustomAttributes<InjectionAttributeBase>().Any()).ToList();

            var transitiveMembers = members
                .SelectMany(x => x.GetCustomAttributes<InjectionAttributeBase>())
                .SelectMany(x => x.GetType().GetMembers(ReflectionService.All))
                .Where(x => x.GetCustomAttributes<InjectionAttributeBase>().Any()).ToList();

            return members.Concat(transitiveMembers).ToList();
        }
    }
}
