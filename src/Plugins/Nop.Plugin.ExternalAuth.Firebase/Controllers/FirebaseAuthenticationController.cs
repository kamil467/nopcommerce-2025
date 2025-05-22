using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.ExternalAuth.Firebase.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using static Nop.Services.Security.StandardPermission;

namespace Nop.Plugin.ExternalAuth.Firebase.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FirebaseAuthenticationController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly FirebaseExternalAuthSettings _firebaseExternalAuthSettings;

        #endregion

        #region Ctor

        public FirebaseAuthenticationController(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            FirebaseExternalAuthSettings firebaseExternalAuthSettings)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _firebaseExternalAuthSettings = firebaseExternalAuthSettings;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(Configuration.MANAGE_EXTERNAL_AUTHENTICATION_METHODS))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ApiKey = _firebaseExternalAuthSettings.ApiKey,
                ProjectId = _firebaseExternalAuthSettings.ProjectId,
                AuthDomain = _firebaseExternalAuthSettings.AuthDomain
            };

            return View("~/Plugins/ExternalAuth.Firebase/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(Configuration.MANAGE_EXTERNAL_AUTHENTICATION_METHODS))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            _firebaseExternalAuthSettings.ApiKey = model.ApiKey;
            _firebaseExternalAuthSettings.ProjectId = model.ProjectId;
            _firebaseExternalAuthSettings.AuthDomain = model.AuthDomain;
            await _settingService.SaveSettingAsync(_firebaseExternalAuthSettings);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        public async Task<IActionResult> SignInCallback(string idToken)
        {
            // Implement Firebase token validation and user authentication here
            // This will be called after successful Firebase authentication
            return RedirectToRoute("Homepage");
        }

        #endregion
    }
}
