namespace UPrinceV4.Web.Azure;

public class AzureAdOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Instance { get; set; }
    public string Domain { get; set; }
    public string TenantId { get; set; }
    public string Authority { get; set; }
    public string TokenUrl { get; set; }
    public string AuthorizationUrl { get; set; }
}