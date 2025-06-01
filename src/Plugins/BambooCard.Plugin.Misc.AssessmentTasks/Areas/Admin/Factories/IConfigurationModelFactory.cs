using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
public interface IConfigurationModelFactory
{
    Task<ConfigurationModel> PrepareConfigurationModelAsync();
}