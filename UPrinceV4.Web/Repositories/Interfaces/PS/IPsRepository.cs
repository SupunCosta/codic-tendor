using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces.PS;

public interface IPsRepository
{
    public Task<PsDropdownData> GetDropdownData(PmolParameter pmolParameter);
    Task<string> GetPsTitleById(PsParameter psParameter);
    Task<PsHeader> CreatePsHeader(PsParameter psParameter);
    Task<IEnumerable<PsFilterReadDto>> PsFilter(PsParameter psParameter);
    Task<PsHeaderReadDto> ReadPsById(PsParameter psParameter);
    Task<PsHeaderReadDto> ReadExcel(PsParameter psParameter);

    Task<string> CreatePsResource(PsParameter psParameter);
    Task<string> ApprovePs(PsParameter parameter);
    Task<string> CUApprovePs(PsParameter parameter);
    public Task<PsCustomerReadDto> GetCustomer(PmolParameter pmolParameter);
}

public class PsParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string PsId { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string UserId { get; set; }
    public PsHeaderCreateDto PsHeaderCreateDto { get; set; }
    public PsFilter Filter { get; set; }
    public PsResourceCreateDto PsResourceCreateDto { get; set; }

    public string GeneralLedgerId { get; set; }
}