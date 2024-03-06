using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers.ExactOnline;

public class ExactOnlineController : CommonConfigurationController
{
    public ExactOnlineController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider) : base(uPrinceCustomerContext, contextAccessor, apiResponse,
        apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _iTenantProvider = iTenantProvider;
    }

    public ITenantProvider _iTenantProvider { get; }

    [AllowAnonymous]
    [HttpPost("Account")]
    public async Task<ActionResult> Account([FromBody] Data.ExactOnline.ExactOnline exactOnline)
    {
        try
        {
            var query =
                @"INSERT INTO dbo.ExactOnline ( Id ,Topic ,Action ,Division ,[Key] ,ExactOnlineEndpoint ,EventCreatedOn ) VALUES ( @Id ,@Topic ,@Action ,@Division ,@Key ,@ExactOnlineEndpoint ,@EventCreatedOn );";

            var param = new
            {
                Id = Guid.NewGuid(),
                exactOnline.Action,
                exactOnline.Division,
                exactOnline.Key,
                exactOnline.Topic,
                exactOnline.EventCreatedOn,
                exactOnline.ExactOnlineEndpoint
            };

            await using (var connection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(query, param);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}