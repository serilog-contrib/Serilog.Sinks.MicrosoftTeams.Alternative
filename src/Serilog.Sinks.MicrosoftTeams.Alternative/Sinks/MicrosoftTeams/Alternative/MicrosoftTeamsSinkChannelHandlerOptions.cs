// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkOptions.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Container for the multiple channel configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative;

/// <summary>
/// Container for the multiple channel configuration
/// </summary>
public class MicrosoftTeamsSinkChannelHandlerOptions
{
    /// <summary>
    ///     Only emit events that have this property (Ignoring it's value)
    /// </summary>
    public string? FilterOnProperty { get; }

    /// <summary>
    ///     Mapping between a requested channel (set in <see cref="FilterOnProperty"/>) and it's api endpoint
    /// </summary>
    /// <example>
    ///     If <see cref="FilterOnProperty"/> is set then the value of that property is
    ///     searched on this dictionary to find the matching channel endpoint, if no matching key is
    ///     found here, it will send the event to the default endpoint for this sink.
    /// </example>
    public Dictionary<string, string> ChannelList { get; }

    /// <summary>
    /// Checks if the sink is configured to only emit events that have the property set in <see cref="FilterOnProperty"/>
    /// </summary>
    public bool GetIsFilterOnPropertyEnabled =>
        !string.IsNullOrWhiteSpace(this.FilterOnProperty);

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkChannelHandlerOptions"/> class.
    /// </summary>
    /// <param name="filterOnProperty">Only emit events that have this property (Ignoring it's value)</param>
    /// <param name="channelList">Mapping between a requested channel (set in <see cref="FilterOnProperty"/>) and it's api endpoint</param>
    public MicrosoftTeamsSinkChannelHandlerOptions(string? filterOnProperty = null, Dictionary<string, string>? channelList = null)
    {
        this.FilterOnProperty = filterOnProperty;
        this.ChannelList = channelList ?? new Dictionary<string, string>();
    }
}