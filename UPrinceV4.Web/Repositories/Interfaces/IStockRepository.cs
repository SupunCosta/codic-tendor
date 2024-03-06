using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Stock;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IStockRepository
{
    Task<StockShortCutPaneCommon> GetShortcutpaneData(StockParameter StockParameter);

    Task<IEnumerable<StockListDto>> GetStockList(StockParameter StockParameter);

    Task<string> CreateHeader(StockParameter StockParameter);

    Task<StockDropDownData> GetStockDropdown(StockParameter StockParameter);
    Task<StockHeaderDto> GetStockById(StockParameter StockParameter);
}

public class StockParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public StockCreateDto StockDto { get; set; }
    public string UserId { get; set; }
    public StockFilter Filter { get; set; }
}