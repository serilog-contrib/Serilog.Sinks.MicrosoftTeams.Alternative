// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageCard.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The teams message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Core;

/// <summary>
/// The teams message.
/// </summary>
internal class MicrosoftTeamsMessage
{
    /// <summary>
    /// Gets the type of the card.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; } = "message";

    /// <summary>
    /// Gets or sets the attachments.
    /// </summary>
    [JsonProperty("attachments")]
    public List<MicrosoftTeamsAttachment> Attachments { get; set; } = [];
}
