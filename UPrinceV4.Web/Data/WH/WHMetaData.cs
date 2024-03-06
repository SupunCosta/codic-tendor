using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WH;

public class WHMetaData
{
    public DateTime CreatedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime ModifiedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }
}