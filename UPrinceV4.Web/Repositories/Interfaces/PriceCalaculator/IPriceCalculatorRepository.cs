using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PriceCalculator;

namespace UPrinceV4.Web.Repositories.Interfaces.PriceCalaculator;

public interface IPriceCalculatorRepository
{
    Task<string> CreatePriceCalculatorTaxonomy(PriceCalculatorParameter priceCalculatorParameter);
    Task<List<PriceCalculatorTaxonomy>> GetPriceCalculatorTaxonomy(PriceCalculatorParameter priceCalculatorParameter);

    Task<string> DeletePriceCalculatorTaxonomy(PriceCalculatorParameter priceCalculatorParameter);

    Task<List<GetPriceCalculatorTaxonomyLevel>> GetPriceCalculatorTaxonomyLevels(
        PriceCalculatorParameter priceCalculatorParameter);
}

public class PriceCalculatorParameter
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
    public ApplicationDbContext uPrinceCustomerContext { get; set; }
    public IFormFile File { get; set; }
    public CreatePriceCalculatorTaxonomy CreatePriceCalculatorTaxonomy { get; set; }
}