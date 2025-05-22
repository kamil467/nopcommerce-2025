using Nop.Core;
using Nop.Plugin.ExternalAuth.Firebase.Components;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.Firebase
{
    /// <summary>
    /// Represents method for the authentication with Firebase
    /// </summary>
    public class FirebaseAuthenticationMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public FirebaseAuthenticationMethod(
            ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/FirebaseAuthentication/Configure";
        }

        /// <summary>
        /// Gets a type of a view component for displaying plugin in public store
        /// </summary>
        /// <returns>View component type</returns>
        public Type GetPublicViewComponent()
        {
            return typeof(FirebaseAuthenticationViewComponent);
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //settings
            await _settingService.SaveSettingAsync(new FirebaseExternalAuthSettings());

            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.ExternalAuth.Firebase.ApiKey"] = "Firebase API Key",
                ["Plugins.ExternalAuth.Firebase.ApiKey.Hint"] = "Enter your Firebase project API key",
                ["Plugins.ExternalAuth.Firebase.ProjectId"] = "Firebase Project ID",
                ["Plugins.ExternalAuth.Firebase.ProjectId.Hint"] = "Enter your Firebase project ID",
                ["Plugins.ExternalAuth.Firebase.AuthDomain"] = "Firebase Auth Domain",
                ["Plugins.ExternalAuth.Firebase.AuthDomain.Hint"] = "Enter your Firebase authentication domain",
                ["Plugins.ExternalAuth.Firebase.Instructions"] = "<p>To configure Firebase authentication, follow these steps:<br/><br/><ol>" +
                    "<li>Go to the <a href=\"https://console.firebase.google.com/\" target=\"_blank\">Firebase Console</a></li>" +
                    "<li>Create a new project or select an existing one</li>" +
                    "<li>In the project settings, find your Web API Key, Project ID, and Auth Domain</li>" +
                    "<li>Enable the authentication methods you want to use (Email/Password, Google, Facebook, etc.)</li>" +
                    "<li>Add your domain to the authorized domains list</li>" +
                    "<li>Copy the configuration values below</li></ol><br/><br/></p>"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<FirebaseExternalAuthSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.ExternalAuth.Firebase");

            await base.UninstallAsync();
        }

        #endregion
    }
}
