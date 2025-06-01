using Nop.Core.Configuration;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Settings;
public class BCAPISettings : ISettings
{
    public string SecretKey { get; set; }

    public int TokenValidityInMinute { get; set; }

    public int JwtRefreshTokenLifetimeInHours { get; set; }
}
