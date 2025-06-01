using BambooCard.Plugin.Misc.AssessmentTasks.Extensions;
using BambooCard.Plugin.Misc.AssessmentTasks.Models.Common;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;
using Nop.Services.Logging;

namespace BambooCard.Plugin.Misc.AssessmentTasks;

public class TokenAuthorizeAttribute : TypeFilterAttribute
{
    #region Ctor

    public TokenAuthorizeAttribute(bool ignore = false) : base(typeof(TokenAuthorizeAttributeFilter))
    {
        IgnoreFilter = ignore;
        Arguments = new object[] { ignore };
    }

    public bool IgnoreFilter { get; }

    #endregion

    #region Nested class

    public class TokenAuthorizeAttributeFilter : IAuthorizationFilter //IAuthorizationFilter
    {
        private readonly bool _ignoreFilter;

        public TokenAuthorizeAttributeFilter(bool ignoreFilter)
        {
            _ignoreFilter = ignoreFilter;
        }

        public void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            var identity = (ParseAuthorizationHeaderAsync(actionContext)).Result;
            if (identity == false)
            {
                Challenge(actionContext);
                return;
            }
        }

        protected async virtual Task<bool> ParseAuthorizationHeaderAsync(AuthorizationFilterContext actionContext)
        {
            //check whether this filter has been overridden for the action
            var actionFilter = actionContext.ActionDescriptor.FilterDescriptors
                .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                .Select(filterDescriptor => filterDescriptor.Filter)
                .OfType<TokenAuthorizeAttribute>()
                .FirstOrDefault();
            //ignore filter (the action is available even if a customer hasn't access to the admin area)
            if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                return (true);

            bool isValid = false;

            if (actionContext.HttpContext.Request.Headers.TryGetValue(AssessmentTasksDefaults.Token, out StringValues checkToken))
            {
                var token = checkToken.FirstOrDefault();
                var webApiSettings = EngineContext.Current.Resolve<BCAPISettings>();
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var customerService = EngineContext.Current.Resolve<ICustomerService>();
                try
                {
                    var payload = JwtHelper.JwtDecoder.DecodeToObject(token, webApiSettings.SecretKey, true);

                    if (payload.TryGetValue("BC-CustomerId", out var customerIdObj) &&
                        int.TryParse(customerIdObj?.ToString(), out int customerId))
                    {
                        var customer = await customerService.GetCustomerByIdAsync(customerId);
                        if (customer != null)
                        {
                            await workContext.SetCurrentCustomerAsync(customer);
                        }
                    }

                    isValid = true;
                }
                catch(Exception ex)
                {
                    await logger.ErrorAsync(ex.Message, ex);
                    isValid = false;
                }
            }

            return isValid;
        }

        private void Challenge(AuthorizationFilterContext actionContext, string error = "")
        {
            var response = new BCBaseResponseModel
            {
                ErrorList = new List<string>
                {
                    error
                }
            };

            actionContext.Result = new ObjectResult(response)
            {
                StatusCode = 403
            };
            return;
        }
    }

    #endregion
}