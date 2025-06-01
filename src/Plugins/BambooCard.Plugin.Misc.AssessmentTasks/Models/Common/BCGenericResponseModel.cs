namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Common;

public class BCGenericResponseModel<TResult> : BCBaseResponseModel
{
    public BCGenericResponseModel()
    {
        Type t = typeof(TResult);
        if (t.GetConstructor(Type.EmptyTypes) != null)
            Data = Activator.CreateInstance<TResult>();
    }

    public TResult Data { get; set; }
}
