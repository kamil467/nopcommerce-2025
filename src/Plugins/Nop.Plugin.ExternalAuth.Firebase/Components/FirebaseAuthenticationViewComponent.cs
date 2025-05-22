using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.ExternalAuth.Firebase.Components
{
    /// <summary>
    /// Represents view component to display Firebase authentication button
    /// </summary>
    public class FirebaseAuthenticationViewComponent : NopViewComponent
    {
        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/ExternalAuth.Firebase/Views/PublicInfo.cshtml");
        }
    }
}
