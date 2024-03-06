using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WF;

public class WFMetaData
{
    public DateTime CreatedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime ModifiedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }
}