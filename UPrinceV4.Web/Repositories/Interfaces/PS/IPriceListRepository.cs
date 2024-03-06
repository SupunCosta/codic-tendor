using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PC;

namespace UPrinceV4.Web.Repositories.Interfaces.PS;

public interface IPriceListRepository
{
    Task<string> CreateResourceTypePriceList(PriceListParameter parameter);
    Task<ResourceTypePriceList> ReadResourceTypePriceList(PriceListParameter parameter);
    Task<string> CreateResourceItemPriceList(PriceListParameter parameter);
    Task<IEnumerable<ResourceItemPriceListReadDto>> ReadMaterialPriceList(PriceListParameter parameter);
    Task<IEnumerable<ResourceItemPriceListReadDto>> ReadConsumablePriceList(PriceListParameter parameter);
    Task<IEnumerable<ResourceItemPriceListReadDto>> ReadLabourPriceList(PriceListParameter parameter);
    Task<IEnumerable<ResourceItemPriceListReadDto>> ReadToolPriceList(PriceListParameter parameter);
    Task<string> DeleteResourceItemPriceList(PriceListParameter parameter);
}

public class PriceListParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Id { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public ResourceTypePriceListCreateDto ResourceTypePriceListCreateDto { get; set; }
    public string UserId { get; set; }
    public ResourceItemPriceList ResourceItemPriceListCreateDto { get; set; }
    public List<string> idList { get; set; }
}