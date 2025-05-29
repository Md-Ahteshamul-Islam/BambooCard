using AutoMapper;
using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using Nop.Core.Infrastructure.Mapper;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Infrastructure.Mapper;

/// <summary>
/// Represents AutoMapper configuration for plugin models
/// </summary>
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public MapperConfiguration()
    {
        CreateMap<BambooCardDiscountSettings, BambooCardDiscountSettingsModel>()
            .ReverseMap();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 1;

    #endregion
}