// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsMessageSection.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The message section.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Core
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The message section.
    /// </summary>
    internal class MicrosoftTeamsMessageSection
    {
        /// <summary>
        /// Gets or sets message section title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets message section facts.
        /// </summary>
        [DataMember(Name = "facts")]
        public IList<MicrosoftTeamsMessageFact> Facts { get; set; }
    }
}