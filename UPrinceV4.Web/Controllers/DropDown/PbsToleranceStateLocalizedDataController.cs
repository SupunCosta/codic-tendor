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
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.DropDown;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PbsToleranceStateLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public PbsToleranceStateLocalizedDataController(ITenantProvider tenantProvider,
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
    public async Task<IActionResult> GetPbsToleranceStateLocalizedDataByCode(string Code)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var query = @"SELECT [dbo].[PbsToleranceStateLocalizedData].[Id]
                                        ,[dbo].[PbsToleranceStateLocalizedData].[Label]
                                        ,[dbo].[PbsToleranceStateLocalizedData].[LanguageCode]
                                        ,[dbo].[PbsToleranceStateLocalizedData].[PbsToleranceStateId]
                                        ,[dbo].[PbsToleranceState].[Name] AS [Name]
                                FROM [dbo].[PbsToleranceStateLocalizedData],[dbo].[PbsToleranceState]	                                
                                WHERE [dbo].[PbsToleranceStateLocalizedData].[PbsToleranceStateId] = [dbo].[PbsToleranceState].[Id]
                                AND PbsToleranceStateLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetPbsToleranceStateLocalizedDataByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetPbsToleranceStateLocalizedDataByCode>(query, parameters);
            }

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("Update")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdatePbsToleranceStateLocalizedData(
        [FromBody] UpdatePbsToleranceStateLocalizedDataDto PbsToleranceStateLocalizedData)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[PbsToleranceStateLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                PbsToleranceStateLocalizedData.Id,
                PbsToleranceStateLocalizedData.Label,
                PbsToleranceStateLocalizedData.LanguageCode
            };
            IEnumerable<DatabasesException> exceptionLst = null;
            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", query, parameters);

            return Ok(new ApiOkResponse(exceptionLst, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("Add")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> AddPbsToleranceStateLocalizedDataDto(
        [FromBody] AddPbsToleranceStateLocalizedDataDto PbsToleranceStateLocalizedData)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), PbsToleranceStateLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[PbsToleranceState]
                                           ([Id]
                                           ,[Name]
                                           ,[LocaleCode])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,@Name)";

            IEnumerable<DatabasesException> exceptionLst = null;
            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", keyQuery, parameters1);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", keyQuery, parameters1);


            var Query = @"INSERT INTO [dbo].[PbsToleranceStateLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[PbsToleranceStateId])
                                     VALUES
                                           (@Id
                                           ,@Label
                                           ,@LanguageCode
                                           ,(SELECT Id FROM [dbo].[PbsToleranceState] WHERE [dbo].[PbsToleranceState].Name = @Name))";


            var parameters = new
            {
                Id = Guid.NewGuid().ToString(),
                PbsToleranceStateLocalizedData.Label,
                PbsToleranceStateLocalizedData.Name,
                PbsToleranceStateLocalizedData.LanguageCode
            };

            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", Query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", Query, parameters);

            return Ok(new ApiOkResponse(exceptionLst, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}