// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Utilities.Output;

namespace Nuke.Common.Execution
{
    internal static class BuildExecutor
    {
        public const string DefaultTarget = "default";

        public static int Execute<T>(Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            var executionList = default(IReadOnlyCollection<TargetDefinition>);
            
            try
            {
                var build = CreateBuildInstance(defaultTargetExpression);

                Logger.OutputSink = new SevereMessageCapturingOutputSink(
                    new FilteringOutputSink(
                        Host.Instance.OutputSink,
                        () => build.LogLevel));
                
                Logger.Block("NUKE").Dispose();
                Logger.Log($"Version: {typeof(BuildExecutor).GetTypeInfo().Assembly.GetInformationalText()}");
                Logger.Log($"Host: {Host.Instance.GetType().Name}");
                Logger.Log();
                
                InjectionService.InjectValues(build);
                HandleEarlyExits(build);

                executionList = TargetDefinitionLoader.GetExecutingTargets(build, build.InvokedTargets);
                RequirementService.ValidateRequirements(executionList, build);
                Execute(executionList);

                return 0;
            }
            catch (Exception exception)
            {
                HandleException(exception);
                return 1;
            }
            finally
            {
                if (Logger.OutputSink is SevereMessageCapturingOutputSink outputSink)
                {
                    Logger.OutputSink = Host.Instance.OutputSink;
                    LogWarningsAndErrors(outputSink);
                }

                if (executionList != null)
                    LogSummary(executionList);
            }
        }

        internal static void Execute(IEnumerable<TargetDefinition> executionList)
        {
            foreach (var target in executionList)
            {
                if (target.Factory == null)
                {
                    target.Status = ExecutionStatus.Absent;
                    continue;
                }

                if (target.Skip || target.DependencyBehavior == DependencyBehavior.Execute && target.Conditions.Any(x => !x()))
                {
                    target.Status = ExecutionStatus.Skipped;
                    continue;
                }

                using (Logger.Block(target.Name))
                {
                    var stopwatch = Stopwatch.StartNew();
                    try
                    {
                        target.Actions.ForEach(x => x());
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

        private static void HandleEarlyExits<T>(T build)
            where T : NukeBuild
        {
            if (build.Help)
            {
                Logger.Log(HelpTextService.GetTargetsText(build));
                Logger.Log(HelpTextService.GetParametersText(build));
            }

            if (build.Graph)
                GraphService.ShowGraph(build);

            if (build.Help || build.Graph)
                Environment.Exit(exitCode: 0);
        }

        private static T CreateBuildInstance<T>(Expression<Func<T, Target>> defaultTargetExpression)
            where T : NukeBuild
        {
            var constructors = typeof(T).GetConstructors();
            ControlFlow.Assert(constructors.Length == 1 && constructors.Single().GetParameters().Length == 0,
                $"Type '{typeof(T).Name}' must declare a single parameterless constructor.");

            var build = Activator.CreateInstance<T>();
            build.TargetDefinitions = build.GetTargetDefinitions(defaultTargetExpression);
            NukeBuild.Instance = build;

            return build;
        }
        
        private static void HandleException(Exception exception)
        {
            switch (exception)
            {
                case AggregateException ex:
                    ex.InnerExceptions.ForEach(HandleException);
                    break;
                case TargetInvocationException ex:
                    HandleException(ex.InnerException);
                    break;
                case TypeInitializationException ex:
                    HandleException(ex.InnerException);
                    break;
                default:
                    Logger.Error(exception.Message, exception.StackTrace);
                    break;
            }
        }

        public static void LogWarningsAndErrors(SevereMessageCapturingOutputSink outputSink)
        {
            if (outputSink.SevereMessages.Count <= 0)
                return;
            
            Logger.Log(string.Empty);
            Logger.Log("Repeating warnings and errors:");
        
            foreach (var severeMessage in outputSink.SevereMessages)
            {
                switch (severeMessage.Item1)
                {
                    case LogLevel.Warning:
                        Logger.Warn(severeMessage.Item2);
                        break;
                    case LogLevel.Error:
                        Logger.Error(severeMessage.Item2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void LogSummary(IReadOnlyCollection<TargetDefinition> executionList)
        {
            var firstColumn = Math.Max(executionList.Max(x => x.Name.Length) + 4, val2: 20);
            var secondColumn = 10;
            var thirdColumn = 10;
            var allColumns = firstColumn + secondColumn + thirdColumn;
            var totalDuration = executionList.Aggregate(TimeSpan.Zero, (t, x) => t.Add(x.Duration));

            string CreateLine(string target, string executionStatus, string duration)
                => target.PadRight(firstColumn, paddingChar: ' ')
                   + executionStatus.PadRight(secondColumn, paddingChar: ' ')
                   + duration.PadLeft(thirdColumn, paddingChar: ' ');

            string ToMinutesAndSeconds(TimeSpan duration)
                => $"{(int) duration.TotalMinutes}:{duration:ss}";

            Logger.Log();
            Logger.Log(new string(c: '=', count: allColumns));
            Logger.Log(CreateLine("Target", "Status", "Duration"));
            Logger.Log(new string(c: '-', count: allColumns));
            foreach (var target in executionList)
            {
                var line = CreateLine(target.Name, target.Status.ToString(), ToMinutesAndSeconds(target.Duration));
                switch (target.Status)
                {
                    case ExecutionStatus.Absent:
                    case ExecutionStatus.Skipped:
                        Logger.Log(line);
                        break;
                    case ExecutionStatus.Executed:
                        Logger.Success(line);
                        break;
                    case ExecutionStatus.NotRun:
                    case ExecutionStatus.Failed:
                        Logger.Error(line);
                        break;
                }
            }

            Logger.Log(new string(c: '-', count: allColumns));
            Logger.Log(CreateLine("Total", "", ToMinutesAndSeconds(totalDuration)));
            Logger.Log(new string(c: '=', count: allColumns));
            Logger.Log();
            if (executionList.All(x => x.Status != ExecutionStatus.Failed && x.Status != ExecutionStatus.NotRun))
                Logger.Success($"Build succeeded on {DateTime.Now.ToString(CultureInfo.CurrentCulture)}.");
            else
                Logger.Error($"Build failed on {DateTime.Now.ToString(CultureInfo.CurrentCulture)}.");
            Logger.Log();
        }
    }
}
