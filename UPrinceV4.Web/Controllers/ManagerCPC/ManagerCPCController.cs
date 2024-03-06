using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers.ManagerCPC;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ManagerCPCController : CommonConfigurationController
{
    private readonly ITenantProvider _tenantProvider;

    public ManagerCPCController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
       )
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
    }
    

    [AllowAnonymous]
    [HttpGet("Get")]
    public IActionResult GetCpcManager([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();

            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                var query = @"SELECT
                                  ProjectDefinition.SequenceCode AS Project
                                 ,CabPerson.FullName AS CabPerson
                                FROM dbo.ProjectTeam
                                INNER JOIN dbo.ProjectDefinition
                                  ON ProjectTeam.ProjectId = ProjectDefinition.Id
                                INNER JOIN dbo.ProjectTeamRole
                                  ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                                INNER JOIN dbo.CabPerson
                                  ON ProjectTeamRole.CabPersonId = CabPerson.Id
                                WHERE ProjectTeamRole.RoleId IN ('1666e217-2b80-4acd-b48b-b041fe263fb9', '476127cb-62db-4af7-ac8e-d4a722f8e142')";

                var parameters = new { lang };
                IEnumerable<CpcManagerDto> data;
                using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
                {
                    data = connection.Query<CpcManagerDto>(query, parameters);
                }

                // ArrayList Cpc = new ArrayList();
                foreach (var i in data)

                {
                    var db = i.Project;

                    //string Connection = $"Server=tcp:uprincev4staging.database.windows.net,1433;Initial Catalog={db};Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                    var Connection = ConnectionString.MapConnectionString(null, db, _tenantProvider);

                    var queryCPC = @"SELECT
                                          CpcResourceType.Name as ResourceName
                                         ,CorporateProductCatalog.ResourceTitle
                                         ,CorporateProductCatalog.ResourceNumber
                                        FROM dbo.CorporateProductCatalog
                                        INNER JOIN dbo.CpcResourceType
                                          ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id
                                         where IsDeleted = 0";

                    var parameter = new { lang };
                    IEnumerable cpcdata;
                    using (var connection = new SqlConnection(Connection))
                    {
                        cpcdata = connection.Query<CpcListDto>(queryCPC, parameter);
                    }

                    i.CPC = (List<CpcListDto>)cpcdata;
                    //Cpc.Add(i);
                    //foreach (CpcListDto n in cpcdata) {
                    //    i.CPCName = n.CPCName;
                    //    Cpc.Add(i);
                    //}
                }

                return Ok(new ApiOkResponse(data, "Ok"));
            }


            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}