using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.ExternalAuth.Firebase.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.ExternalAuth.Firebase.ApiKey")]
        public string ApiKey { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.Firebase.ProjectId")]
        public string ProjectId { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.Firebase.AuthDomain")]
        public string AuthDomain { get; set; }
    }
}
