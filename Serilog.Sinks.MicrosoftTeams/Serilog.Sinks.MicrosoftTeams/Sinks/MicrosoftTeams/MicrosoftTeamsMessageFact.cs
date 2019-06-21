using Newtonsoft.Json;

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// The message card fact.
    /// </summary>
    internal class MicrosoftTeamsMessageFact
    {
        /// <summary>
        /// The name of the card fact.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The value of the card fact.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}