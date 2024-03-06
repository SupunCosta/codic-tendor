using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Contract;

namespace UPrinceV4.Web.Data.PL;

public class PLPriceList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public PLType PLType { get; set; }
    [ForeignKey("PLType")] public string TypeId { get; set; }

    public CabPersonCompany CabPersonCompany { get; set; }
    [ForeignKey("CabPersonCompany")] public string CabPersonCompanyId { get; set; }

    public PLStatus PLStatus { get; set; }
    [ForeignKey("PLStatus")] public string StatusId { get; set; }

    public Currency Currency { get; set; }
    [ForeignKey("Currency")] public int CurrencyId { get; set; }

    public List<PLHistoryLog> PLHistoryLogs { get; set; }
    [ForeignKey("PLHistoryLog")] public string HistoryLogId { get; set; }

    public List<PLListsItems> PLListItems { get; set; }
    [ForeignKey("PLListItem")] public string PriceItemId { get; set; }

    public ContractHeader ContractHeader { get; set; }
    [ForeignKey("ContractHeader")] public string ContractId { get; set; }
}