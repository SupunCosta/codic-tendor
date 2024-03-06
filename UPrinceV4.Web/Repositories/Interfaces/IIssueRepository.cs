using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Issue;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IIssueRepository
{
    Task<string> CreateIssue(IssueParameter issueParameter);
    Task<List<IssueFilterResults>> IssueFilter(IssueParameter issueParameter);
    Task<IssueHeaderCreateDto> IssueGetById(IssueParameter issueParameter);
    Task<IssueDropDownData> GetIssueDropDownData(IssueParameter issueParameter);
    Task<List<string>> DeleteIssueDocuments(IssueParameter issueParameter);

}

public class IssueParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public List<string> IdList { get; set; }

    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public IssueHeaderCreateDto IssueHeader { get; set; }
    public IssueFilterDto IssueFilterDto { get; set; }
    public IWbsRepository iWbsRepository { get; set; }

}