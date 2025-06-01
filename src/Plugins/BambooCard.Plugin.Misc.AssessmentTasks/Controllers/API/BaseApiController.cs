using BambooCard.Plugin.Misc.AssessmentTasks.Extensions;
using BambooCard.Plugin.Misc.AssessmentTasks.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.Filters;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Controllers.API;

[TokenAuthorize]
[PublishModelEvents]
[SaveIpAddress]
[SaveLastActivity]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ILocalizationService _localizationService;
    protected readonly ICustomerService _customerService;

    protected BaseApiController(ILocalizationService localizationService,
        ICustomerService customerService)
    {
        _localizationService = localizationService;
        _customerService = customerService;
    }

    protected async Task<bool> IsGuestCustomerAsync(Customer customer)
    {
        if (customer == null || await _customerService.IsGuestAsync(customer))
            return true;

        return false;
    }

    protected async Task<IActionResult> OkWrap<T>(T baseModel, string message = null, IList<string> errors = null, bool defaultMessage = false) where T : BaseNopModel
    {
        var model = new BCGenericResponseModel<T>();
        model.Data = baseModel;
        if (defaultMessage && string.IsNullOrWhiteSpace(message))
            message = await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.Ok");
        model.Message = message;
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        return base.Ok(model);
    }

    protected async Task<IActionResult> BadRequestWrap<T>(T baseModel, ModelStateDictionary modelState = null, IList<string> errors = null, bool showDefaultMessageIfEmpty = true) where T : BaseNopModel
    {
        var model = new BCGenericResponseModel<T>();
        model.Data = baseModel;
        if (modelState != null && !modelState.IsValid)
            model.ErrorList.AddRange(modelState.GetErrors());
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && showDefaultMessageIfEmpty)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.BadRequest"));
        return base.BadRequest(model);
    }

    /// <summary>
    /// Returns Ok response (200 status code)
    /// </summary>
    /// <param name="message">The success message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> Ok(string message = null, IList<string> errors = null, bool defaultMessage = false)
    {
        var model = new BCBaseResponseModel();
        if (defaultMessage && string.IsNullOrWhiteSpace(message))
            message = await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.Ok");
        model.Message = message;
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        return base.Ok(model);
    }

    /// <summary>
    /// Returns Created response (201 status code)
    /// </summary>
    /// <param name="id">The entity id</param>
    /// <param name="message">The success message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> Created(int id, string message = null, IList<string> errors = null, bool defaultMessage = true)
    {
        var model = new BCGenericResponseModel<int>();
        model.Data = id;
        if (defaultMessage && string.IsNullOrWhiteSpace(message))
            message = await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.Ok");
        model.Message = message;
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        return base.StatusCode(StatusCodes.Status201Created, model);
    }

    /// <summary>
    /// Returns BadRequest response (400 status code)
    /// </summary>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> BadRequest(string error = null, IList<string> errors = null, bool defaultMessage = true)
    {
        var model = new BCBaseResponseModel();
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.BadRequest"));
        return base.BadRequest(model);
    }

    /// <summary>
    /// Returns Unauthorized response (401 status code)
    /// </summary>
    /// <typeparam name="T">Type of 'BaseNopModel'</typeparam>
    /// <param name="baseModel">The main model</param>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> UnauthorizedWrap<T>(T baseModel, string error = null, IList<string> errors = null, bool defaultMessage = true) where T : BaseNopModel
    {
        var model = new BCGenericResponseModel<T>();
        model.Data = baseModel;
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.Unauthorized"));
        return base.Unauthorized(model);
    }

    /// <summary>
    /// Returns Unauthorized response (401 status code)
    /// </summary>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> Unauthorized(string error = null, IList<string> errors = null, bool defaultMessage = true)
    {
        var model = new BCBaseResponseModel();
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.Unauthorized"));
        return base.Unauthorized(model);
    }

    /// <summary>
    /// Returns NotFound response (404 status code)
    /// </summary>
    /// <typeparam name="T">Type of 'BaseNopModel'</typeparam>
    /// <param name="baseModel">The main model</param>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> NotFoundWrap<T>(T baseModel, string error = null, IList<string> errors = null, bool defaultMessage = true) where T : BaseNopModel
    {
        var model = new BCGenericResponseModel<T>();
        model.Data = baseModel;
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.NotFound"));
        return base.NotFound(model);
    }

    /// <summary>
    /// Returns NotFound response (404 status code)
    /// </summary>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> NotFound(string error = null, IList<string> errors = null, bool defaultMessage = true)
    {
        var model = new BCBaseResponseModel();
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.NotFound"));
        return base.NotFound(model);
    }

    /// <summary>
    /// Returns InternalServerError response (500 status code)
    /// </summary>
    /// <typeparam name="T">Type of 'BaseNopModel'</typeparam>
    /// <param name="baseModel">The main model</param>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> InternalServerErrorWrap<T>(T baseModel, string error = null, IList<string> errors = null, bool defaultMessage = true) where T : BaseNopModel
    {
        var model = new BCGenericResponseModel<T>();
        model.Data = baseModel;
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.InternalServerError"));
        return base.StatusCode(StatusCodes.Status500InternalServerError, model);
    }

    /// <summary>
    /// Returns InternalServerError response (500 status code)
    /// </summary>
    /// <param name="error">Single error message</param>
    /// <param name="errors">The error list</param>
    /// <param name="defaultMessage">'true' value indicates it will send a default success message with the response.</param>
    /// <returns></returns>
    protected async Task<IActionResult> InternalServerError(string error = null, IList<string> errors = null, bool defaultMessage = true)
    {
        var model = new BCBaseResponseModel();
        if (!string.IsNullOrWhiteSpace(error))
            model.ErrorList.Add(error);
        if (errors != null && errors.Any())
            model.ErrorList.AddRange(errors);
        if (!model.ErrorList.Any() && defaultMessage)
            model.ErrorList.Add(await _localizationService.GetResourceAsync("Crowley.Plugin.Request.Common.InternalServerError"));
        return base.StatusCode(StatusCodes.Status500InternalServerError, model);
    }

    /// <summary>
    /// Returns MethodNotAllowed response (405 status code)
    /// </summary>
    /// <returns></returns>
    protected IActionResult MethodNotAllowed()
    {
        return base.StatusCode(StatusCodes.Status405MethodNotAllowed);
    }

    /// <summary>
    /// Returns LengthRequired response (411 status code)
    /// </summary>
    /// <returns></returns>
    protected IActionResult LengthRequired()
    {
        return base.StatusCode(StatusCodes.Status411LengthRequired);
    }
}
