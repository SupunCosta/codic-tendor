using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.Stock;

namespace UPrinceV4.Web.Data.WF;

public class WFHeader : WFMetaData
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string TypeId { get; set; }
    public string ResourceType { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public bool IsFinish { get; set; } = false;
    public string RequesterId { get; set; }
    public string ExecutorId { get; set; }
    public DateTime? RequiredDateAndTime { get; set; }
    public DateTime? ExecutedDateAndTime { get; set; }
    public string EffortEstimate { get; set; }
    public string EffortCompleted { get; set; }
    public string StatusId { get; set; }
    public string Project { get; set; }
    public string BorId { get; set; }
    public string Comment { get; set; }
    public bool IsFromCU { get; set; } = false;
    public string PoId { get; set; }
    public string StockId { get; set; }
}

public class WFListDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string TypeId { get; set; }
    public string ResourceType { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public string ModifiedDate { get; set; }
    public string WFTypeName { get; set; }
    public string WFStatusName { get; set; }
    public string CpcName { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public bool IsFinish { get; set; }
    public string RequesterId { get; set; }
    public string Requester { get; set; }
    public string ExecutorId { get; set; }
    public string Executor { get; set; }
    public DateTime? RequiredDateAndTime { get; set; }
    public DateTime? ExecutedDateAndTime { get; set; } = null;
    public string EffortEstimate { get; set; }
    public string EffortCompleted { get; set; }
    public string StatusId { get; set; }
    public string Project { get; set; }
}

public class WFCreateDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string TypeId { get; set; }
    public string ResourceType { get; set; }
    public string RequesterId { get; set; }
    public string ExecutorId { get; set; }
    public string RequiredDateAndTime { get; set; }
    public string ExecutedDateAndTime { get; set; }
    public string EffortEstimate { get; set; }
    public string EffortCompleted { get; set; }
    public string Status { get; set; }
    public List<string> files { get; set; }
    public List<WFTasksDto> Tasks { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public bool IsFinished { get; set; }
    public string Project { get; set; }
    public string Comment { get; set; }
}

public class WFHeaderDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string RequesterId { get; set; }
    public string Requester { get; set; }
    public string ExecutorId { get; set; }
    public string Executor { get; set; }
    public string RequestedDateAndTime { get; set; }
    public string ExecutedDateAndTime { get; set; }
    public string EffortEstimate { get; set; }
    public string EffortCompleted { get; set; }
    public string Project { get; set; }
    public List<string> Files { get; set; }
    public List<WFTasksDto> Tasks { get; set; }
    public bool IsFinish { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public CpcForProductDto CpcType { get; set; }
    public WFTypeDto Type { get; set; }
    public WHHistoryDto History { get; set; }
    public WFActivityStatusDto Status { get; set; }

    public string BorId { get; set; }
    public string BorTitle { get; set; }
    public string Comment { get; set; }
    public string StockId { get; set; }
}

public class WFTasksDto
{
    public string Id { get; set; }
    public string Source { get; set; }
    public string CPCItemId { get; set; }
    public string Quantity { get; set; }
    public string MOUId { get; set; }
    public string PickedQuantity { get; set; }
    public string Destination { get; set; }

    public string Comment { get; set; }
    public string StockAvailability { get; set; }
    public string Mou { get; set; }
    public CorporateProductCatalogWFDto CorporateProductCatalog { get; set; }
    public StockHeader Stock { get; set; }
    public string UnitPrice { get; set; }
}

public class WHHistoryDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class CabPearsonDto
{
    public string FullName { get; set; }
    public string PersonCompanyId { get; set; }
}

public class CorporateProductCatalogWFDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class AllWF
{
    public string SequenceId { get; set; }
    public string Project { get; set; }
}