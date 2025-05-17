﻿using Nop.Core;
using Nop.Plugin.ExternalAuth.Google.Components;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
namespace Nop.Plugin.ExternalAuth.Google;

public class GoogleAuth : BasePlugin, IExternalAuthenticationMethod
{
      #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly ISettingService _settingService;
    protected readonly IWebHelper _webHelper;

    #endregion

public GoogleAuth(ILocalizationService localizationService,
ISettingService settingService,
IWebHelper webHelper)
{
    _localizationService = localizationService;
    _settingService = settingService;
    _webHelper = webHelper;
}

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/GoogleAuthentication/Configure";
    }
    public Type GetPublicViewComponent()
    {
     return typeof(GoogleAuthenticationViewComponent);
    }

    public override async Task InstallAsync()
    {
            //settings
        await _settingService.SaveSettingAsync(new GoogleExternalAuthSettings());

        //locales
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.ExternalAuth.Google.AuthenticationDataDeletedSuccessfully"] = "Data deletion request completed",
            ["Plugins.ExternalAuth.Google.AuthenticationDataExist"] = "Data deletion request is pending, please contact the admin",
            ["Plugins.ExternalAuth.Google.ClientKeyIdentifier"] = "App ID/API Key",
            ["Plugins.ExternalAuth.Google.ClientKeyIdentifier.Hint"] = "Enter your app ID/API key here. You can find it on your google developer console.",
            ["Plugins.ExternalAuth.Google.ClientSecret"] = "App Secret",
            ["Plugins.ExternalAuth.Google.ClientSecret.Hint"] = "Enter your app secret here. You can find it on your google developer console page.",
            ["Plugins.ExternalAuth.Google.Instructions"] = "<p>To configure authentication with google console page, please follow these steps:<br/><br/><ol><li>Navigate to the <a href=\"https://developers.facebook.com/apps\" target =\"_blank\"> Facebook for Developers</a> page and sign in. If you don't already have a Facebook account, use the <b>Sign up for Facebook</b> link on the login page to create one.</li><li>Tap the <b>+ Add a New App button</b> in the upper right corner to create a new App ID. (If this is your first app with Facebook, the text of the button will be <b>Create a New App</b>.)</li><li>Fill out the form and tap the <b>Create App ID button</b>.</li><li>The <b>Product Setup</b> page is displayed, letting you select the features for your new app. Click <b>Get Started</b> on <b>Facebook Login</b>.</li><li>Click the <b>Settings</b> link in the menu at the left, you are presented with the <b>Client OAuth Settings</b> page with some defaults already set.</li><li>Enter \"{0:s}signin-facebook\" into the <b>Valid OAuth Redirect URIs</b> field.</li><li>From User data deletion dropdown menu select \"Data deletion instructions URL\" </li><li> Enter \"{0:s}facebook/data-deletion-callback/\" into the <b> User data deletion </b> input field.</li><li>Click <b>Save Changes</b>.</li><li>Click the <b>Dashboard</b> link in the left navigation.</li><li>Copy your App ID and App secret below.</li></ol><br/><br/></p>"
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
        await _settingService.DeleteSettingAsync<GoogleExternalAuthSettings>();

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.ExternalAuth.Google");

        await base.UninstallAsync();
    }
    
}
