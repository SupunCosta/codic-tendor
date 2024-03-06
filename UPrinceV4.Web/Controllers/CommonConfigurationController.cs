using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class CommonConfigurationController : Controller
{
    protected ApiBadRequestResponse ApiBadRequestResponse;
    protected ApiOkResponse ApiOkResponse;
    protected ApiResponse ApiResponse;
    protected IHttpContextAccessor ContextAccessor;
    protected ITenantProvider ItenantProvider;
    protected ApplicationDbContext UPrinceCustomerContext;

    public CommonConfigurationController(ApplicationDbContext uPrinceCustomerContext,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse
        , ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
    {
        ApiResponse = apiResponse;
        ApiBadRequestResponse = apiBadRequestResponse;
        ApiOkResponse = apiOkResponse;
        UPrinceCustomerContext = uPrinceCustomerContext;
        ContextAccessor = contextAccessor;
        ItenantProvider = iTenantProvider;
    }
    
    [HttpPost("langCode")]
    public string langCode(string lang)
    {
        string langaValue;
        if (lang != null)
        {
            if (lang.Contains("nl"))
                langaValue = "nl";

            else if (lang.Contains("en"))
                langaValue = "en";

            else if (lang.Contains("pl"))
                langaValue = "pl";
            else if (lang.Contains("fr"))
                langaValue = "fr";
            else
                langaValue = "en";
        }
        else
        {
            langaValue = "en";
        }

        return langaValue;
    }
}