using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.ExternalAuth.Google.Models;

/// <summary>
/// Represents plugin configuration model
/// </summary>
public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.ExternalAuth.Google.ClientKeyIdentifier")]
    public string ClientId { get; set; }

    [NopResourceDisplayName("Plugins.ExternalAuth.Google.ClientSecret")]
    public string ClientSecret { get; set; }
}