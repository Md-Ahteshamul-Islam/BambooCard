using Nop.Web.Models.Customer;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Customers;

public class LogInResponseModel
{
    public bool MultiFactorAuthenticationRequired { get; set; }

    public string Token { get; set; }

    public string JwtRefreshToken { get; set; }
}
