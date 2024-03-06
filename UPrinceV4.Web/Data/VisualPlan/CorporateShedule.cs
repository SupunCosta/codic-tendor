using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.PO;

namespace UPrinceV4.Web.Data.VisualPlan;

public class CorporateShedule : POMetaData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public bool IsDefault { get; set; } = false;
}

public class CorporateSheduleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public List<CorporateSheduleTimeDto> CorporateSheduleTime { get; set; }
    public CorporateSheduleHistory History { get; set; }
    public bool IsDefault { get; set; } = false;
}

public class CorporateSheduleHistory
{
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; } = null;
}

public class CorporateSheduleList
{
    public string Title { get; set; }
    public Sorter Sorter { get; set; }
}