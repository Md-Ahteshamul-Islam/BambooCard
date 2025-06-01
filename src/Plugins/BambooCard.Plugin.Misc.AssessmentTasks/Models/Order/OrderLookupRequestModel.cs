using Newtonsoft.Json;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Order;
public record OrderLookupRequestModel
{
    [JsonProperty("email")]
    public string Email { get; set; }
}
