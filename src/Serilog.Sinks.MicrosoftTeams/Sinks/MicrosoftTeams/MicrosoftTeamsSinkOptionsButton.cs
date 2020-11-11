namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// 
    /// </summary>
    public class MicrosoftTeamsSinkOptionsButton
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public MicrosoftTeamsSinkOptionsButton() { }

        /// <summary>
        /// Convenience ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        public MicrosoftTeamsSinkOptionsButton(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        /// <summary>
        /// Link display name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Link Uri
        /// </summary>
        public string Uri { get; set; }
    }
}
