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
    /// Interface according to the <a href="http://devcenter.bitrise.io/faq/available-environment-variables/#exposed-by-bitriseio">official website</a>.
    /// </summary>
    [PublicAPI]
    [BuildServer]
    [ExcludeFromCodeCoverage]
    public class Bitrise : Host
    {
        private static DateTime ConvertUnixTimestamp(long timestamp)
        {
            return new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc)
                .AddSeconds(timestamp)
                .ToLocalTime();
        }

        internal Bitrise()
        {
        }

        protected internal override bool IsRunning => Environment.GetEnvironmentVariable("BITRISE_BUILD_URL") != null;
        protected internal override IOutputSink OutputSink => new BitriseOutputSink();
        
        public string BuildUrl => EnvironmentInfo.Variable("BITRISE_BUILD_URL");
        public long BuildNumber => EnvironmentInfo.Variable<long>("BITRISE_BUILD_NUMBER");
        public string AppTitle => EnvironmentInfo.Variable("BITRISE_APP_TITLE");
        public string AppUrl => EnvironmentInfo.Variable("BITRISE_APP_URL");
        public string AppSlug => EnvironmentInfo.Variable("BITRISE_APP_SLUG");
        public string BuildSlug => EnvironmentInfo.Variable("BITRISE_BUILD_SLUG");
        public DateTime BuildTriggerTimestamp => ConvertUnixTimestamp(EnvironmentInfo.Variable<long>("BITRISE_BUILD_TRIGGER_TIMESTAMP"));
        public string RepositoryUrl => EnvironmentInfo.Variable("GIT_REPOSITORY_URL");
        public string GitBranch => EnvironmentInfo.Variable("BITRISE_GIT_BRANCH");
        [CanBeNull] public string GitTag => EnvironmentInfo.Variable("BITRISE_GIT_TAG");
        [CanBeNull] public string GitCommit => EnvironmentInfo.Variable("BITRISE_GIT_COMMIT");
        [CanBeNull] public string GitMessage => EnvironmentInfo.Variable("BITRISE_GIT_MESSAGE");
        [CanBeNull] public long? PullRequest => EnvironmentInfo.Variable<long?>("BITRISE_PULL_REQUEST");
        [CanBeNull] public string ProvisionUrl => EnvironmentInfo.Variable("BITRISE_PROVISION_URL");
        [CanBeNull] public string CertificateUrl => EnvironmentInfo.Variable("BITRISE_CERTIFICATE_URL");
        [CanBeNull] public string CertificatePassphrase => EnvironmentInfo.Variable("BITRISE_CERTIFICATE_PASSPHRASE");
    }
}
