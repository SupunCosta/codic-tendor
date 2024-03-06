using System;

namespace UPrinceV4.Web.Data;

public class RiskHistoryLog : HistoryMetaData
{
    public string RiskId { get; set; }
}

public class RiskHistoryLogDto
{
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string UpdatedByUser { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public int RevisionNumber { get; set; }
}

public class RiskHistoryLogDapperDto
{
    public string Oid { get; set; }
    public string User { get; set; }
    public DateTime? DateTime { get; set; }
    public int RevisionNumber { get; set; }
    public DateTime? UpdatedDateTime { get; set; }

    public string UpdatedByUser { get; set; }

    public string CreatedByUser { get; set; }
    public DateTime? CreatedDateTime { get; set; }
}