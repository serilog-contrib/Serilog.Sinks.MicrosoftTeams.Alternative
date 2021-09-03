namespace Serilog.Sinks
{
    using System;
    using System.IO;

    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Parsing;
    using Serilog.Rendering;

    /// Based on https://github.com/serilog/serilog/blob/dev/src/Serilog/Formatting/Display/MessageTemplateTextFormatter.cs.
    /// <inheritdoc cref="ITextFormatter"/>
    /// <summary>
    /// A <see cref="ITextFormatter"/> that supports to format the Serilog message template format, but only for simple title messages.
    /// </summary>
    /// <seealso cref="ITextFormatter"/>
    public class SimpleTitleFormatter : ITextFormatter
    {
        /// <summary>
        /// The format provider.
        /// </summary>
        private readonly IFormatProvider? formatProvider;

        /// <summary>
        /// The title template.
        /// </summary>
        private readonly MessageTemplate titleTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTitleFormatter"/> class.
        /// </summary>
        /// <param name="titleTemplate">A message template describing the title messages.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="titleTemplate"/> is <code>null</code></exception>
        public SimpleTitleFormatter(string titleTemplate, IFormatProvider? formatProvider = null)
        {
            if (titleTemplate == null)
            {
                throw new ArgumentNullException(nameof(titleTemplate));
            }

            this.titleTemplate = new MessageTemplateParser().Parse(titleTemplate);
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Format the log event into the output.
        /// </summary>
        /// <param name="logEvent">The event to format.</param>
        /// <param name="output">The output.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="logEvent"/> is <code>null</code></exception>
        /// <exception cref="ArgumentNullException">When <paramref name="output"/> is <code>null</code></exception>
        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            foreach (var token in this.titleTemplate.Tokens)
            {
                if (token is TextToken tt)
                {
                    output.Write(tt.Text);
                }
            }
        }
    }
}
