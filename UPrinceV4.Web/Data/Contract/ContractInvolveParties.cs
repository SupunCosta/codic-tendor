using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data.Contract;

public class ContractInvolveParties
{
    public string id { get; set; }

    public string PartyId { get; set; }

    public CabPersonCompany CabPersonCompany { get; set; }
    [ForeignKey("CabPersonCompany")] public string CabPersonCompanyId { get; set; }

    public CabCompany CabCompany { get; set; }
    [ForeignKey("CabCompany")] public string CompanyId { get; set; }

    public List<ContractHeaderParties> ContractHeaderParties { get; set; }
    public string ContractId { get; set; }
}