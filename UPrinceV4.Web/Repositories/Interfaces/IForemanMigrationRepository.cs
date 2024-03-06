using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IForemanMigrationRepository
{
    Task<string> ForemanAddToPmolTeam(ForemanMigrationParameter ForemanMigrationParameter);
}

public class ForemanMigrationParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Id { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string AuthToken { get; set; }
    public string UserId { get; set; }
    public List<string> IdList { get; set; }
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public IConfiguration Configuration { get; set; }
    public IPmolRepository PmolRepository { get; set; }
}