using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Stock;

public class StockHeader : StockMetaData
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string CPCId { get; set; }
    public string TypeId { get; set; }
    public string ActiveTypeId { get; set; }
    public string StatusId { get; set; }
    public string AvailableQuantity { get; set; }
    public string MOUId { get; set; }
    public string AveragePrice { get; set; }
    public string QuantityToBeDelivered { get; set; }
    public string WareHouseTaxonomyId { get; set; }
}

public class StockListDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string CpcId { get; set; }
    public string CpcResourceTitle { get; set; }
    public string ResourceTypeId { get; set; }
    public string ResourceType { get; set; }
    public string TypeId { get; set; }
    public string Type { get; set; }
    public string StatusId { get; set; }
    public string Status { get; set; }
    public string AvailableQuantity { get; set; }
    public string MOUId { get; set; }
    public string AveragePrice { get; set; }
    public string QuantityToBeDelivered { get; set; }
    public string WareHouseTaxonomyId { get; set; }
    public string WareHouse { get; set; }
    public string Count { get; set; }
}

public class StockCreateDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string CpcId { get; set; }
    public string Type { get; set; }
    public string ResourceType { get; set; }
    public string Status { get; set; }
    public string AvailableQuantity { get; set; }
    public string MouId { get; set; }
    public string AveragePrice { get; set; }
    public string QuantityToBeDelivered { get; set; }
    public string WareHouseTaxonomyId { get; set; }
}

public class StockHeaderDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string AvailableQuantity { get; set; }
    public string MOUId { get; set; }
    public string AveragePrice { get; set; }
    public string QuantityToBeDelivered { get; set; }
    public string WarehouseTaxonomyId { get; set; }
    public StockTypeDto ResourceType { get; set; }
    public StockActiveTypeDto Type { get; set; }
    public StockStatusDto Status { get; set; }
    public StockHistoryDto History { get; set; }
    public IEnumerable<StockActivityHistoryDto> StockHistory { get; set; }
    public CpcDto CpcType { get; set; }
    public string CPCId { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string ActiveTypeId { get; set; }
}

public class StockActivityHistoryDto
{
    public string Id { get; set; }
    public string WorkFlowId { get; set; }
    public DateTime Date { get; set; }
    public string Quantity { get; set; }
    public string Price { get; set; }
    public string Mou { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string WareHouseWorker { get; set; }
}

public class StockHistoryDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class CpcDto
{
    public string Value { get; set; }
    public string Label { get; set; }
}