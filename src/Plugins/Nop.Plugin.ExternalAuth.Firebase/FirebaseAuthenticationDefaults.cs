namespace Nop.Plugin.ExternalAuth.Firebase
{
    /// <summary>
    /// Default values used by the Firebase authentication middleware
    /// </summary>
    public static class FirebaseAuthenticationDefaults
    {
        /// <summary>
        /// System name of the external authentication method
        /// </summary>
        public static string SystemName => "ExternalAuth.Firebase";

        /// <summary>
        /// Name of the view component to display plugin in public store
        /// </summary>
        public const string VIEW_COMPONENT_NAME = "FirebaseAuthentication";

        /// <summary>
        /// Name of the Firebase authentication scheme
        /// </summary>
        public static string AuthenticationScheme => "Firebase";
    }
}
