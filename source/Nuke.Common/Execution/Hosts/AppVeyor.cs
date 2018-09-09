// Copyright 2018 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Utilities.Output;

namespace Nuke.Common.Execution.Hosts
{
    /// <summary>
    /// Interface according to the <a href="https://www.appveyor.com/docs/environment-variables/">official website</a>.
    /// </summary>
    [PublicAPI]
    [BuildServer]
    [ExcludeFromCodeCoverage]
    public class AppVeyor : Host
    {
        internal AppVeyor()
        {
        }

        protected internal override bool IsRunning => Environment.GetEnvironmentVariable("APPVEYOR") != null;
        protected internal override IOutputSink OutputSink => new ConsoleOutputSink();

        public string ApiUrl => EnvironmentInfo.Variable("APPVEYOR_API_URL");
        public string AccountName => EnvironmentInfo.Variable("APPVEYOR_ACCOUNT_NAME");
        public int ProjectId => EnvironmentInfo.Variable<int>("APPVEYOR_PROJECT_ID");
        public string ProjectName => EnvironmentInfo.Variable("APPVEYOR_PROJECT_NAME");
        public string ProjectSlug => EnvironmentInfo.Variable("APPVEYOR_PROJECT_SLUG");
        public string BuildFolder => EnvironmentInfo.Variable("APPVEYOR_BUILD_FOLDER");
        public int BuildId => EnvironmentInfo.Variable<int>("APPVEYOR_BUILD_ID");
        public int BuildNumber => EnvironmentInfo.Variable<int>("APPVEYOR_BUILD_NUMBER");
        public string BuildVersion => EnvironmentInfo.Variable("APPVEYOR_BUILD_VERSION");
        public string BuildWorkerImage => EnvironmentInfo.Variable("APPVEYOR_BUILD_WORKER_IMAGE");
        public int PullRequestNumber => EnvironmentInfo.Variable<int>("APPVEYOR_PULL_REQUEST_NUMBER");
        [CanBeNull] public string PullRequestTitle => EnvironmentInfo.Variable("APPVEYOR_PULL_REQUEST_TITLE");
        public string JobId => EnvironmentInfo.Variable("APPVEYOR_JOB_ID");
        [CanBeNull] public string JobName => EnvironmentInfo.Variable("APPVEYOR_JOB_NAME");
        public int JobNumber => EnvironmentInfo.Variable<int>("APPVEYOR_JOB_NUMBER");
        public string RepositoryProvider => EnvironmentInfo.Variable("APPVEYOR_REPO_PROVIDER");
        public string RepositoryScm => EnvironmentInfo.Variable("APPVEYOR_REPO_SCM");
        public string RepositoryName => EnvironmentInfo.Variable("APPVEYOR_REPO_NAME");
        public string RepositoryBranch => EnvironmentInfo.Variable("APPVEYOR_REPO_BRANCH");
        public bool RepositoryTag => EnvironmentInfo.Variable<bool>("APPVEYOR_REPO_TAG");
        [CanBeNull] public string RepositoryTagName => EnvironmentInfo.Variable("APPVEYOR_REPO_TAG_NAME");
        public string RepositoryCommitSha => EnvironmentInfo.Variable("APPVEYOR_REPO_COMMIT");
        public string RepositoryCommitAuthor => EnvironmentInfo.Variable("APPVEYOR_REPO_COMMIT_AUTHOR");
        public string RepositoryCommitAuthorEmail => EnvironmentInfo.Variable("APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL");
        public DateTime RepositoryCommitTimestamp => EnvironmentInfo.Variable<DateTime>("APPVEYOR_REPO_COMMIT_TIMESTAMP");
        public string RepositoryCommitMessage => EnvironmentInfo.Variable("APPVEYOR_REPO_COMMIT_MESSAGE");
        [CanBeNull] public string RepositoryCommitMessageExtended => EnvironmentInfo.Variable("APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED");
        public bool ScheduledBuild => EnvironmentInfo.Variable<bool>("APPVEYOR_SCHEDULED_BUILD");
        public bool ForcedBuild => EnvironmentInfo.Variable<bool>("APPVEYOR_FORCED_BUILD");
        public bool Rebuild => EnvironmentInfo.Variable<bool>("APPVEYOR_RE_BUILD");
        [CanBeNull] public string Platform => EnvironmentInfo.Variable("PLATFORM");
        [CanBeNull] public string Configuration => EnvironmentInfo.Variable("CONFIGURATION");
    }
}
