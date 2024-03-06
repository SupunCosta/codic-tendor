using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PO;

public class POProduct
{
    public string Id { get; set; }

    public string HeaderTitle { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string PbsProductItemType { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatus { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceState { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    [NotMappedAttribute] public List<string> PDocuments { get; set; }
    [NotMappedAttribute] public List<string> CDocuments { get; set; }
    public string PComment { get; set; }
    public string CComment { get; set; }
    public string PQuantity { get; set; }
    public string CQuantity { get; set; }
    public string Mou { get; set; }
    public string PUnitPrice { get; set; }
    public string CUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string PCrossReference { get; set; }
    public string ProjectTitle { get; set; }


    [ForeignKey("POHeader")] public string POHeaderId { get; set; }

    public bool? IsPo { get; set; }

    public bool? IsUsed { get; set; }
    public string UsedPoId { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
}

public class POProductDto
{
    public string Id { get; set; }

    public string HeaderTitle { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string PbsProductItemType { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatus { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceState { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string ProductId { get; set; }
    public string Title { get; set; }
    [NotMappedAttribute] public List<string> PDocuments { get; set; }
    [NotMappedAttribute] public List<string> CDocuments { get; set; }
    public string PComment { get; set; }
    public string CComment { get; set; }
    public string PQuantity { get; set; }
    public string CQuantity { get; set; }
    public string Mou { get; set; }
    public string PUnitPrice { get; set; }
    public string CUnitPrice { get; set; }
    public string PTotalPrice { get; set; }
    public string CTotalPrice { get; set; }
    public string CCrossReference { get; set; }
    public string PCrossReference { get; set; }
    public string ProjectTitle { get; set; }


    [ForeignKey("POHeader")] public string POHeaderId { get; set; }

    public POHeader POHeader { get; set; }

    public bool? IsPo { get; set; }

    public bool? IsUsed { get; set; }
    public string UsedPoId { get; set; }
}