using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;


public class WeTransferController:Controller
{
    private readonly ITenantProvider _TenantProvider;
    private IConfiguration _iConfiguration { get; }


    public WeTransferController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IConfiguration iConfiguration)
        

    {
        
        _TenantProvider = tenantProvider;
        _iConfiguration = iConfiguration;
        
    }
    
    
    [HttpGet("doc/{param}")]
    public async Task<ActionResult> RedirectShortUrl(string param)
    {
        try
        {
            
            await using var dbconnection = new SqlConnection(_TenantProvider.GetTenant().ConnectionString);

            var thFile = dbconnection.Query<WeTransfer>("Select * from WeTransfer WHERE ShortUrlId = @ShortUrlId", new{ ShortUrlId = param})
                .FirstOrDefault();

            if (thFile != null) 
            {
                return Redirect(thFile?.Link);
                
            }

            throw new Exception("File Not Found");
            
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}