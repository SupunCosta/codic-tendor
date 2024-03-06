using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.TAX;

namespace UPrinceV4.Web.Repositories.Interfaces.PS;

public interface IVATRepository
{
    List<Tax> VATFilter(VATParameter psParameter);
}

public class VATParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string VATId { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string UserId { get; set; }
}