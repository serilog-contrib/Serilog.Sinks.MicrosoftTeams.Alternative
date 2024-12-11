// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageCard.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The teams message card.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Core;

/// <summary>
/// The teams message card.
/// </summary>
internal class MicrosoftTeamsMessage
{
    /// <summary>
    /// Gets the type of the card.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; } = "message";

    [JsonProperty("attachments")]
    public List<MicrosoftTeamsAttachment> Attachments { get; set; } = new List<MicrosoftTeamsAttachment>();
    
}
