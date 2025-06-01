namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Common;

public class BCBaseQueryModel<TModel>
{
    public BCBaseQueryModel()
    {
        Type t = typeof(TModel);
        if (t.GetConstructor(Type.EmptyTypes) != null)
            Data = Activator.CreateInstance<TModel>();

        FormValues = new List<BCKeyValueApi>();
        UploadPicture = new BCPictureQueryModel();
    }

    public TModel Data { get; set; }
    public List<BCKeyValueApi> FormValues { get; set; }
    public BCPictureQueryModel UploadPicture { get; set; }
}
