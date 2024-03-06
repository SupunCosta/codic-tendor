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
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.DropDown;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CpcBrandLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public CpcBrandLocalizedDataController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IDropDownRepository iDropDownRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;

        _iDropDownRepository = iDropDownRepository;
    }


    [HttpGet("GetByCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCpcBrandByCode(string Code)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();

            var query = @"SELECT [dbo].[CpcBrand].[Id]
                                        ,[dbo].[CpcBrand].[Name] as [Name]
                                        ,[dbo].[CpcBrand].[LocaleCode] as [Label]
                                        ,[dbo].[CpcBrand].[CpcBrandId]
                                        ,[dbo].[CpcBrand].[LanguageCode]
                                FROM [dbo].[CpcBrand]	                                
                                WHERE 
                                CpcBrand.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetCpcBrandByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<GetCpcBrandByCode>(query, parameters);
            }

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateCpcBrand([FromBody] UpdateCpcBrandDto CpcBrand)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            IEnumerable<DatabasesException> exceptionLst = null;
            CpcBrand data;
            if (CpcBrand.Id != null)
            {
                var query =
                    @"UPDATE [dbo].[CpcBrand] SET  [LocaleCode] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
                var parameters = new
                {
                    CpcBrand.Id, CpcBrand.Label, CpcBrand.LanguageCode
                };


                if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                    exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", query, parameters);
                else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                    exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", query, parameters);
                //using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
                //{
                //    data = connection.Query<CpcBrand>(query, parameters);
                //}
            }
            else
            {
                var brandInsert = @"INSERT INTO CpcBrand VALUES (@Id, @Name, @Label, @BrandId, @LangCode)";
                using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
                {
                    data = connection
                        .Query<CpcBrand>("SELECT * FROM CpcBrand WHERE Name = @Name", new { CpcBrand.Name })
                        .FirstOrDefault();
                }

                if (data != null)
                {
                    if (data.LanguageCode != CpcBrand.LanguageCode)
                    {
                        var param = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            CpcBrand.Name,
                            CpcBrand.Label,
                            BrandId = data.CpcBrandId,
                            LangCode = CpcBrand.LanguageCode
                        };

                        if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                            exceptionLst =
                                await _iDropDownRepository.Migration("uprincev4uatdb", brandInsert, param);
                        else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                            exceptionLst =
                                await _iDropDownRepository.Migration("uprincev4einstein", brandInsert, param);
                    }
                }
                else
                {
                    var param = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        CpcBrand.Name,
                        CpcBrand.Label,
                        BrandId = Guid.NewGuid().ToString(),
                        LangCode = CpcBrand.LanguageCode
                    };

                    if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                        exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", brandInsert, param);
                    else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                        exceptionLst =
                            await _iDropDownRepository.Migration("uprincev4einstein", brandInsert, param);
                }
            }

            return Ok(new ApiOkResponse(exceptionLst, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("Delete/{Code}")]
    public async Task<ActionResult> Delete(string Code, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            List<DatabasesException> exceptionLst = null;
            var deleteQuery = @"DELETE FROM [dbo].[CpcBrand] WHERE Id =@Id";

            var parameters = new { Id = Code };


            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", deleteQuery, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", deleteQuery, parameters);

            return Ok(new ApiOkResponse(exceptionLst, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}