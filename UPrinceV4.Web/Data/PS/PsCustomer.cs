using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PS;

public class PsCustomer
{
    public string Id { get; set; }
    [ForeignKey("PsHeader")] public string PsId { get; set; }
    public virtual PsHeader PsHeader { get; set; }
    public string CabPersonId { get; set; }
    public string CustomerName { get; set; }
    public string ContactPersonName { get; set; }
    public string ContactPersonEmail { get; set; }
    public string PurchaseOrderNumber { get; set; }
}

public class PsCustomerReadDto
{
    public string CustomerName { get; set; }
    public string ContactPersonName { get; set; }
    public string ContactPersonEmail { get; set; }
    public string PurchaseOrderNumber { get; set; }
    public string CabPersonId { get; set; }
}