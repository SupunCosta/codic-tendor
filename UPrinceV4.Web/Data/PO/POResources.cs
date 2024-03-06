using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PO;

public class POResources
{
    public string Id { get; set; }
    public POHeader POHeader { get; set; }
    [ForeignKey("POHeader")] public string PurchesOrderId { get; set; }
    public string ResourcesType { get; set; }
    public string ResourceNumber { get; set; }
    public DateTime? Date { get; set; }
    public string PQuantity { get; set; }
    public string PPurchased { get; set; }
    public string PDeliveryRequested { get; set; }
    public string warf { get; set; }
    public string PConsumed { get; set; }
    public string PInvoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public string BorTitle { get; set; }
    public string BorId { get; set; }
    public string PDocuments { get; set; }
    public string PComments { get; set; }
    public string PUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string PCrossReference { get; set; }
    public string CComments { get; set; }
    public string CUnitPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string CQuantity { get; set; }
    public string CPurchased { get; set; }
    public bool CDeliveryRequested { get; set; }
    public DateTime? CStartDate { get; set; }
    public DateTime? CStopDate { get; set; }
    public string CNumberOfDate { get; set; }
    public string CFullTimeEmployee { get; set; }
    public DateTime? PStartDate { get; set; }
    public DateTime? PStopDate { get; set; }
    public string PNumberOfDate { get; set; }
    public string PFullTimeEmployee { get; set; }
    public string Pdevices { get; set; }
    public string Cdevices { get; set; }
    public string ProjectTitle { get; set; }
    public string PbsTitle { get; set; }

    public string CTitle { get; set; }

    public string PTitle { get; set; }

    public string CMou { get; set; }

    public string PMou { get; set; }

    public string CCPCId { get; set; }
    public string PCPCId { get; set; }

    public bool? IsStock { get; set; }

    public string ResourceFamily { get; set; }

    public string BorResourceId { get; set; }

    public bool HasChanged { get; set; } = false;

    public bool? IsUsed { get; set; } = false;

    public string UsedPoId { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string WorkFlowId { get; set; }
    public string PbsProductId { get; set; }
}

public class POResourceFilterDto
{
    public string POHeaderId { get; set; }
}

public class POResourceStockUpdate
{
    public string SequenceId { get; set; }

    public string ProjectSequenceId { get; set; }

    public List<POResourceStockDto> POResourceStockDtoList { get; set; }
}

public class POProductIsPoUpdate
{
    public string SequenceId { get; set; }

    public string ProjectSequenceId { get; set; }

    public List<POProductIsPoDto> POProductIsPoDtoList { get; set; }
}

public class POResourceStockDto
{
    public string POResourceId { get; set; }
    public bool IsStock { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string CUnitPrice { get; set; }
    public string BorId { get; set; }
    public string CpcId { get; set; }
    public string ResourceType { get; set; }
    public string CTotalPrice { get; set; }
    public string CcpcId { get; set; }
}

public class POProductIsPoDto
{
    public string POProductId { get; set; }
    public bool IsPo { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string CTotalPrice { get; set; }
}