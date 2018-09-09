// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

namespace Nuke.Common.Utilities.Output
{
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    internal class FilteringOutputSink : IOutputSink
    {
        private readonly IOutputSink _outputSink;
        private readonly Func<LogLevel> _logLevel;

        public FilteringOutputSink(IOutputSink outputSink, Func<LogLevel> logLevel)
        {
            _outputSink = outputSink;
            _logLevel = logLevel;
        }

        public void Write(string text)
        {
            _outputSink.Write(text);
        }

        public IDisposable WriteBlock(string text)
        {
            return _outputSink.WriteBlock(text);
        }

        public void Trace(string text)
        {
            if (_logLevel() > LogLevel.Trace)
                return;

            _outputSink.Trace(text);
        }

        public void Info(string text)
        {
            if (_logLevel() > LogLevel.Information)
                return;

            _outputSink.Info(text);
        }

        public void Warn(string text, string details = null)
        {
            if (_logLevel() > LogLevel.Warning)
                return;

            _outputSink.Warn(text, details);
        }

        public void Error(string text, string details = null)
        {
            _outputSink.Error(text, details);
        }

        public void Success(string text)
        {
            _outputSink.Success(text);
        }
    }
}
