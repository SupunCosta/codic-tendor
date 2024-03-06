namespace UPrinceV4.Shared;

public class Tenant
{
    public int Id { get; set; }
    public int DatabaseType { get; set; }
    public string Host { get; set; }
    public string ConnectionString { get; set; }
    public string CatelogConnectionString { get; set; }
    public string Name { get; set; }
    public string StorageType { get; set; }
    public string StorageConnectionString { get; set; }
    public string AzureContainer { get; set; }
    public bool UseAdvancedProductThumbnails { get; set; }
    public bool SendProductNotifications { get; set; }
    public string MapUrl { get; set; }
    public string ColorCode { get; set; }
    public string LogoUrl { get; set; }
}

public class TenantDto
{
    public int Id { get; set; }
    public string Host { get; set; }
    public string Name { get; set; }
    public string ColorCode { get; set; }
    public string LogoUrl { get; set; }
}