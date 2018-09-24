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

namespace Nuke.Common.Tools.Terraform
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class TerraformTasks
    {
        /// <summary><p>Path to the Terraform executable.</p></summary>
        public static string TerraformPath => ToolPathResolver.GetPathExecutable("terraform");
        /// <summary><p>Terraform</p></summary>
        public static IReadOnlyCollection<Output> Terraform(string arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool logOutput = true, Func<string, string> outputFilter = null)
        {
            var process = ProcessTasks.StartProcess(TerraformPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, null, outputFilter);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>Initialize a configuration</p><p>For more details, visit the <a href="https://www.terraform.io">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> TerraformInit(Configure<TerraformInitSettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new TerraformInitSettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>Apply a configuration</p><p>For more details, visit the <a href="https://www.terraform.io">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> TerraformApply(Configure<TerraformApplySettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new TerraformApplySettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>Destroy a configuration</p><p>For more details, visit the <a href="https://www.terraform.io">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> TerraformDestroy(Configure<TerraformDestroySettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new TerraformDestroySettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>This command is a container for further subcommands.</p><p>For more details, visit the <a href="https://www.terraform.io">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> TerraformNewWorkspace(Configure<TerraformNewWorkspaceSettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new TerraformNewWorkspaceSettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary><p>This command is a container for further subcommands.</p><p>For more details, visit the <a href="https://www.terraform.io">official website</a>.</p></summary>
        public static IReadOnlyCollection<Output> TerraformSelectWorkspace(Configure<TerraformSelectWorkspaceSettings> configurator = null)
        {
            var toolSettings = configurator.InvokeSafe(new TerraformSelectWorkspaceSettings());
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
    }
    #region TerraformInitSettings
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class TerraformInitSettings : ToolSettings
    {
        /// <summary><p>Path to the Terraform executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? TerraformTasks.TerraformPath;
        /// <summary><p>Ask for input for variables if not directly set</p></summary>
        public virtual bool? Input { get; internal set; }
        /// <summary><p> Disables output with coloring</p></summary>
        public virtual bool? NoColor { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("init")
              .Add("-input={value}", Input)
              .Add("-no-color", NoColor);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region TerraformApplySettings
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class TerraformApplySettings : ToolSettings
    {
        /// <summary><p>Path to the Terraform executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? TerraformTasks.TerraformPath;
        /// <summary><p>Skip interactive approval of plan before applying</p></summary>
        public virtual bool? AutoApprove { get; internal set; }
        /// <summary><p>Ask for input for variables if not directly set</p></summary>
        public virtual bool? Input { get; internal set; }
        /// <summary><p> Disables output with coloring</p></summary>
        public virtual bool? NoColor { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("apply")
              .Add("-auto-approve", AutoApprove)
              .Add("-input={value}", Input)
              .Add("-no-color", NoColor);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region TerraformDestroySettings
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class TerraformDestroySettings : ToolSettings
    {
        /// <summary><p>Path to the Terraform executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? TerraformTasks.TerraformPath;
        /// <summary><p>Skip interactive approval of plan before applying</p></summary>
        public virtual bool? AutoApprove { get; internal set; }
        /// <summary><p> Disables output with coloring</p></summary>
        public virtual bool? NoColor { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("destroy")
              .Add("-auto-approve", AutoApprove)
              .Add("-no-color", NoColor);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region TerraformNewWorkspaceSettings
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class TerraformNewWorkspaceSettings : ToolSettings
    {
        /// <summary><p>Path to the Terraform executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? TerraformTasks.TerraformPath;
        /// <summary><p>Workspace Name</p></summary>
        public virtual string Name { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("workspace new")
              .Add("{value}", Name);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region TerraformSelectWorkspaceSettings
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class TerraformSelectWorkspaceSettings : ToolSettings
    {
        /// <summary><p>Path to the Terraform executable.</p></summary>
        public override string ToolPath => base.ToolPath ?? TerraformTasks.TerraformPath;
        /// <summary><p>Workspace Name</p></summary>
        public virtual string Name { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("workspace select")
              .Add("{value}", Name);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region TerraformInitSettingsExtensions
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class TerraformInitSettingsExtensions
    {
        #region Input
        /// <summary><p><em>Sets <see cref="TerraformInitSettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformInitSettings SetInput(this TerraformInitSettings toolSettings, bool? input)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = input;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformInitSettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformInitSettings ResetInput(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformInitSettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformInitSettings EnableInput(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformInitSettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformInitSettings DisableInput(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformInitSettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformInitSettings ToggleInput(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = !toolSettings.Input;
            return toolSettings;
        }
        #endregion
        #region NoColor
        /// <summary><p><em>Sets <see cref="TerraformInitSettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformInitSettings SetNoColor(this TerraformInitSettings toolSettings, bool? noColor)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = noColor;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformInitSettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformInitSettings ResetNoColor(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformInitSettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformInitSettings EnableNoColor(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformInitSettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformInitSettings DisableNoColor(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformInitSettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformInitSettings ToggleNoColor(this TerraformInitSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = !toolSettings.NoColor;
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region TerraformApplySettingsExtensions
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class TerraformApplySettingsExtensions
    {
        #region AutoApprove
        /// <summary><p><em>Sets <see cref="TerraformApplySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformApplySettings SetAutoApprove(this TerraformApplySettings toolSettings, bool? autoApprove)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = autoApprove;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformApplySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformApplySettings ResetAutoApprove(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformApplySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformApplySettings EnableAutoApprove(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformApplySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformApplySettings DisableAutoApprove(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformApplySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformApplySettings ToggleAutoApprove(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = !toolSettings.AutoApprove;
            return toolSettings;
        }
        #endregion
        #region Input
        /// <summary><p><em>Sets <see cref="TerraformApplySettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformApplySettings SetInput(this TerraformApplySettings toolSettings, bool? input)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = input;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformApplySettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformApplySettings ResetInput(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformApplySettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformApplySettings EnableInput(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformApplySettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformApplySettings DisableInput(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformApplySettings.Input"/>.</em></p><p>Ask for input for variables if not directly set</p></summary>
        [Pure]
        public static TerraformApplySettings ToggleInput(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Input = !toolSettings.Input;
            return toolSettings;
        }
        #endregion
        #region NoColor
        /// <summary><p><em>Sets <see cref="TerraformApplySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformApplySettings SetNoColor(this TerraformApplySettings toolSettings, bool? noColor)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = noColor;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformApplySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformApplySettings ResetNoColor(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformApplySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformApplySettings EnableNoColor(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformApplySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformApplySettings DisableNoColor(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformApplySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformApplySettings ToggleNoColor(this TerraformApplySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = !toolSettings.NoColor;
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region TerraformDestroySettingsExtensions
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class TerraformDestroySettingsExtensions
    {
        #region AutoApprove
        /// <summary><p><em>Sets <see cref="TerraformDestroySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformDestroySettings SetAutoApprove(this TerraformDestroySettings toolSettings, bool? autoApprove)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = autoApprove;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformDestroySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformDestroySettings ResetAutoApprove(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformDestroySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformDestroySettings EnableAutoApprove(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformDestroySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformDestroySettings DisableAutoApprove(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformDestroySettings.AutoApprove"/>.</em></p><p>Skip interactive approval of plan before applying</p></summary>
        [Pure]
        public static TerraformDestroySettings ToggleAutoApprove(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.AutoApprove = !toolSettings.AutoApprove;
            return toolSettings;
        }
        #endregion
        #region NoColor
        /// <summary><p><em>Sets <see cref="TerraformDestroySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformDestroySettings SetNoColor(this TerraformDestroySettings toolSettings, bool? noColor)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = noColor;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformDestroySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformDestroySettings ResetNoColor(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = null;
            return toolSettings;
        }
        /// <summary><p><em>Enables <see cref="TerraformDestroySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformDestroySettings EnableNoColor(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = true;
            return toolSettings;
        }
        /// <summary><p><em>Disables <see cref="TerraformDestroySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformDestroySettings DisableNoColor(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = false;
            return toolSettings;
        }
        /// <summary><p><em>Toggles <see cref="TerraformDestroySettings.NoColor"/>.</em></p><p> Disables output with coloring</p></summary>
        [Pure]
        public static TerraformDestroySettings ToggleNoColor(this TerraformDestroySettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NoColor = !toolSettings.NoColor;
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region TerraformNewWorkspaceSettingsExtensions
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class TerraformNewWorkspaceSettingsExtensions
    {
        #region Name
        /// <summary><p><em>Sets <see cref="TerraformNewWorkspaceSettings.Name"/>.</em></p><p>Workspace Name</p></summary>
        [Pure]
        public static TerraformNewWorkspaceSettings SetName(this TerraformNewWorkspaceSettings toolSettings, string name)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Name = name;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformNewWorkspaceSettings.Name"/>.</em></p><p>Workspace Name</p></summary>
        [Pure]
        public static TerraformNewWorkspaceSettings ResetName(this TerraformNewWorkspaceSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Name = null;
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region TerraformSelectWorkspaceSettingsExtensions
    /// <summary><p>Used within <see cref="TerraformTasks"/>.</p></summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class TerraformSelectWorkspaceSettingsExtensions
    {
        #region Name
        /// <summary><p><em>Sets <see cref="TerraformSelectWorkspaceSettings.Name"/>.</em></p><p>Workspace Name</p></summary>
        [Pure]
        public static TerraformSelectWorkspaceSettings SetName(this TerraformSelectWorkspaceSettings toolSettings, string name)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Name = name;
            return toolSettings;
        }
        /// <summary><p><em>Resets <see cref="TerraformSelectWorkspaceSettings.Name"/>.</em></p><p>Workspace Name</p></summary>
        [Pure]
        public static TerraformSelectWorkspaceSettings ResetName(this TerraformSelectWorkspaceSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Name = null;
            return toolSettings;
        }
        #endregion
    }
    #endregion
}
