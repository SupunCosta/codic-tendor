using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.RFQ;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Repositories.Interfaces.RFQ;

public interface IRFQRepository
{
    Task<string> SendRfqEmail(RFQParameter RFQParameter);
    Task<string> AcceptRfqEmail(RFQParameter RFQParameter);
}

public class RFQParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> IdList { get; set; }
    public IConfiguration Configuration { get; set; }
    public IPbsResourceRepository PbsResourceRepository { get; set; }
    public IBorRepository BorRepository { get; set; }
    public IProjectDefinitionRepository ProjectDefinitionRepository { get; set; }
    public ApplicationDbContext uPrinceCustomerContext { get; set; }
    public IShiftRepository _iShiftRepository { get; set; }
    public RfqAccept RfqAccept { get; set; }
    public IFormFile File { get; set; }
}