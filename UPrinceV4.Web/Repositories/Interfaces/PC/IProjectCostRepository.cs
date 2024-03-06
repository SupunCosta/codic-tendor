using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces.PC;

public interface IProjectCostRepository
{
    public Task<IEnumerable<ProjectCostFilterDto>> Filter(
        ProjectCostRepositoryParameter projectCostRepositoryParameter);
    
    public Task<List<string>> IgnoreProjectCostCbcResources(ProjectCostRepositoryParameter projectCostRepositoryParameter);
    
}

public class ProjectCostRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public ProjectCostFilter ProjectCostFilter { get; set; }
    public string Lang { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public List<string> IdList { get; set; }

}