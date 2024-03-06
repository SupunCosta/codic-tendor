using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.WBS;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IWbsRepository
{
    Task<string> CreateWbs(WbsParameter wbsParameter);
    Task<string> CreateTemplate(WbsParameter wbsParameter);
    Task<IEnumerable<WbsTemplate>> WbsFilter(WbsParameter wbsParameter);
    Task<List<WbsTaxonomyList>> WbsTaxonomyList(WbsParameter wbsParameter);
    Task<List<WbsTaxonomyList>> GetWbsTaxonomyByTemplate(WbsParameter wbsParameter);
    Task<WbsDropDownData> GetWbsDropdown(WbsParameter wbsParameter);
    Task<WbsTaxonomyList> GetWbsById(WbsParameter wbsParameter);
    Task<List<PbsTreeStructure>> WbsProductFilter(WbsParameter wbsParameter);
    Task<string> WbsTaskCreate(WbsParameter wbsParameter);
    Task<GetWbsTask> WbsTaskGetById(WbsParameter wbsParameter);
    Task<string> WbsTaskDelete(WbsParameter wbsParameter);
    Task<List<WbsTaskFilterResults>> WbsTaskFilter(WbsParameter wbsParameter);
    Task<List<WbsTaskFilterResultsDto>> WbsTaskGroupFilter(WbsParameter wbsParameter);
    Task<List<TaskInstruction>> GetWbsTaskInstructionList(WbsParameter wbsParameter);
    Task<List<WbsTaskFilterResults>> GetWbsList(WbsParameter wbsParameter);
    Task<string> WbsProductCreate(WbsParameter wbsParameter);
    Task<string> WbsDragAndDrop(WbsParameter wbsParameter);
    Task<GetWbsProduct> WbsProductGetById(WbsParameter wbsParameter);
    Task<string> WbsTaskIsFavourite(WbsParameter wbsParameter);
    Task<string> WbsProductDelete(WbsParameter wbsParameter);
    Task<string> CopyWbsToProject(WbsParameter wbsParameter);
    Task<string> WbsEditTemplateName(WbsParameter wbsParameter);
    Task<string> CreateWbsConversation(WbsParameter wbsParameter);
    Task<List<WbsConversationDto>> GetWbsConversation(WbsParameter wbsParameter);
    Task<List<WbsTaskCheckListDto>> GetWbChecklistById(WbsParameter wbsParameter);
    Task<WbsProductEmai> AddWbsProductEmails(WbsParameter wbsParameter);
    Task<List<WbsTaskTags>> GetTagList(WbsParameter wbsParameter);

    Task<List<WbsTaskFilterResultsDto>> WbsTaskAllProjectsGroupFilter(
        WbsParameter wbsParameter);
    Task<string> WbsTaskStatusUpdate(WbsParameter wbsParameter);
    Task<UploadWbsDocumentMetaDataDto> UploadWbsDocument(WbsParameter wbsParameter);
    Task<UploadWbsDocumentMetaDataDto> UploadWbsDocumentMetaData(WbsParameter wbsParameter);
    Task<UpdateWbsDocumentDto> UpdateWbsDocumentUploadData(WbsParameter wbsParameter);
    Task<List<WbsDocument>> GetWbsDocument(WbsParameter wbsParameter);
    Task<GetWbsDocumentIdByUrl> GetWbsDocumentIdByUrl(WbsParameter wbsParameter);
    Task<List<WbsDocument>> GetWbsDocumentIdByMailId(WbsParameter wbsParameter);
    Task<TaskDocDto> GetWbsDocumentIdByTaskId(WbsParameter wbsParameter);
    Task<string> ReadWbsDocumentIdByUrl(WbsParameter wbsParameter);
    Task<List<string>> WbsTaskDocumentsDelete(WbsParameter wbsParameter);
    Task<List<string>> WbsProductDocumentsDelete(WbsParameter wbsParameter);
    Task<ContentTypeCollectionResponse> GetContentTypes(WbsParameter wbsParameter);
    Task<ListItem> UpdateFile(WbsParameter wbsParameter);
    Task<string> WbsTaskDateUpdate(WbsParameter wbsParameter);

}

public class WbsParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public bool IsMyEnv { get; set; }
    public bool IsCu { get; set; }
    public WbsTaxonomy WbsTaxonomy { get; set; }
    public WbsFilterDto WbsFilter { get; set; }
    public WbsTemplate WbsTemplate { get; set; }
    public WbsProductFilterDto WbsProductFilter { get; set; }
    public  IPbsRepository IPbsRepository { get; set; }
    public  WbsTaskCreate wbsTaskCreate { get; set; }
    public  WbsTaskFilterDto wbsTaskFilterDto { get; set; }
    public IPbsInstructionsRepository iPbsInstructionsRepository { get; set; }
    
    public  WbsProductCreate wbsProductCreate { get; set; }
    public  WbsTemplateUpdate WbsTemplateUpdate { get; set; }
    
    public  GetWbsTaskInstructionList getWbsTaskInstructionList { get; set; }
    public  List<WbsConversationDto> WbsConversationDto { get; set; }
    public  TaskIsFavouriteDto TaskIsFavouriteDto { get; set; }
    public  List<string> IdList { get; set; }
    
    public  WbsProductEmai WbsProductEmai { get; set; }
    public  bool IsCheckList { get; set; } 
    public  WbsDragAndDrop WbsDragAndDrop { get; set; }
    public  TaskStatusUpdateDto TaskStatusUpdateDto { get; set; }
    public  UploadWbsDocumentDto UploadWbsDocumentDto { get; set; }
    public  UpdateWbsDocumentDto UpdateWbsDocumentDto { get; set; }
    public  GetWbsDocumentIdByUrlDto GetWbsDocumentIdByUrlDto { get; set; }
    public List<IFormFile> doc { get; set; }
    public  IIssueRepository iIssueRepository { get; set; }
    public  string ProductGetById { get; set; } 
    public  string IssueGetById { get; set; } 
    public  UrlDto UrlDto { get; set; }
    public GetWbsDocumentIdByTaskId GetWbsDocumentIdByTaskId { get; set; }
    public UpdateFileDto UpdateFileDto { get; set; }
    public WbsTaskDateUpdateDto WbsTaskDateUpdateDto { get; set; }

}