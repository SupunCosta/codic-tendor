using System;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Data.Contract;

public class ContractHistoryLog
{
    public string Id { get; set; }
    public string HistoryId { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }
    public string RevisionNumber { get; set; }


    public ContractHeader Contract { get; set; }
    [ForeignKey("Contract")] public string ContractId { get; set; }

    public CabPersonCompany CabPersonCompany { get; set; }
    [ForeignKey("CabPersonCompany")] public string CabPersonCompanyId { get; set; }
}