// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageCard.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The teams message card.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Utf8Json.Core;

/// <summary>
/// The teams message card.
/// </summary>
internal class MicrosoftTeamsMessageCard
{
    /// <summary>
    /// Gets the type of the card.
    /// </summary>
    [DataMember(Name = "@type")]
    public string Type { get; } = "MessageCard";

    /// <summary>
    /// Gets the context of the card.
    /// </summary>
    [DataMember(Name = "@context")]
    public string Context { get; } = "http://schema.org/extensions";

    /// <summary>
    /// Gets or sets the title of the card.
    /// </summary>
    [DataMember(Name = "title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text of the card.
    /// </summary>
    [DataMember(Name = "text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the theme color of the card.
    /// </summary>
    [DataMember(Name = "themeColor")]
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sections of the card.
    /// </summary>
    [DataMember(Name = "sections")]
    public IList<MicrosoftTeamsMessageSection> Sections { get; set; } = new List<MicrosoftTeamsMessageSection>();

    /// <summary>
    /// Gets or sets the potential action buttons.
    /// </summary>
    [DataMember(Name = "potentialAction")]
    public IList<MicrosoftTeamsMessageAction> PotentialActions { get; set; } = new List<MicrosoftTeamsMessageAction>();
}
