// Generated with Nuke.CodeGeneration, Version: Local

using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Nuke.Common.Tools.Serverless
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class ServerlessTasks
    {
        /// <summary><p>Path to the Serverless executable.</p></summary>
        public static string ServerlessPath => ToolPathResolver.GetPathExecutable("serverless");
        /// <summary><p>Serverless Framework</p></summary>
        public static IReadOnlyCollection<Output> Serverless(string arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool logOutput = true, Func<string, string> outputFilter = null)
        {
            var process = ProcessTasks.StartProcess(ServerlessPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, null, outputFilter);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>Deploy a service</p><p>For more details, visit the <a href="https://www.serverless.com/">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> ServerlessDeploy(Configure<ServerlessDeploySettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new ServerlessDeploySettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>Remove a service deployment</p><p>For more details, visit the <a href="https://www.serverless.com/">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> ServerlessRemove(Configure<ServerlessRemoveSettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new ServerlessRemoveSettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
    }
    #region ServerlessDeploySettings
    /// <summary><p>Used within <see cref="ServerlessTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class ServerlessDeploySettings : ToolSettings
    {
        /// <summary><p>Path to the Serverless executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? ServerlessTasks.ServerlessPath;
        /// <summary><p>Stage</p></summary>
        public virtual string Stage { get; internal set; }
        /// <summary><p>Environment</p></summary>
        public virtual string Environment { get; internal set; }
        /// <summary><p>Alias</p></summary>
        public virtual string Alias { get; internal set; }
        /// <summary><p>Forces Serverless to deploy</p></summary>
        public virtual bool? Force { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("deploy")
              .Add("--stage {value}", Stage)
              .Add("--environment {value}", Environment)
              .Add("--alias {value}", Alias)
              .Add("--force", Force);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region ServerlessRemoveSettings
    /// <summary><p>Used within <see cref="ServerlessTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class ServerlessRemoveSettings : ToolSettings
    {
        /// <summary><p>Path to the Serverless executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? ServerlessTasks.ServerlessPath;
        /// <summary><p>Stage</p></summary>
        public virtual string Stage { get; internal set; }
        /// <summary><p>Environment</p></summary>
        public virtual string Environment { get; internal set; }
        /// <summary><p>Alias</p></summary>
        public virtual string Alias { get; internal set; }
        /// <summary><p>Forces npm to fetch remote resources even if a local copy exists on disk.</p></summary>
        public virtual bool? Force { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("remove")
              .Add("{value}", Stage)
              .Add("{value}", Environment)
              .Add("{value}", Alias)
              .Add("--force", Force);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region ServerlessDeploySettingsExtensions
    /// <summary><p>Used within <see cref="ServerlessTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class ServerlessDeploySettingsExtensions
    {
        #region Stage
        /// <summary><p><em>Sets <see cref="ServerlessDeploySettings.Stage"/>.</em></p><p>Stage</p></summary>
        [Pure]
        public static ServerlessDeploySettings SetStage(this ServerlessDeploySettings toolSettings, string stage)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Stage = stage;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessDeploySettings.Stage"/>.</em></p><p>Stage</p></summary>
        [Pure]
        public static ServerlessDeploySettings ResetStage(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Stage = null;
            return toolSettings;
        }
        #endregion
        #region Environment
        /// <summary><p><em>Sets <see cref="ServerlessDeploySettings.Environment"/>.</em></p><p>Environment</p></summary>
        [Pure]
        public static ServerlessDeploySettings SetEnvironment(this ServerlessDeploySettings toolSettings, string environment)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Environment = environment;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessDeploySettings.Environment"/>.</em></p><p>Environment</p></summary>
        [Pure]
        public static ServerlessDeploySettings ResetEnvironment(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Environment = null;
            return toolSettings;
        }
        #endregion
        #region Alias
        /// <summary><p><em>Sets <see cref="ServerlessDeploySettings.Alias"/>.</em></p><p>Alias</p></summary>
        [Pure]
        public static ServerlessDeploySettings SetAlias(this ServerlessDeploySettings toolSettings, string alias)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Alias = alias;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessDeploySettings.Alias"/>.</em></p><p>Alias</p></summary>
        [Pure]
        public static ServerlessDeploySettings ResetAlias(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Alias = null;
            return toolSettings;
        }
        #endregion
        #region Force
        /// <summary><p><em>Sets <see cref="ServerlessDeploySettings.Force"/>.</em></p><p>Forces Serverless to deploy</p></summary>
        [Pure]
        public static ServerlessDeploySettings SetForce(this ServerlessDeploySettings toolSettings, bool? force)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = force;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessDeploySettings.Force"/>.</em></p><p>Forces Serverless to deploy</p></summary>
        [Pure]
        public static ServerlessDeploySettings ResetForce(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="ServerlessDeploySettings.Force"/>.</em></p><p>Forces Serverless to deploy</p></summary>
        [Pure]
        public static ServerlessDeploySettings EnableForce(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="ServerlessDeploySettings.Force"/>.</em></p><p>Forces Serverless to deploy</p></summary>
        [Pure]
        public static ServerlessDeploySettings DisableForce(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="ServerlessDeploySettings.Force"/>.</em></p><p>Forces Serverless to deploy</p></summary>
        [Pure]
        public static ServerlessDeploySettings ToggleForce(this ServerlessDeploySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = !toolSettings.Force;
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region ServerlessRemoveSettingsExtensions
    /// <summary><p>Used within <see cref="ServerlessTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class ServerlessRemoveSettingsExtensions
    {
        #region Stage
        /// <summary><p><em>Sets <see cref="ServerlessRemoveSettings.Stage"/>.</em></p><p>Stage</p></summary>
        [Pure]
        public static ServerlessRemoveSettings SetStage(this ServerlessRemoveSettings toolSettings, string stage)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Stage = stage;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessRemoveSettings.Stage"/>.</em></p><p>Stage</p></summary>
        [Pure]
        public static ServerlessRemoveSettings ResetStage(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Stage = null;
            return toolSettings;
        }
        #endregion
        #region Environment
        /// <summary><p><em>Sets <see cref="ServerlessRemoveSettings.Environment"/>.</em></p><p>Environment</p></summary>
        [Pure]
        public static ServerlessRemoveSettings SetEnvironment(this ServerlessRemoveSettings toolSettings, string environment)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Environment = environment;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessRemoveSettings.Environment"/>.</em></p><p>Environment</p></summary>
        [Pure]
        public static ServerlessRemoveSettings ResetEnvironment(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Environment = null;
            return toolSettings;
        }
        #endregion
        #region Alias
        /// <summary><p><em>Sets <see cref="ServerlessRemoveSettings.Alias"/>.</em></p><p>Alias</p></summary>
        [Pure]
        public static ServerlessRemoveSettings SetAlias(this ServerlessRemoveSettings toolSettings, string alias)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Alias = alias;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessRemoveSettings.Alias"/>.</em></p><p>Alias</p></summary>
        [Pure]
        public static ServerlessRemoveSettings ResetAlias(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Alias = null;
            return toolSettings;
        }
        #endregion
        #region Force
        /// <summary><p><em>Sets <see cref="ServerlessRemoveSettings.Force"/>.</em></p><p>Forces npm to fetch remote resources even if a local copy exists on disk.</p></summary>
        [Pure]
        public static ServerlessRemoveSettings SetForce(this ServerlessRemoveSettings toolSettings, bool? force)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = force;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="ServerlessRemoveSettings.Force"/>.</em></p><p>Forces npm to fetch remote resources even if a local copy exists on disk.</p></summary>
        [Pure]
        public static ServerlessRemoveSettings ResetForce(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="ServerlessRemoveSettings.Force"/>.</em></p><p>Forces npm to fetch remote resources even if a local copy exists on disk.</p></summary>
        [Pure]
        public static ServerlessRemoveSettings EnableForce(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="ServerlessRemoveSettings.Force"/>.</em></p><p>Forces npm to fetch remote resources even if a local copy exists on disk.</p></summary>
        [Pure]
        public static ServerlessRemoveSettings DisableForce(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="ServerlessRemoveSettings.Force"/>.</em></p><p>Forces npm to fetch remote resources even if a local copy exists on disk.</p></summary>
        [Pure]
        public static ServerlessRemoveSettings ToggleForce(this ServerlessRemoveSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Force = !toolSettings.Force;
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region NpmOnlyMode
    /// <summary><p>Used within <see cref="ServerlessTasks"/>.</p></summary>
    [PublicAPI]
    [Serializable]
    [ExcludeFromCodeCoverage]
    public partial class NpmOnlyMode : Enumeration
    {
        public static NpmOnlyMode production = new NpmOnlyMode { Value = "production" };
        public static NpmOnlyMode development = new NpmOnlyMode { Value = "development" };
    }
    #endregion
}
