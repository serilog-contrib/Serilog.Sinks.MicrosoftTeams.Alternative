// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftExtendedLogEvent.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Added a new class to store the first and last occurrence timestamps.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative;

/// <summary>
/// Added a new class to store the first and last occurrence timestamps.
/// </summary>
public class MicrosoftExtendedLogEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftExtendedLogEvent"/> class.
    /// The occurrences are interpreted as local time, use the
    /// <see cref="DateTimeOffset"/> overload to keep the offset of the log event.
    /// </summary>
    /// <param name="firstOccurrence">The first occurrence.</param>
    /// <param name="lastOccurrence">The last occurrence.</param>
    /// <param name="logEvent">The log event.</param>
    public MicrosoftExtendedLogEvent(DateTime firstOccurrence, DateTime lastOccurrence, LogEvent logEvent)
    {
        this.FirstOccurrence = firstOccurrence;
        this.LastOccurrence = lastOccurrence;
        this.LogEvent = logEvent;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftExtendedLogEvent"/> class.
    /// </summary>
    /// <param name="firstOccurrence">The first occurrence.</param>
    /// <param name="lastOccurrence">The last occurrence.</param>
    /// <param name="logEvent">The log event.</param>
    public MicrosoftExtendedLogEvent(DateTimeOffset firstOccurrence, DateTimeOffset lastOccurrence, LogEvent logEvent)
    {
        this.FirstOccurrence = firstOccurrence;
        this.LastOccurrence = lastOccurrence;
        this.LogEvent = logEvent;
    }

    /// <summary>
    /// Gets or sets the log event.
    /// </summary>
    public LogEvent LogEvent { get; set; }

    /// <summary>
    /// Gets or sets the first occurrence.
    /// </summary>
    public DateTimeOffset FirstOccurrence { get; set; }

    /// <summary>
    /// Gets or sets the last occurrence.
    /// </summary>
    public DateTimeOffset LastOccurrence { get; set; }
}
