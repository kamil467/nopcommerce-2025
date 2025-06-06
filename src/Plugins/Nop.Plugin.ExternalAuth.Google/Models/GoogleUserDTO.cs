using System;
using Newtonsoft.Json;

namespace Nop.Plugin.ExternalAuth.Google.Models;

public class GoogleUserDTO
{
   [JsonProperty("algorithm")]
    public string Algorithm { get; set; }

    [JsonProperty("expires")]
    public int Expires { get; set; }

    [JsonProperty("issued_at")]
    public int IssuedAt { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }
}
