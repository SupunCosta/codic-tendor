using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IManagerCPCRepository
{
    Task<IEnumerable<ProjectWithPM>> ReadCPCByProjectsPM(ManagerCPCParameter managerCPCParameter);
}

public class ManagerCPCParameter
{
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public IHttpContextAccessor ContextAccessor { get; set; }

    //public ApplicationDbContext ApplicationDbContext { get; set; }
}