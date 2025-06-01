using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Logging;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{

    public static void UseApiExceptionHandler(this IApplicationBuilder application)
    {
        var appSettings = EngineContext.Current.Resolve<AppSettings>();
        var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();
        var useDetailedExceptionPage = appSettings.Get<CommonConfig>().DisplayFullErrorStack || webHostEnvironment.IsDevelopment();
        if (useDetailedExceptionPage)
        {
            //get detailed exceptions for developing and testing purposes
            application.UseDeveloperExceptionPage();
        }
        else
        {
            //or use special exception handler
            application.UseExceptionHandler("/Error/Error");
        }

        //log errors
        application.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception == null)
                    return;

                try
                {
                    //check whether database is installed
                    if (DataSettingsManager.IsDatabaseInstalled())
                    {
                        //get current customer
                        var workContext = EngineContext.Current.Resolve<IWorkContext>();
                        var currentCustomer = workContext.GetCurrentCustomerAsync().Result;

                        //log error
                        var logger = EngineContext.Current.Resolve<ILogger>();
                        await logger.ErrorAsync(exception.Message, exception, currentCustomer);
                    }
                }
                finally
                {
                    //rethrow the exception to show the error page
                    ExceptionDispatchInfo.Throw(exception);
                }
            });
        });
    }
}
