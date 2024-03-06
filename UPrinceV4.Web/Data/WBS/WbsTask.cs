using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace UPrinceV4.Web.Data.WBS
{
    public class WbsTask
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
        public string IssueId { get; set; }



    }

    public class WbsTaskCreate
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
        public List<WbsTaskTags> Tags { get; set; }
        public List<WbsTaskDocuments> Documents { get; set; }
        public List<WbsTaskCc> Cc { get; set; }
        public List<TaskInstruction> Instructions { get; set; }
        public List<WbsTaskTo> ToPerson { get; set; }
        public List<WbsTaskEmail>? Email { get; set; }
        public string IssueId { get; set; }


    }

    public class GetWbsTask
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
        public List<WbsTaskTags> Tags { get; set; }
        public List<WbsTaskDocuments> Documents { get; set; }
        public List<WbsTaskCc> Cc { get; set; }
        public List<WbsTaskTo> ToPerson { get; set; }
        public List<TaskInstruction> Instructions { get; set; } 
        public TaskTimeLine TimeLine { get; set; }
        public List<WbsTaskEmail> Email { get; set; }
        
        public List<WbsConversationDto> Conversations { get; set; } 
        public string ProjectSequenceId { get; set; }
        public string Cu { get; set; }
        public string TemplateId { get; set; }
        public string PbsTitle { get; set; }
        public string IssueId { get; set; }
        public string Wbs { get; set; }
        public string ProjectTitle { get; set; }
        public string IssueTitle { get; set; }


    }

    public class TaskInstruction
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsChecked { get; set; }
    }

    public class WbsTaskFilterResults
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string ParentId { get; set; }
        public string WbsTaxonomyId { get; set; }
        public string To { get; set; }
        public string PersonName { get; set; }
        public DateTime? Date { get; set; }
        public int? TotalAttachment { get; set; }
        public int? TotalMail { get; set; }
        public int? TotalInstructions { get; set; }
        public int? TotalCheckedInstructions { get; set; }
        public bool IsFavourite { get; set; }
        public string StatusId { get; set; }
        public string DeliveryStatusId { get; set; }
        public string Tag { get; set; }
        public List<WbsTaskTo> ToPerson { get; set; }
        public List<WbsTaskCc> Cc { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsProduct { get; set; }
        public string ProjectSequenceId { get; set; }
        public string ProjectTitle{ get; set; }

        public string Cu { get; set; }
        public string IssueId { get; set; }

    }

    public class WbsTaskFilterResultsDto
    {
        
        public string GroupItem { get; set; }
        public List<WbsTaskFilterResults> FilterList { get; set; }
        
    }
    
    public class WbsTaskCheckListDto
    {
        
        public string GroupItem { get; set; }
        public List<WbsTaskFilterResultsDto> FilterList { get; set; }
        
    }
    
    public class WbsTaskFilterDto
    {
        public string Name { get; set; }
        public bool? IsFavourite { get; set; }
        public DateTime? Date { get; set; }
        public string GroupItem { get; set; }
        public string StatusId { get; set; }
        public string DeliveryStatusId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Tag { get; set; }
        public Sorter Sorter { get; set; }
        public string ShownBy { get; set; }
        public bool? IsMyTask { get; set; }
        public List<string>? PersonId { get; set; }
        public bool? IsCc { get; set; }
        public bool? IsTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }



        
    }
    
    public class GetWbsTaskInstructionList
    {
        public string TaskId { get; set; }
        public string ProductId { get; set; }
        
    }
    
    public class TaskTimeLine
    {
        public DateTime? CreationDate { get; set; }
    }
    
    public class TaskIsFavouriteDto
    {
        public string Id { get; set; }
        public bool IsFavourite { get; set; }

    }
    
    
    public class TaskStatusUpdateDto
    {
        public string Id { get; set; }
        public string StatusId { get; set; }

    } 
    
    public class UploadWbsDocumentDto
    {
        public string ProjectTitle { get; set; }
        public string Wbs { get; set; }
        public string Product { get; set; }
        public string WbsId { get; set; }

    }
    public class UpdateWbsDocumentDto
    {
        public string ProjectTitle { get; set; }
        public string Wbs { get; set; }
        public string Product { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
        public string SiteId { get; set; }
        public List<UrlDto> Url { get; set; }
    }
    
    public class UrlDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string DownloadUrl { get; set; }
        public string AttachmentId { get; set; }
        public string MailId { get; set; }
    }
    
    public class UploadWbsDocumentMetaDataDto
    {
        public string SiteId { get; set; }
        public string DriveId { get; set; }
        public string FolderId { get; set; }
        public string SharepointFileId { get; set; }
        public string Link { get; set; }
    }
    
    public class GetWbsDocumentIdByUrlDto
    {
        public string AttachmentId { get; set; }
        public string Url { get; set; }
    }
    
    public class GetWbsDocumentIdByTaskId
    {
        public string TaskId { get; set; }
        public string Url { get; set; }
        public string ProjectTitle { get; set; }
    }
    
    public class TaskDocDto
    {
        public FieldValueSet Fields { get; set; }
        public  ColumnDefinitionCollectionResponse Columns { get; set; }
        public  Stream Content { get; set; }
    }
    public class UpdateFileDto
    {
        public string Url { get; set; }
        public ContentType ContentType { get; set; }
        public Fields Fields { get; set; }
        public ListItem ListItem { get; set; }
    }
    
    public class ContentType
    {
        public string Id { get; set; }
    }
    
    public class Fields
    {
        public string FileLeafRef { get; set; }
        public string Title { get; set; }
    }
    
}
