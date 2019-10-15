using Serilog.Events;
using System;

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// Added a new class to store the first and last occurence timestamps.
    /// </summary>
    public class ExtendedLogEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLogEvent"/> class.
        /// </summary>
        public ExtendedLogEvent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLogEvent"/> class.
        /// </summary>
        /// <param name="firstOccurence">The first occurence.</param>
        /// <param name="lastOccurence">The last occurence.</param>
        /// <param name="logEvent">The log event.</param>
        public ExtendedLogEvent(DateTime firstOccurence, DateTime lastOccurence, LogEvent logEvent)
        {
            FirstOccurence = firstOccurence;
            LastOccurence = lastOccurence;
            LogEvent = logEvent;
        }

        /// <summary>
        /// Gets or sets the log event.
        /// </summary>
        public LogEvent LogEvent { get; set; }

        /// <summary>
        /// Gets or sets the first occurence.
        /// </summary>
        public DateTimeOffset FirstOccurence { get; set; }

        /// <summary>
        /// Gets or sets the last occurence.
        /// </summary>
        public DateTimeOffset LastOccurence { get; set; }
    }
}