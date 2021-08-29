// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageActionTargetUri.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The Microsoft Teams message target URI class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Core
{
    using Newtonsoft.Json;

    /// <summary>
    /// The Microsoft Teams message target URI class.
    /// </summary>
    public class MicrosoftTeamsMessageActionTargetUri : MicrosoftTeamsMessageActionTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsMessageActionTargetUri"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="operatingSystem">The operating system.</param>
        public MicrosoftTeamsMessageActionTargetUri(string uri, string operatingSystem = "default")
        {
            this.OperatingSystem = operatingSystem;
            this.Uri = uri;
        }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}