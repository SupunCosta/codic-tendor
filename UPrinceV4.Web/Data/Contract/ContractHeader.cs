using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.PL;

namespace UPrinceV4.Web.Data.Contract;

public class ContractHeader
{
    public string Id { get; set; }
    public string ContractId { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string TermsAndConditions { get; set; }

    public ContractType ContractType { get; set; }
    [ForeignKey("ContractType")] public string TypeId { get; set; }

    public ContractStatus ContractStatus { get; set; }
    [ForeignKey("ContractStatus")] public string StatusId { get; set; }

    public Currency Currency { get; set; }
    [ForeignKey("Currency")] public int CurrencyId { get; set; }

    public List<PLPriceList> PLPriceList { get; set; }

    public List<ContractHeaderParties> ContractHeaderParties { get; set; }
    public string InvolvePartyId { get; set; }
}