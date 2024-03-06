using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories;

public class ForemanMigrationRepository : IForemanMigrationRepository
{
    public async Task<string> ForemanAddToPmolTeam(ForemanMigrationParameter ForemanMigrationParameter)
    {
        // var connectionString = ConnectionString.MapConnectionString(
        //     ForemanMigrationParameter.ContractingUnitSequenceId,
        //     ForemanMigrationParameter.ProjectSequenceId, ForemanMigrationParameter.TenantProvider);
        //
        // await using var connection = new SqlConnection(connectionString);

        await using var dbConnection =
            new SqlConnection(ForemanMigrationParameter.TenantProvider.GetTenant().ConnectionString);

        var projects = dbConnection.Query<ProjectDefinition>(
            "Select * From ProjectDefinition Where IsDeleted = 0");

        foreach (var project in projects)
        {
            await using var connection = new SqlConnection(project.ProjectConnectionString);
            var pmolList = connection.Query<PmolData>(
                @"SELECT PMol.Id ,PMol.ProjectMoleculeId ,PMol.Name ,PMol.ForemanMobileNumber ,PMol.ExecutionDate ,PMol.ForemanId ,PMol.Comment ,PMol.IsDeleted ,PMol.TypeId ,PMol.StatusId ,PMol.Title ,PMol.BorId ,PMol.EndDateTime ,PMol.IsFinished ,PMol.StartDateTime ,PMol.IsBreak ,PMol.PmolType ,PMol.ProductId ,PMol.ProjectSequenceCode ,PMol.ParentId ,PMol.ExecutionEndTime ,PMol.ExecutionStartTime ,PMol.LocationId ,PbsProduct.Name AS ProductName FROM dbo.PMol LEFT OUTER JOIN dbo.PbsProduct ON PMol.ProductId = PbsProduct.Id WHERE PMol.IsDeleted = 0 ");

            foreach (var pmol in pmolList)
                if (pmol.ForemanId == null)
                {
                    var pmolLabourItems = connection.Query<PmolTeamRole>(
                            @"SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.RoleId = 'Foreman' AND PmolTeamRole.IsDeleted = 0 ",
                            new { PmolId = pmol.Id })
                        .FirstOrDefault();

                    if (pmolLabourItems != null)
                        await connection.ExecuteAsync("Update PmolTeamRole Set IsDeleted = 1 WHERE Id = @Id",
                            new { pmolLabourItems.Id });
                }
                else
                {
                    var pmolParameter = new PmolParameter();
                    pmolParameter.PmolDto = new PmolCreateDto();
                    pmolParameter.PmolDto.ForemanId = pmol.ForemanId;
                    pmolParameter.PmolDto.Id = pmol.Id;
                    pmolParameter.Id = pmol.Id;
                    pmolParameter.ProjectSequenceId = project.SequenceCode;
                    pmolParameter.TenantProvider = ForemanMigrationParameter.TenantProvider;
                    pmolParameter.Lang = ForemanMigrationParameter.Lang;
                    pmolParameter.Configuration = ForemanMigrationParameter.Configuration;
                    ForemanMigrationParameter.PmolRepository.ForemanAddToPmol(pmolParameter,
                        project.ProjectConnectionString, true);
                }
        }

        return "ok";
    }
}