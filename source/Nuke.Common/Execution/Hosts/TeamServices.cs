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
    /// Interface according to the <a href="https://www.visualstudio.com/en-us/docs/build/define/variables">official website</a>.
    /// </summary>
    [PublicAPI]
    [BuildServer]
    [ExcludeFromCodeCoverage]
    public class TeamServices : Host
    {
        private readonly Action<string> _messageSink;

        internal TeamServices()
            : this(System.Console.WriteLine)
        {
        }

        internal TeamServices(Action<string> messageSink)
        {
            _messageSink = messageSink;
        }

        protected internal override bool IsRunning => Environment.GetEnvironmentVariable("TF_BUILD") != null;
        protected internal override IOutputSink OutputSink => new TeamServicesOutputSink(this);
        
        public string AgentBuildDirectory => EnvironmentInfo.Variable("AGENT_BUILDDIRECTORY");
        public string AgentHomeDirectory => EnvironmentInfo.Variable("AGENT_HOMEDIRECTORY");
        public long AgentId => EnvironmentInfo.Variable<long>("AGENT_ID");
        public TeamServicesJobStatus AgentJobStatus => EnvironmentInfo.Variable<TeamServicesJobStatus>("AGENT_JOBSTATUS");
        public string AgentMachineName => EnvironmentInfo.Variable("AGENT_MACHINENAME");
        public string AgentName => EnvironmentInfo.Variable("AGENT_NAME");
        public string AgentWorkFolder => EnvironmentInfo.Variable("AGENT_WORKFOLDER");
        public string ArtifactStagingDirectory => EnvironmentInfo.Variable("BUILD_ARTIFACTSTAGINGDIRECTORY");
        public long BuildId => EnvironmentInfo.Variable<long>("BUILD_BUILDID");
        public long BuildNumber => EnvironmentInfo.Variable<long>("BUILD_BUILDNUMBER");
        public string BuildUri => EnvironmentInfo.Variable("BUILD_BUILDURI");
        public string BinariesDirectory => EnvironmentInfo.Variable("BUILD_BINARIESDIRECTORY");
        public string DefinitionName => EnvironmentInfo.Variable("BUILD_DEFINITIONNAME");
        public long DefinitionVersion => EnvironmentInfo.Variable<long>("BUILD_DEFINITIONVERSION");
        public string QueuedBy => EnvironmentInfo.Variable("BUILD_QUEUEDBY");
        public Guid QueuedById => EnvironmentInfo.Variable<Guid>("BUILD_QUEUEDBYID");
        public TeamServicesBuildReason BuildReason => EnvironmentInfo.Variable<TeamServicesBuildReason>("BUILD_REASON");
        public bool RepositoryClean => EnvironmentInfo.Variable<bool>("BUILD_REPOSITORY_CLEAN");
        public string RepositoryLocalPath => EnvironmentInfo.Variable("BUILD_REPOSITORY_LOCALPATH");
        public string RepositoryName => EnvironmentInfo.Variable("BUILD_REPOSITORY_NAME");
        public TeamServicesRepositoryType RepositoryProvider => EnvironmentInfo.Variable<TeamServicesRepositoryType>("BUILD_REPOSITORY_PROVIDER");
        [CanBeNull] public string RepositoryTfvcWorkspace => EnvironmentInfo.Variable("BUILD_REPOSITORY_TFVC_WORKSPACE");
        public string RepositoryUri => EnvironmentInfo.Variable("BUILD_REPOSITORY_URI");
        public string RequestedFor => EnvironmentInfo.Variable("BUILD_REQUESTEDFOR");
        public string RequestedForEmail => EnvironmentInfo.Variable("BUILD_REQUESTEDFOREMAIL");
        public Guid RequestedForId => EnvironmentInfo.Variable<Guid>("BUILD_REQUESTEDFORID");
        public string SourceBranch => EnvironmentInfo.Variable("BUILD_SOURCEBRANCH");
        public string SourceBranchName => EnvironmentInfo.Variable("BUILD_SOURCEBRANCHNAME");
        public string SourceDirectory => EnvironmentInfo.Variable("BUILD_SOURCESDIRECTORY");
        public string SourceVersion => EnvironmentInfo.Variable("BUILD_SOURCEVERSION");
        public string StagingDirectory => EnvironmentInfo.Variable("BUILD_STAGINGDIRECTORY");
        public bool RepositoryGitSubmoduleCheckout => EnvironmentInfo.Variable<bool>("BUILD_REPOSITORY_GIT_SUBMODULECHECKOUT");
        [CanBeNull] public string SourceTfvcShelveset => EnvironmentInfo.Variable("BUILD_SOURCETFVCSHELVESET");
        public string TestResultsDirectory => EnvironmentInfo.Variable("COMMON_TESTRESULTSDIRECTORY");
        [CanBeNull] public string AccessToken => EnvironmentInfo.Variable("SYSTEM_ACCESSTOKEN");
        public Guid CollectionId => EnvironmentInfo.Variable<Guid>("SYSTEM_COLLECTIONID");
        public string DefaultWorkingDirectory => EnvironmentInfo.Variable("SYSTEM_DEFAULTWORKINGDIRECTORY");
        public long DefinitionId => EnvironmentInfo.Variable<long>("SYSTEM_DEFINITIONID");
        [CanBeNull] public long? PullRequestId => EnvironmentInfo.Variable<long?>("SYSTEM_PULLREQUEST_PULLREQUESTID");
        [CanBeNull] public string PullRequestSourceBranch => EnvironmentInfo.Variable("SYSTEM_PULLREQUEST_SOURCEBRANCH");
        [CanBeNull] public string PullRequestTargetBranch => EnvironmentInfo.Variable("SYSTEM_PULLREQUEST_TARGETBRANCH");
        public string TeamFoundationCollectionUri => EnvironmentInfo.Variable("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI");
        public string TeamProject => EnvironmentInfo.Variable("SYSTEM_TEAMPROJECT");
        public Guid TeamProjectId => EnvironmentInfo.Variable<Guid>("SYSTEM_TEAMPROJECTID");

        public void UploadLog(string localFilePath)
        {
            _messageSink($"##vso[build.uploadlog]{localFilePath}");
        }

        public void UpdateBuildNumber(string buildNumber)
        {
            _messageSink($"##vso[build.updatebuildnumber]{buildNumber}");
        }

        public void AddBuildTag(string buildTag)
        {
            _messageSink($"##vso[build.addbuildtag]{buildTag}");
        }

        public void LogError(
            string message,
            string sourcePath = null,
            string lineNumber = null,
            string columnNumber = null,
            string code = null)
        {
            LogIssue(TeamServicesIssueType.Error, message, sourcePath, lineNumber, columnNumber, code);
        }

        public void LogWarning(
            string message,
            string sourcePath = null,
            string lineNumber = null,
            string columnNumber = null,
            string code = null)
        {
            LogIssue(TeamServicesIssueType.Warning, message, sourcePath, lineNumber, columnNumber, code);
        }

        public void LogIssue(
            TeamServicesIssueType type,
            string message,
            string sourcePath = null,
            string lineNumber = null,
            string columnNumber = null,
            string code = null)
        {
            var properties = $"type={GetText(type)};";
            if (!string.IsNullOrEmpty(sourcePath))
                properties += $"sourcepath={sourcePath};";

            if (!string.IsNullOrEmpty(lineNumber))
                properties += $"linenumber={lineNumber};";

            if (!string.IsNullOrEmpty(columnNumber))
                properties += $"columnnumber={columnNumber};";

            if (!string.IsNullOrEmpty(code))
                properties += $"code={code};";

            _messageSink($"##vso[task.logissue {properties}]{message}");
        }

        private string GetText(TeamServicesIssueType type)
        {
            switch (type)
            {
                case TeamServicesIssueType.Warning:
                    return "warning";
                case TeamServicesIssueType.Error:
                    return "error";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, message: null);
            }
        }
    }
}
