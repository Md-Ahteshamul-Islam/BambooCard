namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Common;

public class BCBaseResponseModel
{
    public BCBaseResponseModel()
    {
        ErrorList = new List<string>();
    }

    public string Message { get; set; }

    public List<string> ErrorList { get; set; } = [];
}
