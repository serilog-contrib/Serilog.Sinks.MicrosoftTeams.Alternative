using Newtonsoft.Json;
using System.Collections.Generic;

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// The message section.
    /// </summary>
    internal class MicrosoftTeamsMessageSection
    {
        /// <summary>
        /// The message section title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The message section facts.
        /// </summary>
        [JsonProperty("facts")]
        public IList<MicrosoftTeamsMessageFact> Facts { get; set; }
    }
}