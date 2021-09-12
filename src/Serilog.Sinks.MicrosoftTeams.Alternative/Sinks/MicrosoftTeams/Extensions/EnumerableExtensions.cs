// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   This class contains <see cref="IEnumerable{T}"/> extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class contains <see cref="IEnumerable{T}"/> extension methods.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Checks whether the enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of object to use.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/>.</param>
        /// <returns>A value indicating whether the enumerable is null or empty.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            return !enumerable.Any();
        }
    }
}
