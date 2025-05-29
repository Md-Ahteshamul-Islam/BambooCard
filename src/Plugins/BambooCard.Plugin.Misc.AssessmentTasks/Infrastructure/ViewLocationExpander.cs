using Microsoft.AspNetCore.Mvc.Razor;

namespace BambooCard.Misc.AssessmentTasks.Infrastructure;
public class ViewLocationExpander : IViewLocationExpander
{
    protected const string THEME_KEY = "nop.themename";
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }


    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.AreaName == "Admin")
        {
            viewLocations = new string[]
            {
                $"/Plugins/BambooCard.Misc.AssessmentTasks/Areas/Admin/Views/Shared/{{0}}.cshtml",
                $"/Plugins/BambooCard.Misc.AssessmentTasks/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);
        }

        else
        {
            viewLocations = new string[]
            {
                $"~/Plugins/BambooCard.Misc.AssessmentTasks/Views/Shared/{{0}}.cshtml",
                $"~/Plugins/BambooCard.Misc.AssessmentTasks/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);

            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                viewLocations = new string[]
                {
                    $"/Plugins/BambooCard.Misc.AssessmentTasks/Themes/{theme}/Views/Shared/{{0}}.cshtml",

                    $"/Plugins/BambooCard.Misc.AssessmentTasks/Themes/{theme}/Views/{{1}}/{{0}}.cshtml"
                }.Concat(viewLocations);
            }
        }

        return viewLocations;
    }
}
