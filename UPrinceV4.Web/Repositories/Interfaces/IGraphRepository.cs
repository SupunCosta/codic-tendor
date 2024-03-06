using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IGraphRepository
{
    Task<bool> SendInvitation(GraphParameter GraphParameter);
}

public class GraphParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public GraphServiceClient GraphServiceClient { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public string ButtonText { get; set; }
}