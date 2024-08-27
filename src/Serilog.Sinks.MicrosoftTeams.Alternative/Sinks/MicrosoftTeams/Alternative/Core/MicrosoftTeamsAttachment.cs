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
internal class MicrosoftTeamsAttachment
{
    /// <summary>
    /// Gets the type of the card.
    /// </summary>
    [JsonProperty("contentType")]
    public string ContentType { get; } = "application/vnd.microsoft.card.adaptive";

    [JsonProperty("contentUrl")]
    public string ContentUrl { get; } = null!;

    [JsonProperty("content")]
    [JsonConverter(typeof(AdaptiveCards.AdaptiveCardConverter))]
    public AdaptiveCards.AdaptiveCard Content { get; set; } = null!;
    
}
