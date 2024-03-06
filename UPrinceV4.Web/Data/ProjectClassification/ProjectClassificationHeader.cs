namespace UPrinceV4.Web.Data.ProjectClassification;

public class ProjectClassificationHeader
{
    public string Id { get; set; }
    public string ProjectId { get; set; }

    public string ProjectClassificationBuisnessUnit { get; set; }

    public string ProjectClassificationSizeId { get; set; }

    public string ProjectClassificationConstructionTypeId { get; set; }

    public string ProjectClassificationSectorId { get; set; }
}

public class ProjectClassificationCreateDto
{
    public string ProjectId { get; set; }

    public string ProjectClassificationBuisnessUnit { get; set; }

    public string ProjectClassificationSizeId { get; set; }

    public string ProjectClassificationConstructionTypeId { get; set; }

    public string ProjectClassificationSectorId { get; set; }
}

public class ProjectClassificationUpdateDto
{
    public string Id { get; set; }
    public string ProjectId { get; set; }

    public string ProjectClassificationBuisnessUnit { get; set; }

    public string ProjectClassificationSizeId { get; set; }

    public string ProjectClassificationConstructionTypeId { get; set; }

    public string ProjectClassificationSectorId { get; set; }
}