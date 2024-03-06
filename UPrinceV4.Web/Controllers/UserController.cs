using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class UserController : CommonConfigurationController
{
   
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ITenantProvider _tenantProvider;

    public UserController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        GraphServiceClient graphServiceClient)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _tenantProvider = tenantProvider;
        _graphServiceClient = graphServiceClient;
    }


    [HttpGet("getRoles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getRoles()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            // var query =
            //     @"select RoleId AS Id, RoleName from Role where LanguageCode=@lang AND RoleId NOT in ('910b7af0-b132-4951-a2dc-6ab82d4cd40d','266a5f47-3489-484b-8dae-e4468c5329dn3','yyyyyyy-a513-45e0-a431-170dbd4yyyy') ";

            var parameters = new { lang = lang };
            // await using (var connection = new SqlConnection(_tenantProvider.GetTenant().ConnectionString))
            // {
            //     return Ok(new ApiOkResponse(await connection.QueryAsync<Roles>(query, parameters), "Ok"));
            // }
            return Ok(new ApiOkResponse(
                await _tenantProvider.orgSqlConnection()
                    .QueryAsync<Roles>("getRoles", parameters, commandType: CommandType.StoredProcedure), "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login()
    {
        try
        {
            var options1 = new DbContextOptions<UserDbContext>();
            var context =
                new UserDbContext(options1,_tenantProvider);
            string Email;
            // var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            //     claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var PEmail = ContextAccessor.HttpContext?.User.Identities.First().Claims.Where(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            if (PEmail.Any())
                Email = PEmail.FirstOrDefault().Value;
            else
                Email = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").Value;
            var user = context.ApplicationUser.Where(u => u.Email == Email);
            var createUser = new CreateUser(ContextAccessor, _tenantProvider,context);
            var dbConnection = ItenantProvider.orgSqlConnection();
            ApplicationUser existingUser = null;
            if (!user.Any())
                existingUser = createUser.createUser();
            else
                existingUser = createUser.UpdateUser(user.FirstOrDefault());

            //var tenant = ItenantProvider.GetTenant();
            var emailAddress = existingUser.Email;
            string cabPersonCompanyId = null;
            var emilParam = new { emailAdd = emailAddress };
            var cabPersonCompanyQuery = @"SELECT
                                              CabPersonCompany.Id
                                            FROM dbo.CabPersonCompany
                                            INNER JOIN dbo.CabEmail
                                              ON CabPersonCompany.EmailId = CabEmail.Id
                                            WHERE CabEmail.EmailAddress = @emailAdd";
    
            cabPersonCompanyId = dbConnection.Query<string>(cabPersonCompanyQuery, emilParam).FirstOrDefault();
            
            if (!string.IsNullOrEmpty(cabPersonCompanyId))
            {
                var oidParam = new { cabId = cabPersonCompanyId, oid = existingUser.OId };
                var sb = @"update dbo.CabPersonCompany set Oid = @oid where Id = @cabId";
                var affectedRows = dbConnection.ExecuteAsync(sb, oidParam).Result;
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var loginDetailsDto = new UserLoginDetailsDto();
            var sql = @"SELECT
                              CabCompany.Id AS Id ,CabCompany.Name AS Name  
                              ,CabCompany.SequenceCode AS SequenceCode  ,CabCompany.SequenceCode
                               AS Title
                            FROM dbo.CabCompany
                            LEFT OUTER JOIN dbo.ContractingUnitUserRole
                              ON CabCompany.Id = ContractingUnitUserRole.CabCompanyId
                            LEFT OUTER JOIN dbo.UserRole
                              ON ContractingUnitUserRole.UserRoleId = UserRole.Id
                            LEFT OUTER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserId = ApplicationUser.Id
                            WHERE IsContractingUnit = 1 AND ApplicationUser.OId = '" + existingUser.OId + "'";

            IEnumerable<ContractingUnitDto> result = null;
            result = dbConnection.Query<ContractingUnitDto>(sql).ToList();
            loginDetailsDto.ContractingUnits = result.ToList();
            existingUser = UPrinceCustomerContext.ApplicationUser.FirstOrDefault(u => u.Email == Email);
            loginDetailsDto.User = existingUser;
            var sqlnew =
                @"SELECT DISTINCT ProjectDefinition.Name AS Name  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Id AS ProjectDefinitionId  ,ProjectTemplate.Name AS ProjectTemplateName  ,ProjectToleranceState.Name AS ProjectToleranceStateName  ,ProjectType.Name AS ProjectTypeName  ,ProjectManagementLevel.Name AS ProjectManagementLevelName, ProjectDefinition.Title AS Title FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectTemplate   ON ProjectDefinition.ProjTemplateId = ProjectTemplate.TemplateId LEFT OUTER JOIN dbo.ProjectToleranceState   ON ProjectDefinition.ProjToleranceStateId = ProjectToleranceState.ProjectToleranceStateId 
                     LEFT OUTER JOIN dbo.ProjectType   ON ProjectDefinition.ProjTypeId = ProjectType.ProjectTypeId LEFT OUTER JOIN dbo.ProjectManagementLevel 
                       ON ProjectDefinition.ProjManagementLevelId = ProjectManagementLevel.ProjectManagementLevelId 
                    LEFT OUTER JOIN dbo.ProjectUserRole ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId 
                     LEFT OUTER JOIN dbo.UserRole ON ProjectUserRole.UsrRoleId = UserRole.Id 
                     LEFT OUTER JOIN dbo.ApplicationUser ON UserRole.ApplicationUserOid = ApplicationUser.Oid 
                    WHERE (ProjectTemplate.LanguageCode = @lang OR ProjectDefinition.ProjTemplateId IS NULL) AND ProjectToleranceState.LanguageCode = @lang
                    AND ProjectType.LanguageCode = @lang AND ProjectManagementLevel.LanguageCode = @lang
                    AND ApplicationUser.OId = @Oid AND ProjectDefinition.IsDeleted = 0";
            
                loginDetailsDto.Projects = dbConnection
                    .Query<AllProjectAttributes>(sqlnew, new { lang, Oid = user.FirstOrDefault().OId })
                    .ToList();
                
            var group = await _graphServiceClient.Me.MemberOf
                .GetAsync();
            loginDetailsDto.groups = group.Value;
            
            return Ok(new ApiOkResponse(loginDetailsDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    
    [HttpPost("createUserRoles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> createUserRoles([FromBody] UserRoleRegistration userRole)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.Result = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var role = new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserOid = userRole.UserId,
                RoleId = userRole.RoleId
            };
            
            var options1 = new DbContextOptions<UserDbContext>();
            var context =
                new UserDbContext(options1,_tenantProvider);
            context.Add(role);
            await context.SaveChangesAsync();
            ApiOkResponse.Result = "Ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpPost("createRoles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> createRoles([FromBody] RolesCreateDto role)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var r = new Roles { Id = Guid.NewGuid().ToString(), RoleName = role.RoleName, TenantId = role.TenantId };
            var options1 = new DbContextOptions<UserDbContext>();
            var context =
                new UserDbContext(options1,_tenantProvider);
            context.Role.Add(r);
            await context.SaveChangesAsync();
            ApiOkResponse.Result = r.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

   
    [HttpPost("UpdateRoles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> UpdateRoles([FromBody] RolesUpdateDto role)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ApiBadRequestResponse(ModelState));

            var r = new Roles { Id = role.Id, RoleName = role.RoleName, TenantId = role.TenantId };
            UPrinceCustomerContext.Role.Update(r);
            await UPrinceCustomerContext.SaveChangesAsync();
            ApiOkResponse.Result = r.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    
    [HttpDelete("DeleteUser/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteUser(string id)
    {
        try
        {
            var options1 = new DbContextOptions<UserDbContext>();
            var context =
                new UserDbContext(options1,_tenantProvider);
            var user = (from a in context.ApplicationUser
                where a.Id == id
                select a).Single();
            context.ApplicationUser.Remove(user);
            context.SaveChanges();
            ApiResponse.StatusCode = 204;
            ApiResponse.Status = true;
            ApiOkResponse.Message = "User deleted successfully";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    
    [HttpDelete("DeleteRole/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteRole(string id)
    {
        try
        {
            var options1 = new DbContextOptions<UserDbContext>();
            var context =
                new UserDbContext(options1,_tenantProvider);
            var role = (from a in context.Role
                where a.Id == id
                select a).Single();
            context.Role.Remove(role);
            context.SaveChanges();
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = true;
            ApiResponse.Message = "Role deleted successfully";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("GraphMe")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GraphMe()
    {
        //  HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

        var userProfile = await _graphServiceClient.Me
            //.Select(x => new
            //{
            //    x.Country,
            //    x.City,
            //    x.EmployeeId,
            //    x.DisplayName,
            //    x.GivenName,
            //    x.Department
            //    x.gro
            //})
            .GetAsync();

        return Ok(new ApiOkResponse(userProfile, "Ok"));

        //  return JsonConvert.SerializeObject(userProfile);
    }

    [HttpGet("GraphGroupBelongsTo")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GraphGroup()
    {
        // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

        //var requestOptions = new List<Option>();

        //requestOptions.Add(new QueryOption("$count", "true"));

        var group = await _graphServiceClient.Me.TransitiveMemberOf
            .GetAsync();

        return Ok(new ApiOkResponse(group.Value, "Ok"));

        //  return JsonConvert.SerializeObject(userProfile);
    }

    // [Route("GraphGroupMembers")]
    // [HttpGet]
    // [HttpGet]
    // [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult> GraphGroupMembers()
    // {
    //     // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
    //
    //     var groupIds = new List<string>
    //     {
    //         "631c174d-b2dd-45c9-9cb4-541f119b5834"
    //     };
    //
    //     var group = await _graphServiceClient.Groups["{id}"]
    //         .CheckMemberGroups(groupIds)
    //         .Request()
    //         .PostAsync();
    //
    //     return Ok(new ApiOkResponse(group, "Ok"));
    //
    //     //  return JsonConvert.SerializeObject(userProfile);
    // }


    [HttpGet("GraphUserPhoto")]
    public async Task<string> GetMePhotoAsync()
    {
        try
        {
            // GET /me
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            // var requestOptions = new List<Option>();
            //
            // requestOptions.Add(new QueryOption("$count", "true"));


            var stream = await _graphServiceClient.Me.Photo.Content.GetAsync();


            if (stream != null)
            {
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                var buffer = ms.ToArray();
                var resultI = Convert.ToBase64String(buffer);
                var imgDataURL = string.Format("data:image/png;base64,{0}", resultI);

                return JsonConvert.SerializeObject(imgDataURL);
            }

            return null;
        }
        catch (ServiceException ex)
        {
            Console.WriteLine($"Error getting signed-in user profilephoto: {ex.Message}");
            return null;
        }
    }
}