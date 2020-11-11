namespace Serilog.Sinks.MicrosoftTeams
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal class MicrosoftTeamsMessageAction
    {
        public MicrosoftTeamsMessageAction(string type, string name, MicrosoftTeamsMessageActionTarget target)
        {
            Type = type;
            Name = name;
            Targets = new List<MicrosoftTeamsMessageActionTarget> {target};
        }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("targets")]
        public IList<MicrosoftTeamsMessageActionTarget> Targets { get; set; }
    }

    internal class MicrosoftTeamsMessageActionTarget
    {
        [JsonProperty("os")]
        public string Os { get; set; }
    }

    internal class MicrosoftTeamsMessageActionTargetUri : MicrosoftTeamsMessageActionTarget
    {

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public MicrosoftTeamsMessageActionTargetUri(string uri, string os = "default")
        {
            Os = os;
            Uri = uri;
        }
    }
}
