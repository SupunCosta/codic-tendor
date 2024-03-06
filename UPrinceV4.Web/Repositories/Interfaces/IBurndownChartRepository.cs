using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.BurndownChart;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IBurndownChartRepository
{
    Task <List<GetBurndownChart>> GetBurnDownChartData(BurndownChartParameter BurndownChartParameter);

}

public class BurndownChartParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    
    public BurndownChartDto BurndownChartDto { get; set; }

}