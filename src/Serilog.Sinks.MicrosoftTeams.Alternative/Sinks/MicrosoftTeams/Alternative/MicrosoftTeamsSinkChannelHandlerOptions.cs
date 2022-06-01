// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkChannelHandlerOptions.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A container for the multiple channel configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative;

/// <summary>
/// A container for the multiple channel configuration.
/// </summary>
public class MicrosoftTeamsSinkChannelHandlerOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkChannelHandlerOptions"/> class.
    /// </summary>
    public MicrosoftTeamsSinkChannelHandlerOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkChannelHandlerOptions"/> class.
    /// </summary>
    /// <param name="filterOnProperty">Only emit events that have this property (Ignoring it's value).</param>
    /// <param name="channelList">Mapping between a requested channel (set in <see cref="FilterOnProperty"/>) and it's API endpoint.</param>
    public MicrosoftTeamsSinkChannelHandlerOptions(string? filterOnProperty = null, Dictionary<string, string>? channelList = null)
    {
        this.FilterOnProperty = filterOnProperty;
        this.ChannelList = channelList ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Gets or sets the filter to filter for, only emit events that have this property (Ignoring its value).
    /// </summary>
    public string? FilterOnProperty { get; set; }

    /// <summary>
    /// Gets or sets the mapping between a requested channel (set in <see cref="FilterOnProperty"/>) and it's API endpoint.
    /// </summary>
    /// <example>
    /// If <see cref="FilterOnProperty"/> is set then the value of that property is
    /// searched on this dictionary to find the matching channel endpoint, if no matching key is
    /// found here, it will send the event to the default endpoint for this sink.
    /// </example>
    public Dictionary<string, string> ChannelList { get; set; } = new();

    /// <summary>
    /// Gets a value indicating whether the sink is configured to only emit events that have the property set in <see cref="FilterOnProperty"/> or not.
    /// </summary>
    public bool IsFilterOnPropertyEnabled => !string.IsNullOrWhiteSpace(this.FilterOnProperty);
}