using System;
using System.Collections.Generic;
using System.Data;
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
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers.CPC;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class CpcResourceFamilyController : CommonConfigurationController
{
    private readonly ICpcResourceFamilyRepository _iCpcResourceFamilyRepository;
    private readonly ITenantProvider _tenantProvider;

    public CpcResourceFamilyController(ApplicationDbContext uPrinceCustomerContext,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        ICpcResourceFamilyRepository iCpcResourceFamilyRepository, ITenantProvider tenantProvider) :
        base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iCpcResourceFamilyRepository = iCpcResourceFamilyRepository;
        _tenantProvider = iTenantProvider;
    }

    [HttpGet("ReadCpcResourceFamily")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCpcResourceFamily([FromHeader(Name = "CU")] string CU)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var connectionString = ConnectionString.MapConnectionString(CU,
                null, ItenantProvider);

            var query = @"
                           SELECT
                           CpcResourceFamilyLocalizedData.CpcResourceFamilyId AS Id
                           ,CpcResourceFamilyLocalizedData.Label AS title
                           ,CpcResourceFamilyLocalizedData.ParentId AS parentId
                            FROM dbo.CpcResourceFamilyLocalizedData
                            WHERE CpcResourceFamilyLocalizedData.LanguageCode = @lang
                            ORDER BY title
                              ";
            IEnumerable<CpcResourceFamilyDto> retResult;
            var parameters = new { lang };
            using (var dbConnection = new SqlConnection(connectionString))
            {
                retResult = dbConnection.Query<CpcResourceFamilyDto>(query, parameters).ToList();
            }


            ApiResponse.Message = "No available Cpc Resource Families";
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            return Ok(!retResult.Any() ? ApiResponse : new ApiOkResponse(retResult));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadCpcResourceFamilyForVP")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadCpcResourceFamilyForVP([FromHeader(Name = "CU")] string CU)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var connectionString = ConnectionString.MapConnectionString(CU,
                null, ItenantProvider);

            var query1 = @"WITH name_tree
                        AS
                        (SELECT
                            CpcResourceFamilyLocalizedData.CpcResourceFamilyId AS Id
                           ,CpcResourceFamilyLocalizedData.Label AS title
                           ,CpcResourceFamilyLocalizedData.parentId AS parentId
                           ,NULL AS CpcId
                          FROM dbo.CpcResourceFamilyLocalizedData
                          WHERE CpcResourceFamilyLocalizedData.CpcResourceFamilyId = '2210e768-tool-kknk-jhhk-ee367a82ad17' AND LanguageCode = 'en'
                          UNION ALL
                          SELECT
                            c.CpcResourceFamilyId AS Id
                           ,c.Label AS title
                           ,c.parentId AS parentId
                           ,NULL AS CpcId
                          FROM dbo.CpcResourceFamilyLocalizedData c
                          JOIN name_tree p
                            ON p.Id = c.ParentId)
                        SELECT
                          Id
                         ,title
                         ,parentId
                         ,CpcId
                        FROM name_tree";

            var query2 = @"SELECT
                          CorporateProductCatalog.Id AS Id
                         ,CorporateProductCatalog.Title AS title
                         ,CorporateProductCatalog.ResourceFamilyId AS parentId
                         ,CorporateProductCatalog.Id AS CpcId
                        FROM dbo.StockHeader
                        LEFT OUTER JOIN dbo.CorporateProductCatalog
                          ON StockHeader.CPCId = CorporateProductCatalog.Id
                          WHERE ResourceTypeId = 'c46c3a26-39a5-42cc-n9wn-89655304eh6'
                          AND CorporateProductCatalog.ResourceFamilyId IS NOT NULL
                        ORDER BY Title ";

            var wHQuery = "SELECT  VpWH.*, CONCAT(StartDate,' - ',EndDate) AS Title FROM VpWH";

            List<CpcResourceFamilyDto> retResult;
            List<CpcResourceFamilyDto> whList;

            var parameters = new { lang };
            using (var dbConnection = new SqlConnection(connectionString))
            {
                retResult = dbConnection.Query<CpcResourceFamilyDto>(query1, parameters).ToList();
                retResult.AddRange(dbConnection.Query<CpcResourceFamilyDto>(query2, parameters).ToList());
                retResult.AddRange(dbConnection.Query<CpcResourceFamilyDto>(wHQuery).ToList());

                whList = dbConnection.Query<CpcResourceFamilyDto>(wHQuery).ToList();

                var result = whList.GroupBy(r => r.ParentId);
                var percentageResult = whList.GroupBy(r => r.CPCId);

                foreach (var i in result)
                    if (i.Key != null)
                    {
                        DateTime end;
                        DateTime start;
                        end = (DateTime)i.Max(t => t.EndDate);
                        start = (DateTime)i.Min(t => t.StartDate);

                        var hh = new List<double>();


                        var whData = retResult.Where(k => k.Id == i.Key).FirstOrDefault();

                        if (whData != null)
                        {
                            whData.StartDate = start;
                            whData.EndDate = end;
                        }
                    }

                foreach (var j in percentageResult)
                    if (j.Key != null)
                    {
                        DateTime end;
                        DateTime start;
                        end = (DateTime)j.Max(t => t.EndDate);
                        start = (DateTime)j.Min(t => t.StartDate);

                        var hh = new List<double>();

                        foreach (var k in j)
                        {
                            DateTime kEndDate;
                            DateTime kStartDate;
                            kEndDate = (DateTime)k.EndDate;
                            kStartDate = (DateTime)k.StartDate;
                            var ff = (kEndDate - kStartDate).TotalDays;

                            hh.Add(ff);
                        }

                        var bb = hh.Sum();

                        var precentage = bb / (end - start).TotalDays * 100;

                        var whData = retResult.Where(k =>
                            k.CPCId == j.Key && k.ResourceTypeId == "c46c3a26-39a5-42cc-n9wn-89655304eh6");

                        if (whData != null)
                            foreach (var t in whData)
                                t.Percentage = precentage;
                    }
            }

            ApiResponse.Message = "No available Cpc Resource Families";
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            return Ok(!retResult.Any() ? ApiResponse : new ApiOkResponse(retResult));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreateCpcResourceFamily")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCpcResourceFamily(
        [FromBody] CpcResourceFamilyLocalizedData CpcResourceFamilyLocalizedData,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _cpcResourceFamilyParameters = new CpcResourceFamilyParameters
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _tenantProvider,
                cpcResourceFamily = CpcResourceFamilyLocalizedData
            };
            var result = await _iCpcResourceFamilyRepository.CreateCpcResourceFamily(_cpcResourceFamilyParameters);
            var mApiOkResponse = new ApiOkResponse(result);


            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noAvailableWorkFlow");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}