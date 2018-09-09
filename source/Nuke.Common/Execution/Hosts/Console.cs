// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Utilities.Output;

namespace Nuke.Common.Execution.Hosts
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public sealed class Console : Host
    {
        protected internal override bool IsRunning => true;
        protected internal override IOutputSink OutputSink => new ConsoleOutputSink();
    }
}
