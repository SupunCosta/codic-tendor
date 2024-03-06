using System.Collections.Generic;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data
{
    public class GraphClientGroup
    {
        public string description { get; set; }
        public string displayName { get; set; }
        public List<object> groupTypes { get; set; }
        public bool mailEnabled { get; set; }
        public string mailNickname { get; set; }
        public List<object> onPremisesProvisioningErrors { get; set; }
        public List<object> proxyAddresses { get; set; }
        public bool securityEnabled { get; set; }
        public string securityIdentifier { get; set; }
        public string id { get; set; }
        [JsonProperty("@odata.type")] public string OdataType { get; set; }
        public object createdByAppId { get; set; }
        public List<object> infoCatalogs { get; set; }
        public object isAssignableToRole { get; set; }
        public object isManagementRestricted { get; set; }
        public List<object> resourceBehaviorOptions { get; set; }
        public List<object> resourceProvisioningOptions { get; set; }
        public WritebackConfiguration writebackConfiguration { get; set; }
    }
}

public class WritebackConfiguration
{
    public object isEnabled { get; set; }
    public object onPremisesGroupType { get; set; }
}