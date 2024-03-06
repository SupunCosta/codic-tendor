using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PO;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IWorkflowStateRepository
{
    Task<IEnumerable<WorfFlowStatusLocalizedDto>> GetWorkflowStates(ApplicationDbContext applicationDbContext, string lang,ITenantProvider iTenantProvider);
    Task<WorkflowState> GetWorkflowStateById(ApplicationDbContext applicationDbContext, int id, string lang);
    bool DeleteWorkflowStateType(ApplicationDbContext applicationDbContext, int id);
}