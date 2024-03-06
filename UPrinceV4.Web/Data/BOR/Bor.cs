using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Data.BOR;

public class Bor
{
    public string Id { get; set; }
    public string ItemId { get; set; }
    public string Name { get; set; }

    [ForeignKey("PbsProduct")] public string PbsProductId { get; set; }

    public virtual PbsProduct PbsProduct { get; set; }
    public bool IsDeleted { get; set; }
    public string UtilityParentId { get; set; }
    public string LocationParentId { get; set; }
    public string BorStatusId { get; set; }
    public string Title { get; set; }
    public string BorTypeId { get; set; }

    public string ProjectSequenceCode { get; set; }

    // [NotMapped] public string Title => ItemId + " - " + Name;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [NotMapped] public string HeaderTitle { get; set; }
    [NotMapped] public string Key => Id;
    [NotMapped] public string Text => Title;
}

public class BorDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string BorStatusId { get; set; }
    public BorProductDto Product { get; set; }
    public BorResource BorResources { get; set; }
    public string BorTypeId { get; set; }
    public string CId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool WeekPlan { get; set; }
    public bool IsTh { get; set; }
}

public class BorProductDto
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string UtilityTaxonomyParentId { get; set; }
    public string LocationTaxonomyParentId { get; set; }
}

public class BorResource
{
    public List<BorResourceCreateDto> Consumable { get; set; }
    public List<BorResourceCreateDto> Labour { get; set; }
    public List<BorResourceCreateDto> Materials { get; set; }
    public List<BorResourceCreateDto> Tools { get; set; }
}

public class BorResourceUpdate
{
    public string BorRequiredId { get; set; }
    public string required { get; set; }
    public string BorId { get; set; }
}

public class BorResourceCreateDto
{
    public string Id { get; set; }
    public double Required { get; set; }
    public string Type { get; set; }
    public bool IsNew { get; set; }
    public string Environment { get; set; }
    public string ResourceNumber { get; set; }
    public string Returned { get; set; }
}

public class BorListDto
{
    public string Id { get; set; }
    public string Key { get; set; }
    public string ItemId { get; set; }
    public string BorTitle { get; set; }
    public string Name { get; set; }
    public string ProductId { get; set; }
    public string Product { get; set; }
    public string LocationParent { get; set; }
    public string UtilityParent { get; set; }
    public string PbsId { get; set; }
    public string BorTypeId { get; set; }
}

public class BorShortcutPaneDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}

public class BorGetByIdDto
{
    public string Id { get; set; }
    public string ItemId { get; set; }
    public string BorStatusId { get; set; }
    public string BorTitle { get; set; }
    public string Name { get; set; }
    public string HeaderTitle { get; set; }
    public BorGetByIdProductDto Product { get; set; }
    public BorResourceListGetByIdDto BorResources { get; set; }
    public ProjectDefinitionHistoryLogDto historyLog { get; set; }
    public string BorTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaterialCount { get; set; }
    public int ConsumableCount { get; set; }
    public int LabourCount { get; set; }
    public int ToolsCount { get; set; }
}

public class BorResourceGetByIdDto
{
    public string Id { get; set; }
    public string Title { get; set; }

    public string ResourceNumber { get; set; }
    public DateTime Date { get; set; }
    public double Required { get; set; }
    public double Purchased { get; set; }
    public double DeliveryRequested { get; set; }
    public double Warf { get; set; }
    public double Consumed { get; set; }
    public double Invoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public bool IsNew { get; set; }
    public DateTime? ActualDeliveryDate { get; set; } = null;
    public DateTime? ExpectedDeliveryDate { get; set; } = null;
    public DateTime? RequestedDeliveryDate { get; set; } = null;
    public double Returned { get; set; }
}

public class BorResourceGetByIdsDto
{
    public string Id { get; set; }
    public string Title { get; set; }

    public string ResourceNumber { get; set; }
    public DateTime Date { get; set; }
    public double Required { get; set; }
    public double Purchased { get; set; }
    public double DeliveryRequested { get; set; }
    public double Warf { get; set; }
    public double Consumed { get; set; }
    public double Invoiced { get; set; }
    public string CorporateProductCatalogId { get; set; }

    public string BorTitle { get; set; }


    public string BorId { get; set; }

    public string MOU { get; set; }

    public string InventoryPrice { get; set; }

    public string PbsTitle { get; set; }

    public string ResourceFamily { get; set; }
    public double Returned { get; set; }
}

public class BorResourceListGetByIdsDto
{
    public IEnumerable<BorResourceGetByIdsDto> Consumable { get; set; }
    public IEnumerable<BorResourceGetByIdsDto> Labour { get; set; }
    public IEnumerable<BorResourceGetByIdsDto> Materials { get; set; }
    public IEnumerable<BorResourceGetByIdsDto> Tools { get; set; }
}

public class BorResourceListGetByIdDto
{
    public IEnumerable<BorResourceGetByIdDto> Consumable { get; set; }
    public IEnumerable<BorResourceGetByIdDto> Labour { get; set; }
    public IEnumerable<BorResourceGetByIdDto> Materials { get; set; }
    public IEnumerable<BorResourceGetByIdDto> Tools { get; set; }
}

public class BorGetByIdProductDto
{
    public string ProductId { get; set; }
    public string Name { get; set; }
    public string LocationTaxonomyParentId { get; set; }
    public string UtilityTaxonomyParentId { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string pbsProductItemTypeId { get; set; }
    public string pbsProductItemType { get; set; }
    public string PbsLocation { get; set; }
}

public class BorResourceListDto
{
    public string CorporateProductCatalogId { get; set; }
    public double Required { get; set; }
    public string ResourceType { get; set; }
    public double Consumed { get; set; }
    public double Purchased { get; set; }
    public double DeliveryRequested { get; set; }
    public double Warf { get; set; }
    public double Invoiced { get; set; }
    public string ResourceTitle { get; set; }
    public double Returned { get; set; }
}

public class BorStatusUpdateDto
{
    public string BorId { get; set; }
    public string StatusId { get; set; }
}

public class PbsResourcesForBorDto
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public string CoperateProductCatalogId { get; set; }
    public string Quantity { get; set; }
}