using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.Firebase
{
    /// <summary>
    /// Represents settings of the Firebase authentication method
    /// </summary>
    public class FirebaseExternalAuthSettings : ISettings
    {
        /// <summary>
        /// Gets or sets Firebase project API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets Firebase project ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets Firebase Auth Domain
        /// </summary>
        public string AuthDomain { get; set; }
    }
}
