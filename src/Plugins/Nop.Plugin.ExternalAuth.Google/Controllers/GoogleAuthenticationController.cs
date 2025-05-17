using System;
using System.IdentityModel.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Plugin.ExternalAuth.Google.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.Google.Controllers;

[AutoValidateAntiforgeryToken]
public class GoogleAuthenticationController: BasePluginController
{
#region Fields

    protected readonly GoogleExternalAuthSettings _googleExternalAuthSettings;
    protected readonly IAuthenticationPluginManager _authenticationPluginManager;
    protected readonly IExternalAuthenticationService _externalAuthenticationService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IOptionsMonitorCache<GoogleOptions> _optionsCache;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public GoogleAuthenticationController(GoogleExternalAuthSettings facebookExternalAuthSettings,
        IAuthenticationPluginManager authenticationPluginManager,
        IExternalAuthenticationService externalAuthenticationService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IOptionsMonitorCache<GoogleOptions> optionsCache,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext,
        IWorkContext workContext)
    {
        _googleExternalAuthSettings = facebookExternalAuthSettings;
        _authenticationPluginManager = authenticationPluginManager;
        _externalAuthenticationService = externalAuthenticationService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _optionsCache = optionsCache;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [CheckPermission(StandardPermission.Configuration.MANAGE_EXTERNAL_AUTHENTICATION_METHODS)]
    public IActionResult Configure()
    {
        var model = new ConfigurationModel
        {
            ClientId = _googleExternalAuthSettings.CliendId,
            ClientSecret = _googleExternalAuthSettings.SecretKey
        };

        return View("~/Plugins/ExternalAuth.Facebook/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [CheckPermission(StandardPermission.Configuration.MANAGE_EXTERNAL_AUTHENTICATION_METHODS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return Configure();

        //save settings
        _googleExternalAuthSettings.CliendId = model.ClientId;
        _googleExternalAuthSettings.SecretKey = model.ClientSecret;
        await _settingService.SaveSettingAsync(_googleExternalAuthSettings);

        //clear Facebook authentication options cache
        _optionsCache.TryRemove(GoogleDefaults.AuthenticationScheme);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return Configure();
    }

    public async Task<IActionResult> Login(string returnUrl)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var methodIsAvailable = await _authenticationPluginManager
            .IsPluginActiveAsync(GoogleAuthenticationDefaults.SystemName, await _workContext.GetCurrentCustomerAsync(), store.Id);
        if (!methodIsAvailable)
            throw new NopException("Facebook authentication module cannot be loaded");

        if (string.IsNullOrEmpty(_googleExternalAuthSettings.CliendId) ||
            string.IsNullOrEmpty(_googleExternalAuthSettings.SecretKey))
        {
            throw new NopException("Facebook authentication module not configured");
        }

        //configure login callback action
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("LoginCallback", "GoogleAuthentication", new { returnUrl = returnUrl })
        };
        authenticationProperties.SetString(GoogleAuthenticationDefaults.ErrorCallback, Url.RouteUrl("Login", new { returnUrl }));

        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> LoginCallback(string returnUrl)
    {
        //authenticate Facebook user
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded || !authenticateResult.Principal.Claims.Any())
            return RedirectToRoute("Login");

        //create external authentication parameters
        var authenticationParameters = new ExternalAuthenticationParameters
        {
            ProviderSystemName = GoogleAuthenticationDefaults.SystemName,
            AccessToken = await HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, "access_token"),
            Email = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value,
            ExternalIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
            ExternalDisplayIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value,
            Claims = authenticateResult.Principal.Claims.Select(claim => new ExternalAuthenticationClaim(claim.Type, claim.Value)).ToList()
        };

        //authenticate Nop user
        return await _externalAuthenticationService.AuthenticateAsync(authenticationParameters, returnUrl);
    }

    public async Task<IActionResult> DataDeletionStatusCheck(int earId)
    {
        var externalAuthenticationRecord = await _externalAuthenticationService.GetExternalAuthenticationRecordByIdAsync(earId);
        if (externalAuthenticationRecord is not null)
            _notificationService.WarningNotification(await _localizationService.GetResourceAsync("Plugins.ExternalAuth.Facebook.AuthenticationDataExist"));
        else
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.ExternalAuth.Facebook.AuthenticationDataDeletedSuccessfully"));

        return RedirectToRoute("CustomerInfo");
    }

    #endregion
}
