using BambooCard.Plugin.Misc.AssessmentTasks.Domains;
using Nop.Data.Mapping;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Data;
public class BaseNameCompatibility : INameCompatibility
{
    #region Properties

    public Dictionary<Type, string> TableNames => new()
    {
        
        { typeof(JwtRefreshToken), "BC_JwtRefreshToken" }
    };

    public Dictionary<(Type, string), string> ColumnName => new()
    {
    };

    #endregion
}
