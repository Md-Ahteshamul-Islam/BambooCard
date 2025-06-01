namespace BambooCard.Plugin.Misc.AssessmentTasks;

/// <summary>
/// Represents plugin constants
/// </summary>
public class AssessmentTasksDefaults
{
    /// <summary>
    /// Gets a plugin system name
    /// </summary>
    public static string SystemName => "Misc.AssessmentTasks";

    /// <summary>
    /// Gets a plugin menu system name
    /// </summary>
    public static string PluginMenuSystemName => "BambooCard";
    /// <summary>
    /// Gets a path to the file that contains Resource string xml File
    /// </summary>
    public static string XmlResourceStringFilePath => "~/Plugins/BambooCard.Misc.AssessmentTasks/ResourceString/BambooCard.Resources.en-us.xml";
    public static string ExtendedControllerPreFix => "Override";
    public static readonly string Token = "BC-Token";
    public static readonly string CustomerToken = ".Nop.Customer.BC-Token";
    public static readonly string JwtRefreshToken = ".Nop.Customer.JwtRefreshToken";
    public static readonly string CustomerGuid = "CustomerGuid";
    public static readonly string CustomerId = "BC-CustomerId";
}