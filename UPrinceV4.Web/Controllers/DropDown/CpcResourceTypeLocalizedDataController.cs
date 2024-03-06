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
public class CpcResourceTypeLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public CpcResourceTypeLocalizedDataController(ITenantProvider tenantProvider,
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
    public async Task<IActionResult> GetCpcResourceTypeLocalizedDataByCode(string Code)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var query = @"SELECT [dbo].[CpcResourceTypeLocalizedData].[Id]
                                        ,[dbo].[CpcResourceTypeLocalizedData].[Label]
                                        ,[dbo].[CpcResourceTypeLocalizedData].[LanguageCode]
                                        ,[dbo].[CpcResourceTypeLocalizedData].[CpcResourceTypeId]
                                        ,[dbo].[CpcResourceType].[Name] AS [Name]
                                FROM [dbo].[CpcResourceTypeLocalizedData],[dbo].[CpcResourceType]	                                
                                WHERE [dbo].[CpcResourceTypeLocalizedData].[CpcResourceTypeId] = [dbo].[CpcResourceType].[Id]
                                AND CpcResourceTypeLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetCpcResourceTypeLocalizedDataByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetCpcResourceTypeLocalizedDataByCode>(query, parameters);
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateCpcResourceTypeLocalizedData(
        [FromBody] UpdateCpcResourceTypeLocalizedDataDto CpcResourceTypeLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;

            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[CpcResourceTypeLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                CpcResourceTypeLocalizedData.Id,
                CpcResourceTypeLocalizedData.Label,
                CpcResourceTypeLocalizedData.LanguageCode
            };
            IEnumerable<CpcResourceTypeLocalizedData> data;
            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", query, parameters);

            return Ok(new ApiOkResponse(exceptionLst, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> AddCpcResourceTypeLocalizedDataDto(
        [FromBody] AddCpcResourceTypeLocalizedDataDto CpcResourceTypeLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), CpcResourceTypeLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[CpcResourceType]
                                           ([Id]
                                           ,[Name]
                                           ,[DisplayOrder]
                                           ,[LocaleCode])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,0
                                            ,@Name)";

            IEnumerable<CpcResourceTypeLocalizedData> data1;
            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", keyQuery, parameters1);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", keyQuery, parameters1);


            var Query = @"INSERT INTO [dbo].[CpcResourceTypeLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[CpcResourceTypeId])
                                     VALUES
                                           (@Id
                                           ,@Label
                                           ,@LanguageCode
                                           ,(SELECT Id FROM [dbo].[CpcResourceType] WHERE [dbo].[CpcResourceType].Name = @Name))";


            var parameters = new
            {
                Id = Guid.NewGuid().ToString(),
                CpcResourceTypeLocalizedData.Label,
                CpcResourceTypeLocalizedData.Name,
                CpcResourceTypeLocalizedData.LanguageCode
            };

            IEnumerable<CpcResourceTypeLocalizedData> data;
            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", Query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", Query, parameters);

            return Ok(new ApiOkResponse(exceptionLst, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}