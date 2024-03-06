using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PO;

public class POMetaData
{
    public DateTime CreatedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime ModifiedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }
}