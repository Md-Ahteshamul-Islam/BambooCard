using BambooCard.Misc.AssessmentTasks.Infrastructure;
using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Controllers;
using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
using BambooCard.Plugin.Misc.AssessmentTasks.Infrastructure.Extensions;
using BambooCard.Plugin.Misc.AssessmentTasks.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using AdminContrllers = Nop.Web.Areas.Admin.Controllers;
//using Microsoft.OpenApi.Models;
using BambooCard.Plugin.Misc.AssessmentTasks.Factories;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Infrastructure
{
    public class PluginNopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddEndpointsApiExplorer();

            #region Admin

            services.AddScoped<IConfigurationModelFactory, ConfigurationModelFactory>();
            services.AddScoped<IOrderAPIModelFactory, OrderAPIModelFactory>();
            services.AddScoped<IProductAttributeCustomModelFactory, ProductAttributeCustomModelFactory>();
            services.AddScoped<IProductAttributeCustomService, ProductAttributeCustomService>();
            services.AddScoped<IJwtRefreshTokenService, JwtRefreshTokenService>();
            services.AddScoped<ICustomerApiService, CustomerApiService>();
            services.AddScoped<AdminContrllers.ProductAttributeController, OverrideProductAttributeController>();

            #endregion
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseApiExceptionHandler();
            application.UseMiddleware<JwtAuthMiddleware>();

            // Resolve IConfiguration manually
            var configuration = EngineContext.Current.Resolve<IConfiguration>();
            var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();

        }

        public int Order => int.MaxValue;
    }
}