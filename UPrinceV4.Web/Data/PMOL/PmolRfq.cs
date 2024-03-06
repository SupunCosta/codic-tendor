using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolRfq
{
    public string Id { get; set; }
    public string TeamRoleId { get; set; }
    public string PmolId { get; set; }
}

public class RfqResults
{
    public DateTime? Date { get; set; }
    public Person1 RequestedBy { get; set; }
    public string RfqRequested { get; set; }
    public string RfqApproved { get; set; }
    public List<PmolResults> PmolResults { get; set; }
}

public class PmolResults
{
    public string PmolTitle { get; set; }
    public bool Status { get; set; }
    public string Message { get; set; }
}

public class Person1
{
    public string Key { get; set; }
    public string Text { get; set; }
}