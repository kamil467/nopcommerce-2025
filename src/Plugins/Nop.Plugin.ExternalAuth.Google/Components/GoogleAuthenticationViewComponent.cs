using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.ExternalAuth.Google.Components;

public class GoogleAuthenticationViewComponent:NopViewComponent
{
    /// <summary>
    /// Invoke view component
    /// </summary>
    /// <param name="widgetZone">Widget zone name</param>
    /// <param name="additionalData">Additional data</param>
    /// <returns>View component result</returns>
    public IViewComponentResult Invoke(string widgetZone, object additionalData)
    {
        return View("~/Plugins/ExternalAuth.Google/Views/PublicInfo.cshtml");
    }
}
