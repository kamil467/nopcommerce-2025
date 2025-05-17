using System;
using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.Google;

public class GoogleExternalAuthSettings: ISettings
{
  public string CliendId { get; set;}

  public string SecretKey { get;set; }

}
