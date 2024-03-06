using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Comment;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Data.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces.BM;

public interface IContractorReopsitory
{
    Task<ContractorDropDownData> GetContractorDropDownData(ContractorParameter ContractorParameter);

    Task<string> CreateHeader(ContractorParameter ContractorParameter, IGraphRepository IGraphRepository,
        ISendGridRepositorie ISendGridRepositorie);

    Task<IEnumerable<ContractorListDto>> ContractorFilter(ContractorParameter ContractorParameter);
    Task<BMLotHeaderGetDto> GetContractorById(ContractorParameter ContractorParameter);
    Task<string> UpdateContractorWorkflow(ContractorParameter ContractorParameter);
    Task<List<PmolShortcutpaneDataDto>> ShortcutPaneData(ContractorParameter ContractorParameter);
    Task<ConstructorWorkFlowDto> GetConstructorById(ContractorParameter ContractorParameter);
    Task<List<ContractorTeamList>> GetConstructorByTaxonomy(ContractorParameter ContractorParameter);
    Task<string> ExcelUpload(ContractorParameter ContractorParameter);
    Task<string> ExcelUploadTest(ContractorParameter ContractorParameter);
    Task<string> ContractorPsUpload(ContractorParameter ContractorParameter);

    Task<IEnumerable<ConstructorTeamList>> GetConstructorTeam(ContractorParameter ContractorParameter);
    Task<IEnumerable<ConstructorTeamList>> LotPersonFilter(ContractorParameter ContractorParameter);
    Task<IEnumerable<ExcelLotDataDto>> GetCbcExcelLotData(ContractorParameter ContractorParameter);
    Task<string> ConstructorWorkFlowDelete(ContractorParameter ContractorParameter);
    Task<List<ContractorsCompanyList>> GetContractorsByLotId(ContractorParameter ContractorParameter);

    Task<IEnumerable<ContractorsCompanyList>>
        GetContractorsByLotIdForZeroState(ContractorParameter ContractorParameter);

    Task<List<string>> GetContractorsErrorListByLotId(ContractorParameter ContractorParameter);
    Task<List<ExcelLotDataDtoTest>> GetCbcExcelLotDataTest(ContractorParameter ContractorParameter);
    Task<List<ExcelLotDataDtoTest>> GetCbcExcelLotDataTestForZeroState(ContractorParameter ContractorParameter);
    Task<string> PublishTender(ContractorParameter ContractorParameter, IGraphRepository IGraphRepository);
    Task<string> ApproveInvitation(ContractorParameter ContractorParameter);
    Task<string> CrateCommentCard(ContractorParameter ContractorParameter);
    Task<string> AddComment(ContractorParameter ContractorParameter, ISendGridRepositorie ISendGridRepositorie);
    Task<IEnumerable<CommentCardDto>> GetComment(ContractorParameter ContractorParameter);
    Task<string> ContractorLotExcelUpload(ContractorParameter ContractorParameter);
    Task<string> SendInvitation(ContractorParameter ContractorParameter, ISendGridRepositorie ISendGridRepositorie);
    Task<string> LotPublish(ContractorParameter ContractorParameter);
    Task<string> AcceptComment(ContractorParameter ContractorParameter);
    Task<CommentLogDropDownData> GetCommentLogDropDownData(ContractorParameter ContractorParameter);
    Task<string> UpdateCommentLogDropDown(ContractorParameter ContractorParameter);
    Task<List<ContractorsCompanyList>> GetContractorsByLotIdFilterContractor(ContractorParameter ContractorParameter);
    Task<List<ExcelLotDataDtoTest>> GetCbcExcelLotDataFilterContractor(ContractorParameter ContractorParameter);
    Task<IEnumerable<ContractList>> GetLotByUser(ContractorParameter ContractorParameter);
    Task<IEnumerable<GetContractingUnitByUser>> GetContractingUnitByUser(ContractorParameter ContractorParameter);
    Task<IEnumerable<GetProjectByUser>> GetProjectsByUser(ContractorParameter ContractorParameter);

    Task<IEnumerable<ContractorListDto>> ContractorFilterGetLots(ContractorParameter ContractorParameter);
    Task<GetLotTotalPrices> GetLotTotalPriceById(ContractorParameter ContractorParameter);

    Task<string> AddTenderAwardWinner(ContractorParameter ContractorParameter,
        ISendGridRepositorie ISendGridRepositorie);

