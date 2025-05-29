using BambooCard.Misc.AssessmentTasks.Infrastructure;
using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;

namespace NopStation.Plugin.Theme.GeneratorShop.Infrastructure
{
    public class PluginNopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            #region Admin

            services.AddScoped<IConfigurationModelFactory, ConfigurationModelFactory>();

            #endregion
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => int.MaxValue;
    }
}