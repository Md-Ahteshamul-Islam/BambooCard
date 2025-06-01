using Nop.Core.Domain.Customers;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;
public interface ICustomerApiService
{
    Task<string> GenerateRefreshTokenAsync(Customer customer, bool isPersistent = false);
    Task<string> GetTokenAsync(Customer customer);
}