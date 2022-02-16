// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageSection.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The message section.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Core;

/// <summary>
/// The message section.
/// </summary>
internal class MicrosoftTeamsMessageSection
{
    /// <summary>
    /// Gets or sets message section title.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets message section facts.
    /// </summary>
    [JsonProperty("facts")]
    public IList<MicrosoftTeamsMessageFact> Facts { get; set; } = new List<MicrosoftTeamsMessageFact>();
}
