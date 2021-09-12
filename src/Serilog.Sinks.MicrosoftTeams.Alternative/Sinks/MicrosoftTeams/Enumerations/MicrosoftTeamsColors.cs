// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsColors.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A class to store all the Microsoft Teams colors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Enumerations
{
    /// <summary>
    /// A class to store all the Microsoft Teams colors.
    /// </summary>
    public static class MicrosoftTeamsColors
    {
        /// <summary>
        /// The default Microsoft Teams color.
        /// </summary>
        public static string Default => "777777";

        /// <summary>
        /// The information Microsoft Teams color.
        /// </summary>
        public static string Information => "5bc0de";

        /// <summary>
        /// The warning Microsoft Teams color.
        /// </summary>
        public static string Warning => "f0ad4e";

        /// <summary>
        /// The error Microsoft Teams color.
        /// </summary>
        public static string Error => "d9534f";
    }
}