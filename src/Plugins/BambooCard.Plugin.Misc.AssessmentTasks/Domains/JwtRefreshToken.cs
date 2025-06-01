using Nop.Core;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Domains;

public class JwtRefreshToken : BaseEntity
{
    public Guid Token { get; set; }

    public int CustomerId { get; set; }

    public DateTime ValidTill { get; set; }

    public bool Active { get; set; }
}
