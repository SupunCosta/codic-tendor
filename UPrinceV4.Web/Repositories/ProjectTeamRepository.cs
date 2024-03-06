using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectTeamRepository : IProjectTeamRepository
{
    public async Task<string> CreateProjectTeam(
        ProjectTeamCreateDto projectTeamCreateDto, ITenantProvider iTenantProvider, ApplicationUser user)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, iTenantProvider);
        
        string id = null;
        if (projectTeamCreateDto.ContractingUnitId != null)
        {
            var project =
                applicationDbContext.ProjectTeam.FirstOrDefault(p => p.ProjectId == projectTeamCreateDto.ProjectId);
            if (project == null)
            {
                var newproject = new ProjectTeam();
                newproject.ProjectId = projectTeamCreateDto.ProjectId;
                newproject.ContractingUnitId = projectTeamCreateDto.ContractingUnitId;
                newproject.Id = Guid.NewGuid().ToString();
                applicationDbContext.ProjectTeam.Add(newproject);
                applicationDbContext.SaveChanges();
                id = newproject.Id;
            }
            else
            {
                project.ProjectId = projectTeamCreateDto.ProjectId;
                project.ContractingUnitId = projectTeamCreateDto.ContractingUnitId;
                applicationDbContext.ProjectTeam.Update(project);
                applicationDbContext.SaveChanges();
                id = project.Id;
            }

            if (project == null)
            {
                var role = applicationDbContext.Role.FirstOrDefault(r =>
                    r.RoleId.Equals("0e06111a-a513-45e0-a431-170dbd4b0d82"));
                var email = user.Email;
                var CabEmail =
                    applicationDbContext.CabEmail.FirstOrDefault(e => e.EmailAddress.Equals(user.Email));
                if (CabEmail != null)
                {
                    var cabPersonCompany =
                        applicationDbContext.CabPersonCompany.FirstOrDefault(p =>
                            p.EmailId.Equals(CabEmail.Id));
                    if (cabPersonCompany != null)
                    {
                        
                        
                            var createdUser = new ProjectTeamRoleCreateDto
                            {
                                RoleId = role?.RoleId,
                                Email = email,
                                CabPersonId = cabPersonCompany.PersonId,
                                status = "1",
                                IsAccessGranted = true
                            };
                            projectTeamCreateDto.TeamRoleList.Add(createdUser);
                           
                        
                        
                    }
                }
                
                await using var dbconnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);

                var defaultMembers = dbconnection.Query<ProjectTeamRoleCreateDto>(
                    @"SELECT CabPersonId,'4837043c-119c-47e1-bbf2-1f32557fdf30' AS RoleId,'1' AS Status ,1 AS IsAccessGranted, ce.EmailAddress AS Email FROM ProjectDefaultMembers
                         LEFT OUTER JOIN CabPersonCompany ON ProjectDefaultMembers.CabPersonId = CabPersonCompany.PersonId
                         LEFT OUTER JOIN CabEmail ce ON CabPersonCompany.EmailId = ce.Id").ToList();

                defaultMembers.RemoveAll(x =>
                    projectTeamCreateDto.TeamRoleList.Select(v => v.CabPersonId).Contains(x.CabPersonId));
                projectTeamCreateDto.TeamRoleList.AddRange(defaultMembers);
            }

            await CreateTeamRoleAsync(projectTeamCreateDto, id, iTenantProvider, projectTeamCreateDto.ProjectId,
                user);
        }

        return id;
    }

    public async Task DeleteProjectTeamRole(ProjectTeamRoleParameter parameter)
    {
        await using var connection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString);
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, parameter.TenantProvider);
        foreach (var id in parameter.IdList)
            if (id != null)
            {
                var role = (from a in applicationDbContext.ProjectTeamRole
                    where a.Id == id
                    select a).FirstOrDefault();

                var projectTeamRole = applicationDbContext.ProjectTeamRole.Where(t => t.Id == id)
                    .Include(t => t.ProjectTeam)
                    .Include(t => t.CabPerson).ThenInclude(p => p.PersonCompanyList)
                    .FirstOrDefault();

                var userRole = applicationDbContext.UserRole
                    .FirstOrDefault(u => u.ApplicationUserOid ==
                                         projectTeamRole.CabPerson.PersonCompanyList.FirstOrDefault().Oid &&
                                         u.RoleId == projectTeamRole.RoleId);

                if (userRole != null)
                {
                    var projectUserRole = applicationDbContext.ProjectUserRole.FirstOrDefault(p =>
                        p.ProjectDefinitionId == projectTeamRole.ProjectTeam.ProjectId &&
                        p.UsrRoleId == userRole.Id);

                    if (projectUserRole != null)
                    {
                        applicationDbContext.ProjectUserRole.Remove(projectUserRole);
                        applicationDbContext.SaveChanges();
                    }
                }

                applicationDbContext.ProjectTeamRole.Remove(role);
                applicationDbContext.SaveChanges();

                switch (role.RoleId)
                {
                    case "910b7af0-b132-4951-a2dc-6ab82d4cd40d":
                        await connection.ExecuteAsync(
                            "Update ProjectDefinition Set CustomerId = null Where SequenceCode = @SequenceCode",
                            new { SequenceCode = parameter.ProjectSequenceCode });
                        break;
                    case "266a5f47-3489-484b-8dae-e4468c5329dn3":
                        await connection.ExecuteAsync(
                            "Update ProjectDefinition Set ProjectManagerId = null Where SequenceCode = @SequenceCode",
                            new { SequenceCode = parameter.ProjectSequenceCode });
                        break;
                    case "yyyyyyy-a513-45e0-a431-170dbd4yyyy":
                        await connection.ExecuteAsync(
                            "Update ProjectDefinition Set SiteManagerId = null Where SequenceCode = @SequenceCode",
                            new { SequenceCode = parameter.ProjectSequenceCode });
                        break;
                }
            }
    }

    public async Task<IEnumerable<CabCompanyDto>> GetContractingUnit(ProjectTeamRoleParameter parameter)
    {
        try
        {
            var sb =
                new StringBuilder(
                    "select Id AS [Key], Name from CabCompany where IsDeleted = 0 AND IsContractingUnit = 1 AND  Name like  '%" +
                    parameter.Name + "%' Order by Name asc");
            using (var connection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
            {
                // IEnumerable<CabCompanyDto> result = connection.Query<CabCompanyDto>(sb.ToString()).ToList();
                return connection.QueryAsync<CabCompanyDto>(sb.ToString()).Result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<ProjectTeamRoleReadDto>> GetProjectTeam(ProjectTeamRoleParameter parameter)
    {
        try
        {
            var query =
                @"select ProjectTeamRole.Id, CabPerson.FullName AS CabPersonName, Role.RoleName, ProjectTeamRole.status from ProjectTeamRole 
                                LEFT OUTER JOIN CabPerson on ProjectTeamRole.CabPersonId = CabPerson.Id
                                LEFT OUTER JOIN Role on ProjectTeamRole.RoleId = Role.Id
                                LEFT OUTER JOIN ProjectTeam on ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                                WHERE ProjectTeam.ProjectId = @id";
            var parameters = new { id = parameter.Id };
            using (var connection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
            {
                IEnumerable<ProjectTeamRoleReadDto> result =
                    connection.Query<ProjectTeamRoleReadDto>(query, parameters).ToList();
                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<Currency> GetProjectTeamById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectTeam>> GetProjectTeams(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateProjectTeam(ApplicationDbContext context,
        ProjectTeamUpdateDto projectTeamUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CabCompanyDto>> GetAllContractingUnit(ProjectTeamRoleParameter parameter)
    {
        var sb =
            new StringBuilder(
                "select Id AS [Key], Name, SequenceCode from CabCompany where IsDeleted = 0 AND IsContractingUnit = 1 ORDER BY SequenceCode");
        await using (var connection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
        {
            return connection.QueryAsync<CabCompanyDto>(sb.ToString()).Result;
        }
    }

    public async Task DeleteProjectAccess(ProjectTeamRoleParameter parameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, parameter.TenantProvider);

        await using var dbConnection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString);

        foreach (var id in parameter.IdList)
            if (id != null)
            {
                var sql = @"DELETE FROM ProjectUserRole WHERE Id = (
                            SELECT
                             ProjectUserRole.Id
                            FROM dbo.ProjectTeamRole
                            LEFT OUTER JOIN dbo.ProjectTeam
                              ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                            LEFT OUTER JOIN dbo.CabPerson
                              ON ProjectTeamRole.CabPersonId = CabPerson.Id
                            LEFT OUTER JOIN dbo.CabPersonCompany
                              ON CabPersonCompany.PersonId = CabPerson.Id
                            LEFT OUTER JOIN dbo.UserRole
                              ON CabPersonCompany.Oid = UserRole.ApplicationUserOid
                                AND ProjectTeamRole.RoleId = UserRole.RoleId
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON UserRole.Id = ProjectUserRole.UsrRoleId
                                AND ProjectTeam.ProjectId = ProjectUserRole.ProjectDefinitionId
                            WHERE ProjectTeamRole.Id = @Id)";

                await dbConnection.ExecuteAsync(sql, new { Id = id });
                
                // var projectTeamRole = applicationDbContext.ProjectTeamRole.Where(t => t.Id == id)
                //     .Include(t => t.ProjectTeam)
                //     .Include(t => t.CabPerson).ThenInclude(p => p.PersonCompanyList)
                //     .FirstOrDefault();
                //
                // if (projectTeamRole.CabPerson.PersonCompanyList.FirstOrDefault() != null)
                // {
                //     
                //     var userRole = applicationDbContext.UserRole
                //         .FirstOrDefault(u => u.ApplicationUserOid ==
                //                              projectTeamRole.CabPerson.PersonCompanyList.FirstOrDefault().Oid &&
                //                              u.RoleId == projectTeamRole.RoleId);
                //
                //     if (userRole != null)
                //     {
                //         var projectUserRole = applicationDbContext.ProjectUserRole.FirstOrDefault(p =>
                //             p.ProjectDefinitionId == projectTeamRole.ProjectTeam.ProjectId &&
                //             p.UsrRoleId == userRole.Id);
                //
                //         if (projectUserRole != null)
                //         {
                //             applicationDbContext.ProjectUserRole.Remove(projectUserRole);
                //             await applicationDbContext.SaveChangesAsync();
                //         }
                //     }
                // }
            }
    }

    public async Task CreateTeamRoleAsync(ProjectTeamCreateDto projectTeamCreateDto, string teamId,
        ITenantProvider iTenantProvider, string projectId, ApplicationUser user)
    {
        // var options = new DbContextOptions<ApplicationDbContext>();
        // var applicationDbContext = new ApplicationDbContext(options, iTenantProvider);
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var applicationDbContext =
            new ProjectDefinitionDbContext(options1,iTenantProvider);

        foreach (var dto in projectTeamCreateDto.TeamRoleList)
        {
            var existingRecord = applicationDbContext.ProjectTeamRole
                .FirstOrDefault(t => t.Id == dto.Id);

            string id = null;

            if (dto.Id == null)
            {
                var role = new ProjectTeamRole
                {
                    CabPersonId = dto.CabPersonId,
                    ProjectTeamId = teamId,
                    RoleId = dto.RoleId,
                    status = dto.status,
                    IsAccessGranted = dto.IsAccessGranted,
                    Message = dto.Message
                };
                if (dto.status.Trim().Equals("1") && dto.IsAccessGranted) role.status = "2";

                role.Id = Guid.NewGuid().ToString();
                var projectTeamExist = applicationDbContext.ProjectTeamRole.FirstOrDefault(p =>
                    p.CabPersonId == dto.CabPersonId && p.ProjectTeamId == teamId);
                if (projectTeamExist == null)
                {
                    applicationDbContext.ProjectTeamRole.Add(role);
                    await applicationDbContext.SaveChangesAsync();
                }


                id = role.Id;

                string userId = null;

                if (dto.status.Trim().Equals("1") && dto.IsAccessGranted)
                {
                    userId = await SendInvitationAsync(dto, iTenantProvider, user);
                    CreateProjectAclRole(dto, iTenantProvider, userId, projectId,
                        projectTeamCreateDto.ContractingUnitId, id);
                }

                if (existingRecord != null)
                    if (existingRecord.IsAccessGranted == false && dto.IsAccessGranted)
                        CreateProjectAclRole(dto, iTenantProvider, userId, projectId,
                            projectTeamCreateDto.ContractingUnitId, id);
            }
            else
            {
                if (existingRecord.IsAccessGranted && dto.IsAccessGranted == false)
                {
                    var parameter = new ProjectTeamRoleParameter
                    {
                        TenantProvider = iTenantProvider
                    };
                    var idList = new List<string> { existingRecord.Id };
                    parameter.IdList = idList;
                    await DeleteProjectAccess(parameter);
                }

                var access = existingRecord.IsAccessGranted;

                existingRecord.CabPersonId = dto.CabPersonId;
                existingRecord.ProjectTeamId = teamId;
                existingRecord.RoleId = dto.RoleId;
                existingRecord.status = dto.status;
                existingRecord.IsAccessGranted = dto.IsAccessGranted;
                existingRecord.Message = dto.Message;
                if (dto.status.Trim().Equals("1") && dto.IsAccessGranted) existingRecord.status = "2";

                existingRecord.Id = dto.Id;
                applicationDbContext.ProjectTeamRole.Update(existingRecord);
                await applicationDbContext.SaveChangesAsync();
                id = existingRecord.Id;

                string userId = null;
                if (dto.status.Trim().Equals("1")) userId = await SendInvitationAsync(dto, iTenantProvider, user);

                if (dto.status.Trim().Equals("1") && dto.IsAccessGranted)
                    CreateProjectAclRole(dto, iTenantProvider, userId, projectId,
                        projectTeamCreateDto.ContractingUnitId, id);

                if (access == false && dto.IsAccessGranted)
                {
                    userId = await SendInvitationAsync(dto, iTenantProvider, user);
                    CreateProjectAclRole(dto, iTenantProvider, userId, projectId,
                        projectTeamCreateDto.ContractingUnitId, id);
                }
            }
        }
    }

    private async Task<string> SendInvitationAsync(ProjectTeamRoleCreateDto projectTeamCreateDto,
        ITenantProvider iTenantProvider, ApplicationUser applicationUser)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, iTenantProvider);

        var person = applicationDbContext.CabPerson.Where(p => p.Id == projectTeamCreateDto.CabPersonId)
            .Include(p => p.Email).Include(p => p.BusinessEmail).FirstOrDefault();

        var query = "select Oid from CabPersoncompany where PersonId ='" + person.Id + "'";
        string result;
        await using (var dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
        {
            result = dbConnection.Query<string>(query).ToList().FirstOrDefault();
        }

        if (!string.IsNullOrEmpty(result)) return result;
        {
            var queryEmail = @"SELECT
                                          CabEmail.EmailAddress AS Email
                                        FROM dbo.CabPersonCompany
                                        INNER JOIN dbo.CabEmail
                                          ON CabPersonCompany.EmailId = CabEmail.Id
                                        WHERE CabPersonCompany.PersonId = '" + person.Id + "'";
            string email;
            await using (var dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
            {
                email = dbConnection.Query<string>(queryEmail).ToList().FirstOrDefault();
            }

            string Oid = null;
            var queryOid = @"SELECT
                                      ApplicationUser.OId AS Oid
                                    FROM dbo.ApplicationUser
                                    WHERE ApplicationUser.Email = '" + email + "'";
            await using (var dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
            {
                Oid = dbConnection.Query<string>(queryOid).ToList().FirstOrDefault();
                
            }

            if (!string.IsNullOrEmpty(Oid))
            {
                var personCompanyEx = applicationDbContext.CabPersonCompany
                    .Where(p => p.PersonId.Equals(person.Id)).FirstOrDefault();
                if (personCompanyEx != null)
                {
                    personCompanyEx.Oid = Oid;
                    applicationDbContext.CabPersonCompany.Update(personCompanyEx);
                    applicationDbContext.SaveChanges();
                }

                return Oid;
            }

            return null;
        }
    }

    private void CreateProjectAclRole(ProjectTeamRoleCreateDto dto, ITenantProvider iTenantProvider, string userId,
        string projectId, string contractingUnitId, string userRoleId)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, iTenantProvider);

        if (dto.RoleId != "910b7af0-b132-4951-a2dc-6ab82d4cd40d") // customer project contact
        {
            var existingUserRole = applicationDbContext.UserRole.FirstOrDefault(u =>
                u.ApplicationUserOid.Equals(userId)
                && u.RoleId.Equals(dto.RoleId));
            string savedUserRoleId = null;

            if (existingUserRole == null)
            {
                var userRole = new UserRole();
                userRole.ApplicationUserOid = userId;
                userRole.Id = userRoleId;
                userRole.RoleId = dto.RoleId;
                applicationDbContext.UserRole.Add(userRole);
                applicationDbContext.SaveChanges();
                savedUserRoleId = userRole.Id;
            }
            else
            {
                savedUserRoleId = existingUserRole.Id;
            }

            var existingProjectUser = applicationDbContext.ProjectUserRole.FirstOrDefault(p =>
                p.UsrRoleId.Equals(savedUserRoleId)
                && p.ProjectDefinitionId.Equals(projectId));
            if (existingProjectUser == null)
            {
                var projectUserRole = new ProjectUserRole();
                projectUserRole.Id = Guid.NewGuid().ToString();
                projectUserRole.ProjectDefinitionId = projectId;
                projectUserRole.UsrRoleId = savedUserRoleId;
                applicationDbContext.ProjectUserRole.Add(projectUserRole);
                applicationDbContext.SaveChanges();
            }

            var existingContractingUser = applicationDbContext.ContractingUnitUserRole.FirstOrDefault(p =>
                p.UserRoleId.Equals(savedUserRoleId)
                && p.CabCompanyId.Equals(contractingUnitId));
            if (existingContractingUser == null)
            {
                var contractingUnitUserRole = new ContractingUnitUserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    CabCompanyId = contractingUnitId,
                    UserRoleId = savedUserRoleId
                };
                applicationDbContext.ContractingUnitUserRole.Add(contractingUnitUserRole);
                applicationDbContext.SaveChanges();
            }
        }
    }
}