using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.WF;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IWorkFlowRepository
{
    Task<IEnumerable<WFShortCutPaneDto>> GetShortcutpaneData(WFParameter WFParameter);

    Task<IEnumerable<WFListDto>> GetWFList(WFParameter WFParameter);

    Task<string> CreateHeader(WFParameter WFParameter);

    Task<WFDropDownData> GetWFDropdown(WFParameter WFParameter);
    Task<WFHeaderDto> GetWFById(WFParameter WFParameter);
    Task<string> CUApprove(WFParameter WFParameter);
    Task<string> ProjectApprove(WFParameter WFParameter);
}

public class WFParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public WFCreateDto WFDto { get; set; }
    public string UserId { get; set; }
    public WFFilter Filter { get; set; }
}