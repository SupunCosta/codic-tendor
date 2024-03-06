namespace UPrinceV4.Web.Data.TAX;

public class Tax
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }

    public bool IsDefault { get; set; }
}