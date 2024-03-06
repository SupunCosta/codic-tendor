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
public class PbsProductStatusLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public PbsProductStatusLocalizedDataController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IDropDownRepository iDropDownRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
        ;
        _iDropDownRepository = iDropDownRepository;
    }
    

    [HttpGet("GetByCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPbsProductStatusLocalizedDataByCode(string Code)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var query = @"SELECT [dbo].[PbsProductStatusLocalizedData].[Id]
                                        ,[dbo].[PbsProductStatusLocalizedData].[Label]
                                        ,[dbo].[PbsProductStatusLocalizedData].[LanguageCode]
                                        ,[dbo].[PbsProductStatusLocalizedData].[PbsProductStatusId]
                                        ,[dbo].[PbsProductStatus].[Name] AS [Name]
                                FROM [dbo].[PbsProductStatusLocalizedData],[dbo].[PbsProductStatus]	                                
                                WHERE [dbo].[PbsProductStatusLocalizedData].[PbsProductStatusId] = [dbo].[PbsProductStatus].[Id]
                                AND PbsProductStatusLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetPbsProductStatusLocalizedDataByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetPbsProductStatusLocalizedDataByCode>(query, parameters);
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
    public async Task<ActionResult<int>> UpdatePbsProductStatusLocalizedData(
        [FromBody] UpdatePbsProductStatusLocalizedDataDto PbsProductStatusLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[PbsProductStatusLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                PbsProductStatusLocalizedData.Id,
                PbsProductStatusLocalizedData.Label,
                PbsProductStatusLocalizedData.LanguageCode
            };
            IEnumerable<PbsProductStatusLocalizedData> data;
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
    public async Task<ActionResult<int>> AddPbsProductStatusLocalizedDataDto(
        [FromBody] AddPbsProductStatusLocalizedDataDto PbsProductStatusLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), PbsProductStatusLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[PbsProductStatus]
                                           ([Id]
                                           ,[Name]
                                           ,[LocaleCode]
                                           ,[DisplayOrder])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,@Name
                                            ,(SELECT COUNT ([Name])FROM [dbo].[PbsProductStatus])+1)";

            IEnumerable<PbsProductStatusLocalizedData> data1;

            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", keyQuery, parameters1);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", keyQuery, parameters1);


            var Query = @"INSERT INTO [dbo].[PbsProductStatusLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[PbsProductStatusId]
                                           ,[DisplayOrder])
                                     VALUES
                                           (@Id
                                           ,@Label
                                           ,@LanguageCode
                                           ,(SELECT Id FROM [dbo].[PbsProductStatus] WHERE [dbo].[PbsProductStatus].Name = @Name)
                                           ,(SELECT DisplayOrder FROM [dbo].[PbsProductStatus] WHERE [dbo].[PbsProductStatus].Name = @Name)*5)";


            var parameters = new
            {
                Id = Guid.NewGuid().ToString(),
                PbsProductStatusLocalizedData.Label,
                PbsProductStatusLocalizedData.Name,
                PbsProductStatusLocalizedData.LanguageCode
            };

            IEnumerable<PbsProductStatusLocalizedData> data;
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