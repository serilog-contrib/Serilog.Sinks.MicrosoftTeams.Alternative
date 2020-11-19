// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageActionTarget.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The Microsoft Teams message action target class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    using Newtonsoft.Json;

    /// <summary>
    /// The Microsoft Teams message action target class.
    /// </summary>
    public class MicrosoftTeamsMessageActionTarget
    {
        /// <summary>
        /// Gets or sets the operating system.
        /// </summary>
        [JsonProperty("os")]
        public string OperatingSystem { get; set; }
    }
}