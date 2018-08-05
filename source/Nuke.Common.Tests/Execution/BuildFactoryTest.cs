// // Copyright Matthias Koch, Sebastian Karasek 2018.
// // Distributed under the MIT License.
// // https://github.com/nuke-build/nuke/blob/master/LICENSE
//
// using System;
// using System.Linq;
// using System.Linq.Expressions;
// using FakeItEasy;
// using FluentAssertions;
// using Nuke.Common.Execution;
// using Xunit;
//
// namespace Nuke.Common.Tests.Execution
// {
//     public class BuildFactoryTest
//     {
//         [Fact]
//         public void Test()
//         {
//             var injectionService = A.Fake<IInjectionService>();
//             var executionPlanner = A.Fake<IExecutionPlanner>();
//             var executionPlan = new ExecutableTarget[0];
//             A.CallTo(() => executionPlanner.GetExecutionPlan(A<NukeBuild>._, A<string[]>._)).Returns(executionPlan);
//
//             var buildFactory = new BuildFactory(TODO, injectionService, executionPlanner);
//             var build = buildFactory.Create<Build>(x => x.Compile);
//
//             A.CallTo(() => injectionService.InjectValues(build)).MustHaveHappened();
//             A.CallTo(() => executionPlanner.GetExecutionPlan(build, A<string[]>._)).MustHaveHappened();
//             build.ExecutionPlan.Should().BeSameAs(executionPlan);
//             
//             var targets = build.ExecutableTargets;
//             targets.Select(x => x.Name).Should().BeEquivalentTo(
//                 nameof(Build.Merge),
//                 nameof(Build.Clean),
//                 nameof(Build.Compile),
//                 nameof(Build.Test),
//                 nameof(Build.Pack),
//                 nameof(Build.Analyze),
//                 nameof(Build.Publish));
//             targets.Should().ContainSingle(x => x.IsDefault);
//
//             ExecutableTarget GetTarget(string targetName) => targets.Single(x => x.Name == targetName);
//
//             var merge = GetTarget(nameof(Build.Merge));
//             var clean = GetTarget(nameof(Build.Clean));
//             var compile = GetTarget(nameof(Build.Compile));
//             var test = GetTarget(nameof(Build.Test));
//             var pack = GetTarget(nameof(Build.Pack));
//             var analyze = GetTarget(nameof(Build.Analyze));
//             var publish = GetTarget(nameof(Build.Publish));
//
//             compile.IsDefault.Should().BeTrue();
//
//             clean.Description.Should().Be(build.Description);
//             clean.Actions.Should().Equal(build.Actions);
//             clean.Conditions.Should().Equal(build.Conditions);
//             clean.Requirements.Should().Equal(build.Requirements.Cast<LambdaExpression>());
//
//             clean.Dependencies.Should().BeEquivalentTo(merge);
//             compile.Dependencies.Should().BeEquivalentTo(clean);
//             pack.Dependencies.Should().BeEquivalentTo(compile);
//             analyze.Dependencies.Should().BeEquivalentTo(clean);
//             publish.Dependencies.Should().BeEquivalentTo(test, pack);
//         }
//
//         private class Build : NukeBuild
//         {
//             public readonly string Description = "description";
//             public readonly Action[] Actions = { () => { } };
//             public readonly Func<bool>[] Conditions = { () => true };
//             public readonly Expression<Func<bool>>[] Requirements = { () => true };
//
//             public Target Merge => _ => _
//                 .Before(Clean);
//
//             public Target Clean => _ => _
//                 .Description(Description)
//                 .OnlyWhen(Conditions)
//                 .Requires(Requirements)
//                 .Executes(Actions);
//
//             public Target Compile => _ => _
//                 .DependsOn(Clean);
//
//             public Target Test => _ => _
//                 .DependsOn(Compile);
//
//             public Target Pack => _ => _
//                 .DependsOn(nameof(Compile));
//
//             public Target Analyze => _ => _
//                 .After(Clean);
//
//             public Target Publish => _ => _
//                 .DependsOn(Test, Pack);
//         }
//     }
// }
