using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;
[Route("")]
[ApiController]
public class RootController : Controller
{
    [Route(""), HttpGet]
    public ActionResult Index()
    {
        var feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
        return Content(JsonConvert.SerializeObject(new ApiOkResponse( feature?.Error.Message)), "application/json");
    }   
    [Route("Error"), HttpGet]
    public IActionResult Error(int? statusCode = null)
    {
        // if (statusCode == 401)
        // {
        //     return Unauthorized();
        // }
        // if (statusCode == 404 || statusCode == 500)
        // {
        //     return Ok(new ApiOkResponse("Error code " + statusCode));
        // }
        // return Ok(new ApiOkResponse("Error code " + statusCode));
        
        var feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
        var content = new ExceptionMessageContent();

        content.Error = $"{statusCode}";

        switch (statusCode)
        {
            case 401:
                content.Error = "invalid_token";
                content.Message = "Your login may have timed out, please log in again (401 Unauthorized)";
                break;
            default:
                content.Error = feature?.Error.Message;
                content.Message = $"Server Error! The Server responded with status code {statusCode}";
                break;
        }
        //{ Error = "Server Error", Message = $"The Server responded with status code {statusCode}" };
        return Content(JsonConvert.SerializeObject(content), "application/json");

    }
}

public class ExceptionMessageContent
{
    public string Error;
    public string Message;
}

