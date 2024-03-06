using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.CPC;

public interface ICpcVendorRepository
{
    Task<string> UpdateVendor(CpcVendorParameters cpcParameters);
}

public class CpcVendorParameters
{
    public ApplicationDbContext Context { get; set; }
    public CpcVendorCreateDto CpcVendorDto { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public ITenantProvider TenantProvider { get; set; }
}