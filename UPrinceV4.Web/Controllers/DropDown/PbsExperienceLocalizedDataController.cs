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
public class PbsExperienceLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public PbsExperienceLocalizedDataController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider, IDropDownRepository iDropDownRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
        _iDropDownRepository = iDropDownRepository;
    }
    

    [HttpGet("GetByCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPbsExperienceLocalizedDataByCode(string Code)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();

            var query = @"SELECT [dbo].[PbsExperienceLocalizedData].[Id]
                                        ,[dbo].[PbsExperienceLocalizedData].[Label]
                                        ,[dbo].[PbsExperienceLocalizedData].[LanguageCode]
                                        ,[dbo].[PbsExperienceLocalizedData].[PbsExperienceId]
                                        ,[dbo].[PbsExperience].[Name] AS [Name]
                                FROM [dbo].[PbsExperienceLocalizedData],[dbo].[PbsExperience]	                                
                                WHERE [dbo].[PbsExperienceLocalizedData].[PbsExperienceId] = [dbo].[PbsExperience].[Id]
                                AND PbsExperienceLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetPbsExperienceLocalizedDataByCode> data;
            await using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetPbsExperienceLocalizedDataByCode>(query, parameters);
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
    public async Task<ActionResult<int>> UpdatePbsExperienceLocalizedData(
        [FromBody] UpdatePbsExperienceLocalizedDataDto PbsExperienceLocalizedData)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[PbsExperienceLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                PbsExperienceLocalizedData.Id,
                PbsExperienceLocalizedData.Label,
                PbsExperienceLocalizedData.LanguageCode
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
    public async Task<ActionResult<int>> AddPbsExperienceLocalizedDataDto(
        [FromBody] AddPbsExperienceLocalizedDataDto PbsExperienceLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), PbsExperienceLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[PbsExperience]
                                           ([Id]
                                           ,[Name]
                                           ,[LocaleCode])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,@Name)";

            IEnumerable<PbsExperienceLocalizedData> data1;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                connection.Query<PbsExperienceLocalizedData>(keyQuery, parameters1);
            }


            var Query = @"INSERT INTO [dbo].[PbsExperienceLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[PbsExperienceId])
                                     VALUES
                                           (@Id
                                           ,@Label
                                           ,@LanguageCode
                                           ,(SELECT Id FROM [dbo].[PbsExperience] WHERE [dbo].[PbsExperience].Name = @Name))";


            var parameters = new
            {
                Id = Guid.NewGuid().ToString(),
                PbsExperienceLocalizedData.Label,
                PbsExperienceLocalizedData.Name,
                PbsExperienceLocalizedData.LanguageCode
            };

            IEnumerable<PbsExperienceLocalizedData> data;
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