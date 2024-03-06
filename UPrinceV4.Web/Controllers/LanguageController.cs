using System;
using System.Collections.Generic;
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
public class LanguageController : CommonConfigurationController
{
    private readonly ITenantProvider _tenantProvider;

    public LanguageController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    [HttpGet("getLanguage")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getLanguage()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (string.IsNullOrEmpty(lang) || lang.Contains("en")) lang = "en";

            if (lang.Contains("nl")) lang = "nl";

            var query = @"SELECT Language.Id,Language.Country,Language.Lang,Language.Code FROM dbo.Language";

            var parameters = new { lang };
            IEnumerable<Language> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<Language>(query, parameters);
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetLanguageById/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLanguageById(int id)
    {
        try
        {
            var lang = Request.Headers["lang"];

            var query =
                @"SELECT Language.Id,Language.Country,Language.Lang,Language.Code FROM dbo.Language WHERE Language.Id=@Id";


            var parameters = new { Id = id };
            Language data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<Language>(query, parameters).FirstOrDefault();
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("GetCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCode(string lan, string country)
    {
        try
        {
            var lang = Request.Headers["lang"];

            var query = @"SELECT [Id],[Code] FROM [dbo].[Language] WHERE [Lang]=@Lan AND [Country]=@Country ";


            var parameters = new { Lan = lan, Country = country };
            Language data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<Language>(query, parameters).FirstOrDefault();
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    //[HttpPost("UpdateLanguage")]
    [HttpPost("UpdateLanguage")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateLanguage([FromBody] UpdateLanguageDto language)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var r = new Language
                { Id = language.Id, Country = language.Country, Lang = language.Lang, Code = language.Code };
            UPrinceCustomerContext.Language.Update(r);
            await UPrinceCustomerContext.SaveChangesAsync();
            ApiOkResponse.Result = r.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    //[HttpPost("AddLanguage")]
    [HttpPost("AddLanguage")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> AddLanguage([FromBody] Language language)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));


            var query = @"SELECT[ID]FROM[dbo].[Language] WHERE [Code] = @Code";


            var parameters = new { language.Code };
            WebTranslation data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<WebTranslation>(query, parameters).FirstOrDefault();
            }

            if (data != null)
            {
                ApiResponse.StatusCode = 400;
                ApiResponse.Status = false;
                ApiResponse.Message = "Already Exists";
                return BadRequest(ApiResponse);
            }

            var r = new Language { Country = language.Country, Code = language.Code, Lang = language.Lang };
            UPrinceCustomerContext.Language.Add(r);
            UPrinceCustomerContext.SaveChanges();


            var keyquery = @"INSERT INTO [dbo].[WebTranslation] ([Key],[Value],[LanguageId]) 
                                        SELECT
                                          WebTranslation.[Key]
                                          ,CONCAT(WebTranslation.Value, '( ', @Code, ')') AS expr1
                                         ,(SELECT TOP 1 [Id] FROM [dbo].[Language] ORDER BY [Id] DESC )
 
                                        FROM dbo.WebTranslation
                                        INNER JOIN dbo.Language
                                          ON WebTranslation.LanguageId = Language.Id
                                        WHERE WebTranslation.LanguageId= (SELECT TOP 1 [Id] FROM [dbo].[WebTranslation] ORDER BY [Id] ASC )";

            var parameter = new { language.Code };
            WebTranslation keydata;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                keydata = connection.Query<WebTranslation>(keyquery, parameters).FirstOrDefault();
            }


            ApiOkResponse.Result = r.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("getLanguageDistinct")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getLanguageDistinct()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (string.IsNullOrEmpty(lang) || lang.Contains("en")) lang = "en";

            if (lang.Contains("nl")) lang = "nl";

            var query = @"SELECT DISTINCT Language.Lang FROM dbo.Language ORDER BY Lang ASC";

            var parameters = new { lang };
            IEnumerable<Language> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<Language>(query, parameters);
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpGet("getCountryDistinct")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getCountryDistinct()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            if (string.IsNullOrEmpty(lang) || lang.Contains("en")) lang = "en";

            if (lang.Contains("nl")) lang = "nl";

            var query = @"SELECT DISTINCT Language.Country FROM dbo.Language ORDER BY Country ASC";

            var parameters = new { lang };
            IEnumerable<Language> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<Language>(query, parameters);
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}