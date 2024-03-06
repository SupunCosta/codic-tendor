using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ManagerCPCRepository : IManagerCPCRepository
{
    public async Task<IEnumerable<ProjectWithPM>> ReadCPCByProjectsPM(ManagerCPCParameter managerCPCParameter)

    {
        var projectWithPmList = ProjectPm(managerCPCParameter.TenantProvider.GetTenant().ConnectionString);

        foreach (var project in projectWithPmList)
            if (project != null && project.ProjectConnectionString != null)
            {
                const string query = @"SELECT
                                          CpcResourceType.Name as ResourceName
                                         ,CorporateProductCatalog.ResourceTitle
                                         ,CorporateProductCatalog.ResourceNumber
                                       FROM dbo.CorporateProductCatalog
                                      INNER JOIN dbo.CpcResourceType
                                        ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id
                                         where IsDeleted = 0";
                var sb = new StringBuilder(query);

                var connectionString = project.ProjectConnectionString;
                await using var connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    project.CPC = connection
                        .Query<CpcListDto>(sb.ToString(), new { lang = managerCPCParameter.Lang }).ToList();
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

        return projectWithPmList;
    }

    public IEnumerable<ProjectWithPM> ProjectPm(string connection)
    {
        const string projectWithPm = @"SELECT
                                                   ProjectTeam.ProjectId
                                                 ,ProjectTeam.ContractingUnitId
                                                 ,ProjectTeamRole.CabPersonId
                                                 ,ProjectDefinition.SequenceCode
                                                 ,ProjectDefinition.Name
                                                 ,CabPerson.FullName
                                                 ,ProjectDefinition.ProjectConnectionString
                                                 ,ProjectDefinition.Title AS ProjectTitle
                                                FROM dbo.ProjectTeamRole
                                                INNER JOIN dbo.ProjectTeam
                                                  ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                                                INNER JOIN dbo.ProjectDefinition
                                                  ON ProjectTeam.ProjectId = ProjectDefinition.Id
                                                INNER JOIN dbo.CabPerson
                                                  ON ProjectTeamRole.CabPersonId = CabPerson.Id
                                                WHERE ProjectTeamRole.RoleId IN ('1666e217-2b80-4acd-b48b-b041fe263fb9', '476127cb-62db-4af7-ac8e-d4a722f8e142','266a5f47-3489-484b-8dae-e4468c5329dn3')";

        IEnumerable<ProjectWithPM> projectWithPmList = null;
        using var connectionDb = new SqlConnection(connection);
        try
        {
            connectionDb.Open();
            projectWithPmList = connectionDb.Query<ProjectWithPM>(projectWithPm);
        }
        catch (Exception e)
        {
            // ignored
        }

        return projectWithPmList;
    }
}