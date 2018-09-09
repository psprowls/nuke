// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Utilities.Output;

namespace Nuke.Common.Execution
{
    public abstract class Host
    {
        public static Host Instance { get; } = GetInstance();

        public static IEnumerable<TypeInfo> GetTypes()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            return entryAssembly
                .Concat(entryAssembly.GetReferencedAssemblies().Select(Assembly.Load))
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(Host)));
        }

        private static Host GetInstance()
        {
            var hostTypes = GetTypes();
            var hosts = hostTypes.Select(x => Activator.CreateInstance(x, nonPublic: true)).Cast<Host>().ToList();
            var selectedHost = EnvironmentInfo.Argument(nameof(Host));
            if (selectedHost != null)
            {
                return hosts.SingleOrDefault(x => x.GetType().Name.EqualsOrdinalIgnoreCase(selectedHost))
                    .NotNull(new[] { $"Host with name '{selectedHost}' is not available. Available hosts are:" }
                        .Concat(hosts.Select(x => " - " + x.GetType().Name)).JoinNewLine());
            }

            var runningHosts = hosts.Where(x => x.IsRunning).ToList();
            if (runningHosts.Count > 1)
                runningHosts.RemoveAll(x => x is Hosts.Console);

            return runningHosts
                .SingleOrDefaultOrError("Multiple hosts found: " + runningHosts.Select(x => x.GetType().Name).JoinComma())
                .NotNull("No host found.");
        }

        protected internal abstract bool IsRunning { get; }
        protected internal abstract IOutputSink OutputSink { get; }
    }
}
