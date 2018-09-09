// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Execution;
using Nuke.Common.Execution.Hosts;
using Nuke.Common.Tools.Git;
using Nuke.Common.Utilities;

namespace Nuke.Common.Git
{
    /// <inheritdoc/>
    /// <summary>
    /// Implements auto-injection for <see cref="GitRepository"/>.
    /// <para/>
    /// <inheritdoc/>
    /// </summary>
    [PublicAPI]
    [UsedImplicitly(ImplicitUseKindFlags.Default)]
    public class GitRepositoryAttribute : StaticInjectionAttributeBase
    {
        public static GitRepository Value { get; private set; }

        private static Lazy<string> s_branch = new Lazy<string>(()
            => (Host.Instance as AppVeyor)?.RepositoryBranch ??
               (Host.Instance as Bitrise)?.GitBranch ??
               (Host.Instance as GitLab)?.CommitRefName ??
               (Host.Instance as Jenkins)?.GitBranch ??
               (Host.Instance as TeamCity)?.BranchName ??
               (Host.Instance as TeamServices)?.SourceBranchName ??
               (Host.Instance as Travis)?.Branch ??
               GitTasks.GitCurrentBranch());

        [CanBeNull]
        public string Branch { get; set; } = s_branch.Value;

        public string Remote { get; set; } = "origin";

        [CanBeNull]
        public override object GetStaticValue()
        {
            return Value = Value
                           ?? ControlFlow.SuppressErrors(() =>
                               GitRepository.FromLocalDirectory(NukeBuild.Instance.RootDirectory, Branch, Remote.NotNull()));
        }
    }
}
