using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileUploadController : CommonConfigurationController
{
    private readonly ITenantProvider _TenantProvider;

    public FileUploadController(ITenantProvider tenantProvider,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApplicationDbContext uPrinceCustomerContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _TenantProvider = tenantProvider;
    }

    [HttpPost("UploadFile")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UploadFile([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var message = "Ok";
            var uploadData = new UploadData();
            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider
                , image.Files.FirstOrDefault(), "Uploads");
            uploadData.Url = url;
            uploadData.FileName = image.Files.FirstOrDefault().FileName;

            return Ok(new ApiOkResponse(uploadData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CKUploadFile")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CKUploadFile([FromForm] IFormCollection image)
    {
        try
        {
            //_logger.LogTrace("Started");
            //log.Error("UploadImage");
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var uploadData = new UploadData();
            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider
                , image.Files.FirstOrDefault(), "Uploads");
            uploadData.Url = url;
            uploadData.FileName = image.Files.FirstOrDefault()?.FileName;

            return Ok(uploadData);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}