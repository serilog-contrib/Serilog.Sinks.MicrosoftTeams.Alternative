// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageFact.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The message card fact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Core
{
    using Newtonsoft.Json;

    /// <summary>
    /// The message card fact.
    /// </summary>
    internal class MicrosoftTeamsMessageFact
    {
        /// <summary>
        /// Gets or sets the name of the card fact.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the card fact.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}