    Task<string> ContractorLotUploadDocuments(ContractorParameter ContractorParameter);
    Task<List<GetContractorLotUploadedDocs>> GetContractorLotUploadDocuments(ContractorParameter ContractorParameter);
    Task<GetUserInformationDto> GetUserInformation(ContractorParameter ContractorParameter);
    Task<GetUserTypeDto> GetLoggedUserType(ContractorParameter ContractorParameter);
    Task<GetContractorByIdForMailDto> GetContractorByIdForMail(ContractorParameter ContractorParameter);
    Task<List<DownloadLotLinks>> DownloadLotDocuments(ContractorParameter ContractorParameter);
    Task<string> SubscribeLot(ContractorParameter ContractorParameter);
    Task<GetContractorByIdForMailDto> GetContractorByIdForSubscribMail(ContractorParameter ContractorParameter);
    Task<List<CBCExcelLotDataParent>> GetLotCbcTree(ContractorParameter ContractorParameter);
    Task<IEnumerable<ContractorsPsList>> GetPsSequenceIdByLotIdForZeroState(ContractorParameter ContractorParameter);
    Task<List<ExcelLotDataDtoTest>> GetContractorPsDataForZeroState(ContractorParameter ContractorParameter);
    Task<List<ContractorPdfErrorLog>> ContractorPsErrorLogForZeroState(ContractorParameter ContractorParameter);
    Task<string> SaveContractorPsData(ContractorParameter ContractorParameter);
    Task<string> CreateCommentCardForPs(ContractorParameter ContractorParameter);
    Task<IEnumerable<CommentCardDto>> GetPsComment(ContractorParameter ContractorParameter);
    Task<string> AddCommentForPs(ContractorParameter ContractorParameter, ISendGridRepositorie ISendGridRepositorie);
    Task<string> UpdateCommentLogDropDownForPs(ContractorParameter ContractorParameter);
    Task<string> ApproveCommentForPs(ContractorParameter ContractorParameter);

    Task<IEnumerable<GetContractorPsOrderNumber>> ContractorPsOrderNumberDropDown(
        ContractorParameter ContractorParameter);

    Task<string> PublishContractorPs(ContractorParameter ContractorParameter);
    Task<string> ApproveContractorPs(ContractorParameter ContractorParameter);
    Task<string> ContractorPsMinusPlusWork(ContractorParameter ContractorParameter);
    Task<List<GetContractorPsMinusPlusWork>> GetContractorPsMinusPlusWork(ContractorParameter ContractorParameter);

    public Task<List<GetContractorPsMinusPlusWork>> UpdateContractorPsMinusPlusWorkTotalPrice(
        ContractorParameter ContractorParameter);

    Task<string> ApproveContractorPsMinusPlusWork(ContractorParameter ContractorParameter);

    Task<string> ZipDownload(ContractorParameter ContractorParameter);
    Task<List<ContractorTeamList>> GetContractorListByLotId(ContractorParameter ContractorParameter);
    Task<List<GetTotalPriceErrorsDto>> GetContractorTotalPriceErrorsByLotId(ContractorParameter contractorParameter);
    Task<List<IsUnresolvedCommentDto>> IsUnresolvedComment(ContractorParameter ContractorParameter);
    Task<List<ContractorStatusDto>> GetAwardedLotInProject(ContractorParameter ContractorParameter);
    Task<List<AwardedLotDataResult>> GetAwardedContractorLotData(ContractorParameter ContractorParameter);


}

public class ContractorParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public List<string> IdList { get; set; }
    public string UserId { get; set; }
    public ContractorHeaderDto BMLotHeaderDto { get; set; }
    public ContractorFilterDto Filter { get; set; }
    public ConstructorLotInfoDto ConstructorLotInfoDto { get; set; }
    public CBCExcelLotDataDto UploadExcelDto { get; set; }
    public ConstructorTeamListFilter PersonFilter { get; set; }
    public GraphServiceClient GraphServiceClient { get; set; }
    public ConstructorWorkFlowDelete ConstructorWorkFlowDelete { get; set; }
    public IFormFile File { get; set; }
    public AcceptInvitationDto AcceptInvitationDto { get; set; }
    public CommentCard CommentCard { get; set; }
    public ContractorComment ContractorComment { get; set; }
    public ContractorLotExcelData ContractorLotExcelData { get; set; }
    public ContractorTeam ContractorTeam { get; set; }
    public IPdfToExcelRepository _pdfToExcelRepository { get; set; }
    public AcceptComment AcceptComment { get; set; }
    public CommentCardContractorDto CommentCardContractorDto { get; set; }
    public CommentFilter CommentFilter { get; set; }
    public AwardWinner AwardWinner { get; set; }
    public FilterByUser FilterByUser { get; set; }
    public ContractorLotUploadedDocs ContractorLotUploadedDocs { get; set; }
    public IConfiguration Configuration { get; set; }

    public ISendGridRepositorie SendGridRepositorie { get; set; }
    public DownloadLotDocsDto DownloadLotDocsDto { get; set; }
    public IGraphRepository GraphRepository { get; set; }

    public UploadContractorPs ContractorPsUploadDto { get; set; }
    public SaveContractorPs PsData { get; set; }

    public CreateCommentCardPs CommentCardPs { get; set; }
    public string lotId { get; set; }
    public string psSequenceId { get; set; }
    public string DocumentCategory { get; set; }

    public ContractorPsMinusPlusWorkDto MinusPlusWorkDto { get; set; }

    public List<GetContractorPsMinusPlusWork> GetContractorPsMinusPlusWork { get; set; }

    public IFormCollection LotFile { get; set; }
    
    public AwardedLotDataDto AwardedLotDataDto { get; set; }

}