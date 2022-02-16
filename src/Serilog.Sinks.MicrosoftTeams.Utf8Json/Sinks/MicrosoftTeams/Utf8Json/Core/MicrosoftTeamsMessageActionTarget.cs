// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageActionTarget.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The Microsoft Teams message action target class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Utf8Json.Core;

/// <summary>
/// The Microsoft Teams message action target class.
/// </summary>
public class MicrosoftTeamsMessageActionTarget
{
    /// <summary>
    /// Gets or sets the operating system.
    /// </summary>
    [DataMember(Name = "os")]
    public string OperatingSystem { get; set; } = string.Empty;
}
