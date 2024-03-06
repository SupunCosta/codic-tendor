namespace UPrinceV4.Web.Data.CAB;

public class CabEmail : CabUniqueData
{
    public string Id { get; set; }
    public string EmailAddress { get; set; }
    public bool IsDeleted { get; set; }
}