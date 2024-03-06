using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.Contract;

public class ContractHeaderParties

{
    public string Id { get; set; }

    public ContractInvolveParties ContractInvolveParties { get; set; }
    [ForeignKey("ContractInvolveParties")] public string PartyId { get; set; }

    public ContractHeader ContractHeader { get; set; }
    [ForeignKey("ContractHeader")] public string ContractId { get; set; }
}