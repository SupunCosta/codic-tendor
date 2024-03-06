using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.INV;
using UPrinceV4.Web.Data.PS;

namespace UPrinceV4.Web.Repositories.Interfaces.INV;

public interface IInvoiceRepository
{
    public Task<PsDropdownData> GetDropdownData(InvoiceParameter parameter);
    Task<string> CreateInvoice(InvoiceParameter parameter);
    Task<IEnumerable<InvoiceFilterDto>> InvoiceFilter(InvoiceParameter parameter);
    Task<Invoice> ReadInVoiceById(InvoiceParameter parameter);

    Task<string> CUApproveInvoice(InvoiceParameter parameter);
    Task<string> CUInReviewInvoice(InvoiceParameter parameter);
}

public class InvoiceParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string Id { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string UserId { get; set; }
    public InvoiceFilter filter { get; set; }
    public string InvoiceId { get; set; }
}