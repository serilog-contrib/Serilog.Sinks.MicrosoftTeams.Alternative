// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingException.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The logging exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Exceptions;

/// <inheritdoc cref="LoggingFailedException"/>
/// <summary>
/// The logging exception.
/// </summary>
public sealed class LoggingException : LoggingFailedException
{
    /// <summary>
    /// Gets or sets the status code.
    /// </summary>
    public HttpStatusCode? StatusCode { get; set; }

    /// <summary>
    /// Construct a <see cref="LoggingException"/> to communicate a logging failure.
    /// </summary>
    /// <param name="message">A message describing the logging failure.</param>
    public LoggingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Construct a <see cref="LoggingException"/> to communicate a logging failure.
    /// </summary>
    /// <param name="message">A message describing the logging failure.</param>
    /// <param name="statusCode">The status code.</param>
    public LoggingException(string message, HttpStatusCode statusCode) : base(message)
    {
        this.StatusCode = statusCode;
    }
}
