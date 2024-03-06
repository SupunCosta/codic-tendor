using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Translations;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WebTranslationController : CommonConfigurationController
{
    private readonly ITenantProvider _tenantProvider;

    public WebTranslationController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
    }


    [HttpGet("getKeyWord/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getKeyWord(string id)
    {
        try
        {
            var query = @"SELECT * FROM dbo.WebTranslation WHERE WebTranslation.LanguageId=@Id ORDER BY [Key]";

            var parameters = new { id };

            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                return Ok(new ApiOkResponse(await connection.QueryAsync<WebTranslation>(query, parameters), "Ok"));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdateWebTranslation")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateWebTranslation([FromBody] UpdateWebTranslationDto webTranslation)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var r = new WebTranslation
            {
                Id = webTranslation.Id, Key = webTranslation.Key, Value = webTranslation.Value,
                LanguageId = webTranslation.LanguageId
            };
            UPrinceCustomerContext.WebTranslation.Update(r);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = r.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpPost("AddWebTranslation")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> AddWebTranslation([FromBody] AddWebTranslationDto webTranslation)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));


            var query = @"SELECT[ID]FROM[dbo].[WebTranslation] WHERE [Key] = @Key AND LanguageId = @LanguageId";

            var parameters = new { webTranslation.Key, webTranslation.LanguageId };
            WebTranslation data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<WebTranslation>(query, parameters).FirstOrDefault();
            }

            if (data != null) return BadRequest(new ApiResponse(400, false, "Already Exists"));

            var r = new WebTranslation
            {
                Key = webTranslation.Key, Value = webTranslation.Value, LanguageId = webTranslation.LanguageId
            };
            UPrinceCustomerContext.WebTranslation.Add(r);
            UPrinceCustomerContext.SaveChanges();
            ApiOkResponse.Result = r.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}