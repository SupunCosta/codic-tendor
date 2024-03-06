using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PMOL;

public class AzureMap
{
    public List<Query> batchItems { get; set; }
}

public class Query
{
    public string query { get; set; }
}