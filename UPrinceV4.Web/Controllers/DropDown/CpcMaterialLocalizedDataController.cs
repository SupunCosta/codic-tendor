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
public class CpcMaterialLocalizedDataController : CommonConfigurationController
{
    private readonly IDropDownRepository _iDropDownRepository;
    private readonly ITenantProvider _tenantProvider;

    public CpcMaterialLocalizedDataController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IDropDownRepository iDropDownRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;

        _iDropDownRepository = iDropDownRepository;
    }

    public object UPrinceCpcMaterialLocalizedDataControllerContex { get; }

    [HttpGet("GetByCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCpcMaterialLocalizedDataByCode(string Code)
    {
        try
        {

            var query = @"SELECT [dbo].[CpcMaterialLocalizedData].[Id]
                                        ,[dbo].[CpcMaterialLocalizedData].[Label]
                                        ,[dbo].[CpcMaterialLocalizedData].[LanguageCode]
                                        ,[dbo].[CpcMaterialLocalizedData].[CpcMaterialId]
                                        ,[dbo].[CpcMaterial].[Name] AS [Name]
                                FROM [dbo].[CpcMaterialLocalizedData],[dbo].[CpcMaterial]	                                
                                WHERE [dbo].[CpcMaterialLocalizedData].[CpcMaterialId] = [dbo].[CpcMaterial].[Id]
                                AND CpcMaterialLocalizedData.LanguageCode=@Code";

            var parameters = new { Code };
            IEnumerable<GetCpcMaterialLocalizedDataByCode> data;
            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data = connection
                    .Query<GetCpcMaterialLocalizedDataByCode>(query, parameters);
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
    public async Task<ActionResult<int>> UpdateCpcMaterialLocalizedDataByCode(
        [FromBody] UpdateCpcMaterialLocalizedDataDto CpcMaterialLocalizedData)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var query =
                @"UPDATE [dbo].[CpcMaterialLocalizedData] SET  [Label] =@Label,[LanguageCode]=@LanguageCode WHERE [Id] = @Id ";
            var parameters = new
            {
                CpcMaterialLocalizedData.Id,
                CpcMaterialLocalizedData.Label,
                CpcMaterialLocalizedData.LanguageCode
            };
            IEnumerable<CpcMaterialLocalizedData> data;
            IEnumerable<DatabasesException> exceptionLst = null;

            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", query, parameters);
            //using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            //    {
            //        data = connection.Query<CpcMaterialLocalizedData>(query, parameters);
            //    }

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
    public async Task<ActionResult<int>> AddCpcMaterialLocalizedDataDto(
        [FromBody] AddCpcMaterialLocalizedDataDto CpcMaterialLocalizedData)
    {
        try
        {
            IEnumerable<DatabasesException> exceptionLst = null;
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var parameters1 = new
            {
                Id = Guid.NewGuid().ToString(), CpcMaterialLocalizedData.Name
            };

            var keyQuery = @"INSERT INTO [dbo].[CpcMaterial]
                                           ([Id]
                                           ,[Name]
                                           ,[DisplayOrder]
                                           ,[LocaleCode])
                                     VALUES
                                           (@Id
                                            ,@Name
                                            ,0
                                            ,@Name)";

            CpcMaterialLocalizedData data1;

            using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            {
                data1 = connection.Query<CpcMaterialLocalizedData>("SELECT * FROM CpcMaterial WHERE Name = @Name",
                    new { CpcMaterialLocalizedData.Name }).FirstOrDefault();
            }

            if (data1 == null)
            {
                if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                    exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", keyQuery, parameters1);
                else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                    exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", keyQuery, parameters1);
            }
            //using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            //{
            //    data1 = connection.Query<CpcMaterialLocalizedData>(keyQuery, parameters1);
            //}

            var Query = @"INSERT INTO [dbo].[CpcMaterialLocalizedData]
                                           ([Id]
                                           ,[Label]
                                           ,[LanguageCode]
                                           ,[CpcMaterialId])
                                     VALUES
                                           (@Id
                                           ,@Label
                                           ,@LanguageCode
                                           ,(SELECT Id FROM [dbo].[CpcMaterial] WHERE [dbo].[CpcMaterial].Name = @Name))";


            var parameters = new
            {
                Id = Guid.NewGuid().ToString(),
                CpcMaterialLocalizedData.Label,
                CpcMaterialLocalizedData.Name,
                CpcMaterialLocalizedData.LanguageCode
            };

            IEnumerable<CpcMaterialLocalizedData> data;
            if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4uatdb", Query, parameters);
            else if (_tenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await _iDropDownRepository.Migration("uprincev4einstein", Query, parameters);
            //using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            //{
            //    data = connection.Query<CpcMaterialLocalizedData>(Query, parameters);
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
            var deleteQuery = @"DELETE FROM [dbo].[CpcMaterialLocalizedData] WHERE Id =@Id";

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