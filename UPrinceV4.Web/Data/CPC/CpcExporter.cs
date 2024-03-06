namespace UPrinceV4.Web.Data.CPC;

public class CpcExporter
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ResourceType { get; set; }
    public string ResourceFamily { get; set; }
    public string CpcBasicUnitOfMeasure { get; set; }
    public string CpcMaterial { get; set; }
    public string CpcPressureClass { get; set; }
    public double? Size { get; set; }
    public string CpcUnitOfSizeMeasure { get; set; }
    public double? WallThickness { get; set; }
    public double? InventoryPrice { get; set; }
    public double? MinOrderQuantity { get; set; }
    public double? MaxOrderQuantity { get; set; }
    public double? Weight { get; set; }
    public int Status { get; set; }
}