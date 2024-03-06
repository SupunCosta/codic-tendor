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
public class PbsProductItemTypeLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public PbsProductItemTypeLocalizedDataController(ITenantProvider tenantProvider,
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
    public async Task<IActionResult> GetPbsProductItemTypeLocalizedDataByCode(string Code)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var query = @"SELECT [dbo].[PbsProductItemTypeLocalizedData].[Id]
                                        ,[dbo].[PbsProductItemTypeLocalizedData].[Label]
                                        ,[dbo].[PbsProductItemTypeLocalizedData].[LanguageCode]
                                        ,[dbo].[PbsProductItemTypeLocalizedData].[PbsProductItemTypeId]
                                        ,[dbo].[PbsProductItemType].[Name] AS [Name]
                                FROM [dbo].[PbsProductItemTypeLocalizedData],[dbo].[PbsProductItemType]	                                
                                WHERE [dbo].[PbsProductItemTypeLocalizedData].[PbsProductItemTypeId] = [dbo].[PbsProductItemType].[Id]
                                AND PbsProductItemTypeLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetPbsProductItemTypeLocalizedDataByCode> data;
            await using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetPbsProductItemTypeLocalizedDataByCode>(query, parameters);
            }

            return Ok(new ApiOkResponse(data, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("Update")]
   
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdatePbsProductItemTypeLocalizedData(
        [FromBody] UpdatePbsProductItemTypeLocalizedDataDto PbsProductItemTypeLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[PbsProductItemTypeLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                PbsProductItemTypeLocalizedData.Id,
                PbsProductItemTypeLocalizedData.Label,
                PbsProductItemTypeLocalizedData.LanguageCode
            };
            IEnumerable<PbsProductItemTypeLocalizedData> data;
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
    public async Task<ActionResult<int>> AddPbsProductItemTypeLocalizedDataDto(
        [FromBody] AddPbsProductItemTypeLocalizedDataDto PbsProductItemTypeLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), PbsProductItemTypeLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[PbsProductItemType]
                                           ([Id]
                                           ,[Name]
                                           ,[LocaleCode])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,@Name)";

            IEnumerable<PbsProductItemTypeLocalizedData> data1;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data1 = connection.Query<PbsProductItemTypeLocalizedData>(keyQuery, parameters1);
            }


            var Query = @"INSERT INTO [dbo].[PbsProductItemTypeLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[PbsProductItemTypeId])
                                     VALUES
                                           (@Id
                                           ,@Label
                                           ,@LanguageCode
                                           ,(SELECT Id FROM [dbo].[PbsProductItemType] WHERE [dbo].[PbsProductItemType].Name = @Name))";


            var parameters = new
            {
                Id = Guid.NewGuid().ToString(),
                PbsProductItemTypeLocalizedData.Label,
                PbsProductItemTypeLocalizedData.Name,
                PbsProductItemTypeLocalizedData.LanguageCode
            };

            IEnumerable<PbsProductItemTypeLocalizedData> data;
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