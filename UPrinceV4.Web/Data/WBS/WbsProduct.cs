using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.WBS
{
    public class WbsProduct
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string WbsTaxonomyId { get; set; }
        public DateTime? Date { get; set; }
        public bool IsFavourite { get; set; }
        public string StatusId { get; set; }
        public string DeliveryStatusId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int DisplayOrder { get; set; }
        public string ParentId { get; set; }
    }
    
    public class WbsProductCreate
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string WbsTaxonomyId { get; set; }
        public DateTime? Date { get; set; }
        public string StatusId { get; set; }
        public string DeliveryStatusId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public List<WbsProductTags> Tags { get; set; }
        public List<WbsProductDocuments> Documents { get; set; }
        public List<WbsProductCc> Cc { get; set; }
        public List<WbsProductTo> ToPerson { get; set; }


    }
    
    public class GetWbsProduct
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string PbsId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string WbsTaxonomyId { get; set; }
        public DateTime? Date { get; set; }
        public bool IsFavourite { get; set; }
        public string StatusId { get; set; }
        public string DeliveryStatusId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int DisplayOrder { get; set; }
        public List<WbsProductTags> Tags { get; set; }
        public List<WbsProductDocuments> Documents { get; set; }
        public List<WbsProductCc> Cc { get; set; }
        public List<WbsProductTo> ToPerson { get; set; }
        public List<WbsTaskFilterResults> Tasks { get; set; }
        public List<GetConversationDto> Conversations { get; set; }
        
        public string PbsTitle { get; set; }


    }
    
    public class WbsTemplateUpdate
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string SequenceCode { get; set; }


    }
    
    public class WbsDragAndDrop
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        
    }
    
    public class WbsTaskDateUpdateDto
    {
        public string TaskId { get; set; }
        public DateTime Date { get; set; }
        
    }
}
