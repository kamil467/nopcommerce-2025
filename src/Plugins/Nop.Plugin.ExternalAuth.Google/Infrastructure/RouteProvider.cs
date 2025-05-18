using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Plugin.ExternalAuth.Google;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.ExternalAuth.Google.Infrastructure;

/// <summary>
/// Represents plugin route provider
/// </summary>
public class RouteProvider : IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(GoogleAuthenticationDefaults.DataDeletionCallbackRoute, $"google/data-deletion-callback/",
            new { controller = "GoogleDataDeletion", action = "DataDeletionCallback" });

        endpointRouteBuilder.MapControllerRoute(GoogleAuthenticationDefaults.DataDeletionStatusCheckRoute, $"google/data-deletion-status-check/{{earId:min(0)}}",
            new { controller = "GoogleAuthentication", action = "DataDeletionStatusCheck" });
    }

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}