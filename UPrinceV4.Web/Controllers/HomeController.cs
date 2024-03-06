using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Web.Models;

//using UPrinceV4.Web.Commands.SaveProduct;

namespace UPrinceV4.Web.Controllers;

public class HomeController
{
    [AllowAnonymous]
    public async Task<IActionResult> Error(int? statusCode = null)
    {
        var code = statusCode.GetValueOrDefault();
        //ApiResponse response = new ApiResponse(statusCode, "", false);
        //ApiResponse.StatusCode = code;
        //ApiResponse.Status = false;
        ////return new ObjectResult(ApiResponse);
        //return Ok(calenderTemplate == null
        //       ? new ApiResponse(200, false, "No Calendar Templates available for the Id " + id)
        return new ObjectResult(new ApiResponse(code, false));
    }
}