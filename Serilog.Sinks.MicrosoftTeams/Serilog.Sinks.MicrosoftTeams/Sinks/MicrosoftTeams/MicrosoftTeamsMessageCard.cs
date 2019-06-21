using Newtonsoft.Json;
using System.Collections.Generic;

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// The teams message card.
    /// </summary>
    internal class MicrosoftTeamsMessageCard
    {
        /// <summary>
        /// The type of the card.
        /// </summary>
        [JsonProperty("@type")]
        public string Type { get; } = "MessageCard";

        /// <summary>
        /// The context of the card.
        /// </summary>
        [JsonProperty("@context")]
        public string Context { get; } = "http://schema.org/extensions";

        /// <summary>
        /// The title of the card.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The text of the card.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// The theme color of the card.
        /// </summary>
        [JsonProperty("themeColor")]
        public string Color { get; set; }

        /// <summary>
        /// The sections of the card.
        /// </summary>
        [JsonProperty("sections")]
        public IList<MicrosoftTeamsMessageSection> Sections { get; set; }
    }
}