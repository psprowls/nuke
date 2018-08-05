// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.Common.Execution;
using Xunit;

namespace Nuke.Common.Tests.Execution
{
    public class BuildExecutorTest
    {
        [Fact]
        public void Test()
        {
            var buildExecutor = new BuildExecutor();


            var executableTargets =
                new[]
                {
                  new ExecutableTarget(), 
                };
        }
    }
}
