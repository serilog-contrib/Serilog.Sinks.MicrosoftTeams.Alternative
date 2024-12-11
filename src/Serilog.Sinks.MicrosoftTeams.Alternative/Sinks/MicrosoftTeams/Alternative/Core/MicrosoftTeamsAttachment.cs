// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsAttachment.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The attachment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Core;

/// <summary>
/// The attachment.
/// </summary>
internal class MicrosoftTeamsAttachment
{
    /// <summary>
    /// Gets the content type of the card.
    /// </summary>
    [JsonProperty("contentType")]
    public string ContentType { get; } = "application/vnd.microsoft.card.adaptive";

    /// <summary>
    /// Gets the content URL.
    /// </summary>
    [JsonProperty("contentUrl")]
    public string ContentUrl { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    [JsonProperty("content")]
    [JsonConverter(typeof(AdaptiveCards.AdaptiveCardConverter))]
    public AdaptiveCards.AdaptiveCard? Content { get; set; }
}
