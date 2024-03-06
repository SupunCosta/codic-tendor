using System;

namespace UPrinceV4.Web.Data;

public class QualityHistoryLog : HistoryMetaData
{
    public string QualityId { get; set; }
}

public class QualityHistoryLogDto
{
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string UpdatedByUser { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public int RevisionNumber { get; set; }
}

public class QualityHistoryLogDapperDto
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