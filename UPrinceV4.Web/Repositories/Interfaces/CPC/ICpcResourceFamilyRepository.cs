using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.CPC;

public interface ICpcResourceFamilyRepository
{
    Task<List<DatabasesEx>> CreateCpcResourceFamily(CpcResourceFamilyParameters cpcResourceFamilyParameters);
}

public class CpcResourceFamilyParameters
{
    public ApplicationDbContext Context { get; set; }
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Id { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public CpcResourceFamilyLocalizedData cpcResourceFamily { get; set; }
}