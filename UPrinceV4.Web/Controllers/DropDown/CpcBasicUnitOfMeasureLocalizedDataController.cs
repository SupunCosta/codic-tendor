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
public class CpcBasicUnitOfMeasureLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public CpcBasicUnitOfMeasureLocalizedDataController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IDropDownRepository iDropDownRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;

        _iDropDownRepository = iDropDownRepository;
    }


    [HttpGet("Get")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCpcBasicUnitOfMeasureLocalizedData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var query = @"SELECT [dbo].[CpcBasicUnitOfMeasureLocalizedData].[Id]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[Label]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[LanguageCode]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[BasicUnitOfMeasureId]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[CpcBasicUnitOfMeasureId]
	                                    ,[dbo].[CpcBasicUnitOfMeasure].[Name] AS [Name]
                                FROM [dbo].[CpcBasicUnitOfMeasureLocalizedData],[dbo].[CpcBasicUnitOfMeasure]
                                WHERE [dbo].[CpcBasicUnitOfMeasureLocalizedData].[CpcBasicUnitOfMeasureId] = [dbo].[CpcBasicUnitOfMeasure].[Id]";

            var parameters = new { lang };
            IEnumerable<GetCpcResourceTypeLocalizedDataByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetCpcResourceTypeLocalizedDataByCode>(query, parameters);
            }

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetByCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCpcBasicUnitOfMeasureLocalizedDataByCode(string Code)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var query = @"SELECT [dbo].[CpcBasicUnitOfMeasureLocalizedData].[Id]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[Label]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[LanguageCode]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[BasicUnitOfMeasureId]
                                        ,[dbo].[CpcBasicUnitOfMeasureLocalizedData].[CpcBasicUnitOfMeasureId]
	                                    ,[dbo].[CpcBasicUnitOfMeasure].[Name] AS [Name]
                                FROM [dbo].[CpcBasicUnitOfMeasureLocalizedData],[dbo].[CpcBasicUnitOfMeasure]
                                WHERE [dbo].[CpcBasicUnitOfMeasureLocalizedData].[CpcBasicUnitOfMeasureId] = [dbo].[CpcBasicUnitOfMeasure].[Id]
                                AND CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetCpcBasicUnitOfMeasureLocalizedDataByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetCpcBasicUnitOfMeasureLocalizedDataByCode>(query, parameters);
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
    public async Task<ActionResult<int>> UpdateCpcBasicUnitOfMeasureLocalizedData(
        [FromBody] UpdateCpcBasicUnitOfMeasureLocalizedDataDto CpcBasicUnitOfMeasureLocalizedData)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[CpcBasicUnitOfMeasureLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                CpcBasicUnitOfMeasureLocalizedData.Id,
                CpcBasicUnitOfMeasureLocalizedData.Label,
                CpcBasicUnitOfMeasureLocalizedData.LanguageCode
            };
            IEnumerable<CpcBasicUnitOfMeasureLocalizedData> data;
            IEnumerable<DatabasesException> exceptionLst = null;

            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", query, parameters);


            return Ok(new ApiOkResponse(exceptionLst));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("Add")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> AddCpcBasicUnitOfMeasureLocalizedData(
        [FromBody] AddCpcBasicUnitOfMeasureLocalizedDataDto CpcBasicUnitOfMeasureLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), CpcBasicUnitOfMeasureLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[CpcBasicUnitOfMeasure]
                                           ([Id]
                                           ,[Name]
                                           ,[DisplayOrder]
                                           ,[LocaleCode])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,0
                                            ,@Name)";

            CpcBasicUnitOfMeasure data1;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data1 = connection
                    .Query<CpcBasicUnitOfMeasure>("SELECT * FROM CpcBasicUnitOfMeasure WHERE Name = @Name",
                        new { CpcBasicUnitOfMeasureLocalizedData.Name }).FirstOrDefault();
            }

            if (data1 == null)
            {
                if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                    exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", keyQuery, parameters1);
                else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                    exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", keyQuery, parameters1);
                //using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
                //{
                //    data1 = connection.Query<CpcBasicUnitOfMeasureLocalizedData>(keyQuery,parameters1);
                //}
            }

            var Query = @"INSERT INTO [dbo].[CpcBasicUnitOfMeasureLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[BasicUnitOfMeasureId]
                                           ,[CpcBasicUnitOfMeasureId])
                                     VALUES
                                           (@Id1
                                           ,@Label
                                           ,@LanguageCode
                                           ,@BasicUnitOfMeasureId
                                           ,(SELECT Id FROM [dbo].[CpcBasicUnitOfMeasure] WHERE [dbo].[CpcBasicUnitOfMeasure].Name = @Name))";


            var parameters = new
            {
                Id1 = Guid.NewGuid().ToString(),
                CpcBasicUnitOfMeasureLocalizedData.Label,
                CpcBasicUnitOfMeasureLocalizedData.Name,
                CpcBasicUnitOfMeasureLocalizedData.LanguageCode,
                CpcBasicUnitOfMeasureLocalizedData.BasicUnitOfMeasureId
            };

            IEnumerable<CpcBasicUnitOfMeasureLocalizedData> data;

            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", Query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", Query, parameters);
            //using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            //{
            //    data = connection.Query<CpcBasicUnitOfMeasureLocalizedData>(Query, parameters);
            //}

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
            var deleteQuery = @"DELETE FROM [dbo].[CpcBasicUnitOfMeasureLocalizedData] WHERE Id =@Id";

            var parameters = new { Id = Code };


            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", deleteQuery, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", deleteQuery, parameters);

            return Ok(new ApiOkResponse(exceptionLst));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}