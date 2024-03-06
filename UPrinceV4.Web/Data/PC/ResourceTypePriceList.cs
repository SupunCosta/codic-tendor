using UPrinceV4.Web.Data.TAX;

namespace UPrinceV4.Web.Data.PC;

public class ResourceTypePriceList
{
    public string Id { get; set; }
    public double? MaterialCoefficient { get; set; }
    public double? LabourCoefficient { get; set; }
    public double? ConsumableCoefficient { get; set; }
    public double? ToolCoefficient { get; set; }
    public double? ServiceCoefficient { get; set; }
    public bool IsCurrent { get; set; }

    public Tax Tax { get; set; }
}

public class ResourceTypePriceListCreateDto
{
    public double? MaterialCoefficient { get; set; }
    public double? LabourCoefficient { get; set; }
    public double? ConsumableCoefficient { get; set; }
    public double? ToolCoefficient { get; set; }
    public double? ServiceCoefficient { get; set; }
}

public class ResourceTypePriceListReadDto
{
    public string ProjectSequenceCode { get; set; }
}