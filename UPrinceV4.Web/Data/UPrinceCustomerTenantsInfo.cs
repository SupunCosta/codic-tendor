using System.Collections.Generic;

namespace UPrinceV4.Web.Data;

public class UPrinceCustomerTenantsInfo
{
    public string Id { get; set; }
    public string Host { get; set; }
    public string DatabaseType { get; set; }
    public string Name { get; set; }
    public string AzureBlob { get; set; }
    public string StorageConnectionString { get; set; }
    public string AzureContainer { get; set; }
    public string ConnectionString { get; set; }
    public string CatelogConnectionString { get; set; }
    public string ClientId { get; set; }
    public string ClientSecretKey { get; set; }
    public string TenantId { get; set; }
    public string MapUrl { get; set; }
    private IList<UPrinceCustomerContractingUnit> UPrinceCustomerContractingUnit { get; set; }
    public string ColorCode { get; set; }
    public string LogoUrl { get; set; }
}