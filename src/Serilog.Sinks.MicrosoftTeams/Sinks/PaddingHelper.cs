namespace Serilog.Sinks
{
    using System.IO;
    using System.Linq;

    using Serilog.Parsing;

    public static class PaddingHelper
    {
        static readonly char[] PaddingChars = Enumerable.Repeat(' ', 80).ToArray();

        /// <summary>
        /// Writes the provided value to the output, applying direction-based padding when <paramref name="alignment"/> is provided.
        /// </summary>
        public static void Apply(TextWriter output, string value, Alignment? alignment)
        {
            if (alignment == null || value.Length >= alignment.Value.Width)
            {
                output.Write(value);
                return;
            }

            var pad = alignment.Value.Width - value.Length;

            if (alignment.Value.Direction == AlignmentDirection.Left)
            {
                output.Write(value);
            }

            if (pad <= PaddingChars.Length)
            {
                output.Write(PaddingChars, 0, pad);
            }
            else
            {
                output.Write(new string(' ', pad));
            }

            if (alignment.Value.Direction == AlignmentDirection.Right)
            {
                output.Write(value);
            }
        }
    }
}
