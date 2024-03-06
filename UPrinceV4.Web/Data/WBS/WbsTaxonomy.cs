using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.WBS;

public class WbsTaxonomy
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string WbsTaxonomyLevelId { get; set; }
    public string ParentId { get; set; }
    public DateTime? CreatedDateTime{ get; set; }
    public DateTime? UpdatedDateTime{ get; set; }
    public string CreatedBy{ get; set; }
    public string UpdatedBy{ get; set; }
    public bool IsDefault { get; set; } = false;
    public string TemplateId { get; set; }
    public string IssueId { get; set; }
}

public class WbsFilterDto
{ 
    public string Title { get; set; }
    public Sorter Sorter { get; set; }
}

public class WbsTaxonomyList
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceId { get; set; }
    public string Title { get; set; }
    public string WbsTaxonomyLevelId { get; set; }
    public string WbsTaxonomyLevel { get; set; }
    public string ParentId { get; set; }
    public DateTime? CreatedDateTime{ get; set; }
    public DateTime? UpdatedDateTime{ get; set; }
    public string CreatedBy{ get; set; }
    public string UpdatedBy{ get; set; }
    public bool IsDefault { get; set; } = false;
    public string TemplateId { get; set; }
    public string PbsId { get; set; }
    public string ProductId { get; set; }

}

public class WbsDropDownData
{ 
    public IEnumerable<WbsTaxonomyLevelDto> WbsTaxonomyLevel { get; set; }
    public IEnumerable<WbsTaxonomyLevelDto> Status { get; set; }
    public IEnumerable<WbsTaxonomyLevelDto> DeliveryStatus { get; set; }

}

public class WbsProductFilterDto
{ 
    public string ProjectTitle { get; set; }
    public string ProductTitle { get; set; }
}