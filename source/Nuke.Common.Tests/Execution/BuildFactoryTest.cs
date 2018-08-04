// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Nuke.Common.Execution;
using Xunit;

namespace Nuke.Common.Tests.Execution
{
    public class BuildFactoryTest
    {
        private NukeBuild _singleton;

        [Fact]
        public void Test()
        {
            var buildFactory = new BuildFactory(x => { _singleton = x; });
            var build = buildFactory.Create<Build>(x => x.Compile);

            _singleton.Should().Be(build);

            var targets = build.ExecutableTargets;
            targets.Select(x => x.Name).Should().BeEquivalentTo(
                nameof(Build.Clean),
                nameof(Build.Compile),
                nameof(Build.Test),
                nameof(Build.Pack),
                nameof(Build.Publish));
            targets.Should().ContainSingle(x => x.IsDefault);

            ExecutableTarget GetTarget(Func<Build, Target> factorySelector) => targets.Single(x => x.Factory == factorySelector(build));

            var clean = GetTarget(x => x.Clean);
            var compile = GetTarget(x => x.Compile);
            var test = GetTarget(x => x.Test);
            var pack = GetTarget(x => x.Pack);
            var publish = GetTarget(x => x.Publish);

            compile.IsDefault.Should().BeTrue();

            clean.Description.Should().Be(build.Description);
            clean.Actions.Should().Equal(build.Actions);
            clean.Conditions.Should().Equal(build.Conditions);
            clean.Requirements.Should().Equal(build.Requirements.Cast<LambdaExpression>());
            
            compile.Dependencies.Should().BeEquivalentTo(clean);
            pack.Dependencies.Should().BeEquivalentTo(compile);
            publish.Dependencies.Should().BeEquivalentTo(test, pack);
        }

        private class Build : NukeBuild
        {
            public readonly string Description = "description";
            public readonly Action[] Actions = { () => { } };
            public readonly Func<bool>[] Conditions = { () => true };
            public readonly Expression<Func<bool>>[] Requirements = { () => true };

            // public Target Release => _ => _
            //     .Before(Clean);

            public Target Clean => _ => _
                .Description(Description)
                .OnlyWhen(Conditions)
                .Requires(Requirements)
                .Executes(Actions);

            public Target Compile => _ => _
                .DependsOn(Clean);

            public Target Test => _ => _
                .DependsOn(Compile);

            public Target Pack => _ => _
                .DependsOn(nameof(Compile));

            public Target Publish => _ => _
                .DependsOn(Test, Pack);
        }
    }
}
