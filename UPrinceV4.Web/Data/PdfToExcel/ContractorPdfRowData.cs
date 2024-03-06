using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PdfToExcel;

public class ContractorPdfRowData
{
    public string Id { get; set; }
    public string Result { get; set; }
    public string LotId { get; set; }
    public string ContractorId { get; set; }
    public DateTime CreatedDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }
}