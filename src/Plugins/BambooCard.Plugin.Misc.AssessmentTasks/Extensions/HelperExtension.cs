using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using BambooCard.Plugin.Misc.AssessmentTasks.Models.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Services.Attributes;
using Nop.Services.Common;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Extensions;

public static class HelperExtension
{
    public static Guid GetGuid(string deviceId)
    {
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(deviceId));
            var result = new Guid(hash);
            return result;
        }
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var characterArray = chars.Distinct().ToArray();
        var bytes = new byte[length * 8];
        var result = new char[length];
        using (var cryptoProvider = RandomNumberGenerator.Create())
        {
            cryptoProvider.GetBytes(bytes);
        }
        for (var i = 0; i < length; i++)
        {
            var value = BitConverter.ToUInt64(bytes, i * 8);
            result[i] = characterArray[value % (uint)characterArray.Length];
        }
        return new string(result);
    }

    public static IList<string> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = new List<string>();
        foreach (var ms in modelState.Values)
            foreach (var error in ms.Errors)
                errors.Add(error.ErrorMessage);

        return errors;
    }

    public static NameValueCollection ToNameValueCollection(this List<BCKeyValueApi> formValues)
    {
        var form = new NameValueCollection();
        if (formValues == null)
            return form;

        foreach (var values in formValues)
        {
            form.Add(values.Key, values.Value);
        }
        return form;
    }
}
