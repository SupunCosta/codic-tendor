using System;

namespace UPrinceV4.Web.Data.WH;

public class WHTaxonomy
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string WareHouseId { get; set; }
    public string ParentId { get; set; }
    public string WHTaxonomyLevelId { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
}

public class WHTaxonomyCreateDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string WareHouseId { get; set; }
    public string ParentId { get; set; }
    public string WHTaxonomyLevelId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class WHTaxonomyListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string WareHouseId { get; set; }
    public string ParentId { get; set; }
    public string WHTaxonomyLevelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class WHTaxonomyFilterDto
{
    public string WareHouseId { get; set; }
}