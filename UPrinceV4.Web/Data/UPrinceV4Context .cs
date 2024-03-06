using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Category;
using UPrinceV4.Web.Data.CIAW;
using UPrinceV4.Web.Data.Comment;
using UPrinceV4.Web.Data.Contract;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.GL;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.INV;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.PriceCalculator;
using UPrinceV4.Web.Data.ProjectClassification;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Data.RFQ;
using UPrinceV4.Web.Data.StandardMails;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.TAX;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Data.Translations;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Data.WH;

namespace UPrinceV4.Web.Data;

public class UPrinceV4Context : DbContext
{
    // private IConfiguration Configuration { get; }

    public DbSet<LocaleName> Locales { get; set; }

    public DbSet<Roles> Role { get; set; }

    // public DbSet<Users> AllUsers { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    public DbSet<ProjectKPI> ProjectKPI { get; set; }
    public DbSet<ProjectTime> ProjectTime { get; set; }
    public DbSet<ProjectHistoryLog> ProjectHistoryLog { get; set; }
    public DbSet<ProjectFinance> ProjectFinance { get; set; }
    public DbSet<ProjectDefinition> ProjectDefinition { get; set; }
    public DbSet<ProjectToleranceState> ProjectToleranceState { get; set; }
    public DbSet<ProjectType> ProjectType { get; set; }
    public DbSet<ProjectTemplate> ProjectTemplate { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<ProjectState> ProjectState { get; set; }
    public DbSet<CalendarTemplate> CalendarTemplate { get; set; }
    public DbSet<ProjectManagementLevel> ProjectManagementLevel { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<QRCode> QRCode { get; set; }
    public DbSet<TimeClock> TimeClock { get; set; }
    public DbSet<LocalizedData> LocalizedData { get; set; }
    public DbSet<AppConfigurationData> AppConfigurationData { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<TimeClockActivityType> TimeClockActivityType { get; set; }
    public DbSet<WorkflowState> WorkflowState { get; set; }

    public DbSet<LastSeenProjectDefinition> LastSeenProjectDefinition { get; set; }
    public DbSet<ProjectDefinitionHistoryLog> ProjectDefinitionHistoryLog { get; set; }
    public DbSet<Properties> Properties { get; set; }
    public DbSet<ErrorMessage> ErrorMessage { get; set; }
    public DbSet<CorporateProductCatalog> CorporateProductCatalog { get; set; }
    public DbSet<CpcHistoryLog> CpcHistoryLog { get; set; }
    public DbSet<CpcResourceFamily> CpcResourceFamily { get; set; }
    public DbSet<CpcResourceNickname> CpcResourceNickname { get; set; }
    public DbSet<CpcResourceType> CpcResourceType { get; set; }
    public DbSet<CpcImage> CpcImage { get; set; }
    public DbSet<CpcVendor> CpcVendor { get; set; }
    public DbSet<CpcBasicUnitOfMeasure> CpcBasicUnitOfMeasure { get; set; }
    public DbSet<CabAddress> CabAddress { get; set; }
    public DbSet<CabHistoryLog> CabHistoryLog { get; set; }
    public DbSet<CabCompany> CabCompany { get; set; }
    public DbSet<CabPerson> CabPerson { get; set; }
    public DbSet<CabPersonCompany> CabPersonCompany { get; set; }
    public DbSet<Country> Country { get; set; }
    public DbSet<CabEmail> CabEmail { get; set; }
    public DbSet<CabLandPhone> CabLandPhone { get; set; }
    public DbSet<CabMobilePhone> CabMobilePhone { get; set; }
    public DbSet<CabWhatsApp> CabWhatsApp { get; set; }
    public DbSet<CabSkype> CabSkype { get; set; }
    public DbSet<CabBankAccount> CabBankAccount { get; set; }
    public DbSet<CabVat> CabVat { get; set; }
    public DbSet<CpcMaterial> CpcMaterial { get; set; }
    public DbSet<CpcPressureClass> CpcPressureClass { get; set; }
    public DbSet<CpcUnitOfSizeMeasure> CpcUnitOfSizeMeasure { get; set; }
    public DbSet<ConsumableForPbs> PbsConsumable { get; set; }
    public DbSet<LabourForPbs> PbsLabour { get; set; }
    public DbSet<MaterialForPbs> PbsMaterial { get; set; }
    public DbSet<PbsExperience> PbsExperience { get; set; }
    public DbSet<PbsInstruction> PbsInstruction { get; set; }
    public DbSet<PbsProduct> PbsProduct { get; set; }
    public DbSet<PbsProductItemType> PbsProductItemType { get; set; }
    public DbSet<PbsProductStatus> PbsProductStatus { get; set; }
    public DbSet<PbsSkillExperience> PbsSkillExperience { get; set; }
    public DbSet<PbsSkill> PbsSkill { get; set; }
    public DbSet<ToolsForPbs> PbsTools { get; set; }
    public DbSet<Risk> Risk { get; set; }
    public DbSet<RiskType> RiskType { get; set; }
    public DbSet<RiskStatus> RiskStatus { get; set; }
    public DbSet<Log> Log { get; set; }
    public DbSet<Quality> Quality { get; set; }
    public DbSet<PbsRisk> PbsRisk { get; set; }
    public DbSet<PbsQuality> PbsQuality { get; set; }
    public DbSet<CpcBrand> CpcBrand { get; set; }
    public DbSet<PbsProductTaxonomy> PbsProductTaxonomy { get; set; }
    public DbSet<PbsTaxonomy> PbsTaxonomy { get; set; }
    public DbSet<PbsQualityResponsibility> PbsQualityResponsibility { get; set; }
    public DbSet<PbsToleranceState> PbsToleranceState { get; set; }
    public DbSet<PbsInstructionFamily> PbsInstructionFamily { get; set; }
    public DbSet<PbsInstructionLink> PbsInstructionLink { get; set; }
    public DbSet<PbsHistoryLog> PbsHistoryLog { get; set; }
    public DbSet<QualityHistoryLog> QualityHistoryLog { get; set; }
    public DbSet<RiskHistoryLog> RiskHistoryLog { get; set; }
    public DbSet<PbsTaxonomyLevel> PbsTaxonomyLevel { get; set; }
    public DbSet<BorConsumable> BorConsumable { get; set; }
    public DbSet<BorLabour> BorLabour { get; set; }
    public DbSet<BorMaterial> BorMaterial { get; set; }
    public DbSet<Bor> Bor { get; set; }
    public DbSet<BorTools> BorTools { get; set; }
    public DbSet<BorRequiredConsumable> BorRequiredConsumable { get; set; }
    public DbSet<BorRequiredTools> BorRequiredTools { get; set; }
    public DbSet<BorRequiredLabour> BorRequiredLabour { get; set; }
    public DbSet<BorRequiredMaterial> BorRequiredMaterial { get; set; }
    public DbSet<ProjectToleranceStateLocalizedData> ProjectToleranceStateLocalizedData { get; set; }
    public DbSet<ProjectStateLocalizedData> ProjectStateLocalizedData { get; set; }
    public DbSet<ProjectTypeLocalizedData> ProjectTypeLocalizedData { get; set; }
    public DbSet<ProjectManagementLevelLocalizedData> ProjectManagementLevelLocalizedData { get; set; }
    public DbSet<ProjectTemplateLocalizedData> ProjectTemplateLocalizedData { get; set; }
    public DbSet<AllProjectAttributes> AllProjectAttributes { get; set; }
    public DbSet<CpcMaterialLocalizedData> CpcMaterialLocalizedData { get; set; }
    public DbSet<CpcBasicUnitOfMeasureLocalizedData> CpcBasicUnitOfMeasureLocalizedData { get; set; }
    public DbSet<CpcResourceFamilyLocalizedData> CpcResourceFamilyLocalizedData { get; set; }
    public DbSet<CpcResourceTypeLocalizedData> CpcResourceTypeLocalizedData { get; set; }
    public DbSet<PbsExperienceLocalizedData> PbsExperienceLocalizedData { get; set; }
    public DbSet<PbsProductItemTypeLocalizedData> PbsProductItemTypeLocalizedData { get; set; }
    public DbSet<PbsProductStatusLocalizedData> PbsProductStatusLocalizedData { get; set; }
    public DbSet<PbsSkillLocalizedData> PbsSkillLocalizedData { get; set; }
    public DbSet<PbsTaxonomyLocalizedData> PbsTaxonomyLocalizedData { get; set; }
    public DbSet<PbsToleranceStateLocalizedData> PbsToleranceStateLocalizedData { get; set; }
    public DbSet<PbsTaxonomyLevelLocalizedData> PbsTaxonomyLevelLocalizedData { get; set; }
    public DbSet<TimeClockActivityTypeLocalizedData> TimeClockActivityTypeLocalizedData { get; set; }
    public DbSet<PbsScope> PbsScope { get; set; }
    public DbSet<Pmol> PMol { get; set; }
    public DbSet<PmolPlannedWorkConsumable> PMolPlannedWorkConsumable { get; set; }
    public DbSet<PmolInstruction> PMolInstruction { get; set; }
    public DbSet<PmolInstructionLink> PMolInstructionLink { get; set; }
    public DbSet<PmolExtraWork> PMolExtraWork { get; set; }
    public DbSet<PmolExtraWorkFiles> PMolExtraWorkFiles { get; set; }
    public DbSet<PmolHistoryLog> PMolHistoryLog { get; set; }
    public DbSet<PmolJournal> PMolJournal { get; set; }
    public DbSet<PmolJournalPicture> PMolJournalPicture { get; set; }
    public DbSet<PmolPlannedWorkLabour> PMolPlannedWorkLabour { get; set; }
    public DbSet<PmolPlannedWorkMaterial> PMolPlannedWorkMaterial { get; set; }
    public DbSet<PmolRisk> PMolRisk { get; set; }
    public DbSet<PmolStartHandshake> PMolStartHandshake { get; set; }
    public DbSet<PmolStatus> PMolStatus { get; set; }
    public DbSet<PmolStopHandshake> PMolStopHandshake { get; set; }
    public DbSet<PmolStopHandshakeDocument> PMolStopHandshakeDocument { get; set; }
    public DbSet<PmolPlannedWorkTools> PMolPlannedWorkTools { get; set; }
    public DbSet<PmolType> PMolType { get; set; }
    public DbSet<PmolShortcutpaneData> PMolShortcutpaneData { get; set; }
    public DbSet<ProjectUserRoleFeature> ProjectUserRoleFeature { get; set; }
    public DbSet<ProjectUserRole> ProjectUserRole { get; set; }
    public DbSet<ProjectFeature> ProjectFeature { get; set; }
    public DbSet<PmolQuality> PMolQuality { get; set; }
    public DbSet<ProjectTeam> ProjectTeam { get; set; }
    public DbSet<ProjectTeamRole> ProjectTeamRole { get; set; }
    public DbSet<ContractingUnitUserRole> ContractingUnitUserRole { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<BorHistoryLog> BorHistoryLog { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<BoundingPoint> BoundingPoint { get; set; }
    public DbSet<DataSources> DataSources { get; set; }
    public DbSet<Geometry> Geometry { get; set; }
    public DbSet<MapLocation> MapLocation { get; set; }
    public DbSet<Position> Position { get; set; }
    public DbSet<PmolTeamRole> PmolTeamRole { get; set; }
    public DbSet<ProjectCostConversion> ProjectCostConversion { get; set; }
    public DbSet<ProjectCost> ProjectCost { get; set; }
    public DbSet<ResourceTypePriceList> ResourceTypePriceList { get; set; }
    public DbSet<ResourceItemPriceList> ResourceItemPriceList { get; set; }
    public DbSet<PsHeader> PsHeader { get; set; }
    public DbSet<PsCustomer> PsCustomer { get; set; }
    public DbSet<PsResource> PsResource { get; set; }
    public DbSet<PsType> PsType { get; set; }
    public DbSet<PsStatus> PsStatus { get; set; }
    public DbSet<PsHistoryLog> PsHistoryLog { get; set; }
    public DbSet<Translation> Translation { get; set; }
    public DbSet<QrHistoryLog> QrHistoryLog { get; set; }
    public DbSet<WebTranslation> WebTranslation { get; set; }
    public DbSet<Language> Language { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<InvoiceStatus> InvoiceStatus { get; set; }
    public DbSet<InvoiceHistoryLog> InvoiceHistoryLog { get; set; }
    public DbSet<GeneralLedgerNumber> GeneralLedger { get; set; }
    public DbSet<Tax> Tax { get; set; }
    public DbSet<POHeader> POHeader { get; set; }
    public DbSet<POType> POType { get; set; }
    public DbSet<POStatus> POStatus { get; set; }
    public DbSet<POResources> POResources { get; set; }
    public DbSet<POResourcesDocument> POResourcesDocument { get; set; }
    public DbSet<POProduct> POProduct { get; set; }
    public DbSet<POResourcesDocumentC> POResourcesDocumentC { get; set; }
    public DbSet<TimeClockHistory> TimeClockHistory { get; set; }
    public DbSet<WHHeader> WHHeader { get; set; }
    public DbSet<WHHistoryLog> WHHistoryLog { get; set; }
    public DbSet<WHStatus> WHStatus { get; set; }
    public DbSet<WHTaxonomy> WHTaxonomy { get; set; }
    public DbSet<WHType> WHType { get; set; }
    public DbSet<WFActivity> WFActivity { get; set; }
    public DbSet<WFActivityStatus> WFActivityStatus { get; set; }
    public DbSet<WFDestinationTaxonomy> WFDestinationTaxonomy { get; set; }
    public DbSet<WFHeader> WFHeader { get; set; }
    public DbSet<WFHistoryLog> WFHistory { get; set; }
    public DbSet<WFSourceTaxonomy> WFSourceTaxonomy { get; set; }
    public DbSet<WFTask> WFTask { get; set; }
    public DbSet<WFType> WFType { get; set; }
    public DbSet<StockActivityType> StockActivityType { get; set; }
    public DbSet<StockHeader> StockHeader { get; set; }
    public DbSet<StockHistoryLog> StockHeaderHistoryLog { get; set; }
    public DbSet<StockActivityHistory> StockHistoryLog { get; set; }
    public DbSet<StockStatus> StockStatus { get; set; }
    public DbSet<StockType> StockType { get; set; }
    public DbSet<StockShortCutPane> StockShortCutPane { get; set; }
    public DbSet<WHShortCutPane> WHShortCutPane { get; set; }
    public DbSet<WFShortCutPane> WFShortCutPane { get; set; }
    public DbSet<WHTaxonomyLevel> WHTaxonomyLevel { get; set; }
    public DbSet<WFDocument> WFDocument { get; set; }
    public DbSet<StockActiveType> StockActiveType { get; set; }
    public DbSet<ProjectFinancialStatus> ProjectFinancialStatus { get; set; }
    public DbSet<ProjectScopeStatus> ProjectScopeStatus { get; set; }
    public DbSet<PmolBreak> PmolBreak { get; set; }
    public DbSet<POCpcAverage> POCpcAverage { get; set; }
    public DbSet<PbsService> PbsService { get; set; }
    public DbSet<ServiceDocuments> ServiceDocuments { get; set; }
    public DbSet<PmolService> PmolService { get; set; }
    public DbSet<PmolServiceDocuments> PmolServiceDocuments { get; set; }
    public DbSet<PORequestType> PORequestType { get; set; }
    public DbSet<BorType> BorType { get; set; }
    public DbSet<CompetenciesTaxonomyLevel> CompetenciesTaxonomyLevel { get; set; }
    public DbSet<CompetenciesTaxonomy> CompetenciesTaxonomy { get; set; }
    public DbSet<Competencies> Competencies { get; set; }
    public DbSet<CategoryLevel> CategoryLevel { get; set; }
    public DbSet<VPOrganisationShortcutPane> VPOrganisationShortcutPane { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<Comments> PostComments { get; set; }
    public DbSet<Pictures> PostPictures { get; set; }
    public DbSet<VpPo> VpPo { get; set; }
    public DbSet<CabCompetencies> CabCompetencies { get; set; }
    public DbSet<CabCertification> CabCertification { get; set; }
    public DbSet<OrganizationCompetencies> OrganizationCompetencies { get; set; }
    public DbSet<OrganizationCertification> OrganizationCertification { get; set; }
    public DbSet<CertificationTaxonomy> CertificationTaxonomy { get; set; }
    public DbSet<CertificationTaxonomyLevel> CertificationTaxonomyLevel { get; set; }
    public DbSet<CorporateSheduleTime> CorporateSheduleTime { get; set; }
    public DbSet<CorporateShedule> CorporateShedule { get; set; }
    public DbSet<Organization> Organization { get; set; }
    public DbSet<OrganizationTaxonomyLevel> OrganizationTaxonomyLevel { get; set; }
    public DbSet<OrganizationTaxonomy> OrganizationTaxonomy { get; set; }
    public DbSet<AbsenceHeader> AbsenceHeader { get; set; }
    public DbSet<AbsenceType> AbsenceType { get; set; }
    public DbSet<HRHeader> HRHeader { get; set; }
    public DbSet<POLabourTeam> POLabourTeam { get; set; }
    public DbSet<HRRoles> HRRoles { get; set; }
    public DbSet<WorkSchedule> WorkSchedule { get; set; }
    public DbSet<AbsenceLeaveType> AbsenceLeaveType { get; set; }
    public DbSet<POToolPool> POToolPool { get; set; }
    public DbSet<VpHR> VpHR { get; set; }
    public DbSet<MilestoneHeader> MilestoneHeader { get; set; }
    public DbSet<MilestoneDocuments> MilestoneDocuments { get; set; }
    public DbSet<MilestoneType> MilestoneType { get; set; }
    public DbSet<MilestoneStatus> MilestoneStatus { get; set; }
    public DbSet<VpWH> VpWH { get; set; }
    public DbSet<MachineTaxonmy> MachineTaxonmy { get; set; }
    public DbSet<MachineTaxonomyLevel> MachineTaxonomyLevel { get; set; }
    public DbSet<PmolLabourTeams> PmolLabourTeams { get; set; }
    public DbSet<PmolToolsPool> PmolToolsPool { get; set; }
    public DbSet<VpFilterDropdown> VpFilterDropdown { get; set; }
    public DbSet<ContractTaxonomy> ContractTaxonomy { get; set; }
    public DbSet<ProjectClassificationBuisnessUnit> ProjectClassificationBuisnessUnit { get; set; }
    public DbSet<ProjectClassificationSize> ProjectClassificationSize { get; set; }
    public DbSet<ProjectClassificationConstructionType> ProjectClassificationConstructionType { get; set; }
    public DbSet<ProjectClassificationSector> ProjectClassificationSector { get; set; }
    public DbSet<ProjectClassificationHeader> ProjectClassification { get; set; }
    public DbSet<ProjectLanguage> ProjectLanguage { get; set; }
    public DbSet<ContractorStatus> ContractorStatus { get; set; }
    public DbSet<ContractorFileType> ContractorFileType { get; set; }
    public DbSet<ContractorHeader> ContractorHeader { get; set; }
    public DbSet<ContractorsList> ContractorsList { get; set; }
    public DbSet<ContractorTechInstructionsDocs> ContractorTechInstructionsDocs { get; set; }
    public DbSet<ContractorTenderDocs> ContractorTenderDocs { get; set; }
    public DbSet<ContractorProvisionalAcceptenceDocs> ContractorProvisionalAcceptenceDocs { get; set; }
    public DbSet<ContractorFinalDeliveryDocs> ContractorFinalDeliveryDocs { get; set; }
    public DbSet<ContractorTechDocs> ContractorTechDocs { get; set; }
    public DbSet<ContractorTeamList> ContractorTeamList { get; set; }
    public DbSet<ContractorTenderAward> ContractorTenderAward { get; set; }
    public DbSet<ContractorHistoryLog> ContractorHistoryLog { get; set; }
    public DbSet<TeamsWithPmol> TeamsWithPmol { get; set; }
    public DbSet<ContractorProductItemType> ContractorProductItemType { get; set; }
    public DbSet<StandardMailHeader> StandardMailHeader { get; set; }
    public DbSet<ConstructorWorkFlow> ConstructorWorkFlow { get; set; }
    public DbSet<ContractorForLot> ContractorForLot { get; set; }
    public DbSet<ContractorAccreditation> ContractorAccreditation { get; set; }
    public DbSet<ContractorSupplierList> ContractorSupplierList { get; set; }
    public DbSet<ContractorWorkFlow> ContractorWorkFlow { get; set; }
    public DbSet<CabContractorTaxonomycs> CabContractorTaxonomycs { get; set; }
    public DbSet<ConstructorWorkFlowStatus> ConstructorWorkFlowStatus { get; set; }
    public DbSet<CBCExcelLotData> CBCExcelLotData { get; set; }
    public DbSet<ConstructorTeam> ConstructorTeam { get; set; }
    public DbSet<ContractorPdfData> ContractorPdfData { get; set; }
    public DbSet<ContractorHasTaxonony> ContractorHasTaxonony { get; set; }
    public DbSet<ContractorPdfRowData> ContractorPdfRowData { get; set; }
    public DbSet<ContractorComment> ContractorComment { get; set; }
    public DbSet<CommentCard> CommentCard { get; set; }
    public DbSet<CommentCardContractor> CommentCardContractor { get; set; }
    public DbSet<PublishedContractorsPdfData> PublishedContractorsPdfData { get; set; }
    public DbSet<CommentLogPriority> CommentLogPriority { get; set; }
    public DbSet<CommentLogSeverity> CommentLogSeverity { get; set; }
    public DbSet<CommentLogStatus> CommentLogStatus { get; set; }
    public DbSet<CommentLogField> CommentLogField { get; set; }
    public DbSet<CBCExcelLotdataPublished> CBCExcelLotdataPublished { get; set; }
    public DbSet<ContractorTotalValues> ContractorTotalValues { get; set; }

    public DbSet<ContractorLotUploadedDocs> ContractorLotUploadedDocs { get; set; }
    public DbSet<ContractorPdfOrginalData> ContractorPdfOrginalData { get; set; }
    public DbSet<ConstructorWfStatusChangeTime> ConstructorWfStatusChangeTime { get; set; }
    public DbSet<LotStatusChangeTime> LotStatusChangeTime { get; set; }
    public DbSet<ContractorTotalValuesPublished> ContractorTotalValuesPublished { get; set; }
    public DbSet<CommentChangeType> CommentChangeType { get; set; }
    public DbSet<ContractorUploadedFiles> ContractorUploadedFiles { get; set; }
    public DbSet<PbsTreeIndex> PbsTreeIndex { get; set; }
    public DbSet<ContractorPs> ContractorPs { get; set; }
    public DbSet<OrganizationTeamRole> OrganizationTeamRole { get; set; }
    public DbSet<Instructions> Instructions { get; set; }
    public DbSet<ContractorPsPublished> ContractorPsPublished { get; set; }
    public DbSet<OrganizationTeamPmol> OrganizationTeamPmol { get; set; }
    public DbSet<CommentCardPs> CommentCardPs { get; set; }
    public DbSet<CommentCardContractorPs> CommentCardContractorPs { get; set; }
    public DbSet<ContractorPsOrderNumber> ContractorPsOrderNumber { get; set; }
    public DbSet<TeamWithPmol> TeamWithPmol { get; set; }
    public DbSet<TeamWithPerson> TeamWithPerson { get; set; }
    public DbSet<CBCDynamicsAttributes> CBCDynamicsAttributes { get; set; }
    public DbSet<OrganizationTeamVehicel> OrganizationTeamVehicel { get; set; }
    public DbSet<PmolLabourTime> PmolLabourTime { get; set; }
    public DbSet<PmolLabourBreak> PmolLabourBreak { get; set; }
    public DbSet<UserCurrentPmol> UserCurrentPmol { get; set; }
    public DbSet<ContractorPsMinusPlusWork> ContractorPsMinusPlusWork { get; set; }
    public DbSet<POHistoryLog> POHistoryLog { get; set; }
    public DbSet<WFHistoryLogFroPO> WFHistoryLogFroPO { get; set; }
    public DbSet<PbsScopeOfWork> PbsScopeOfWork { get; set; }
    public DbSet<PmolRfq> PmolRfq { get; set; }
    public DbSet<CiawHeader> CiawHeader { get; set; }
    public DbSet<CiawStatus> CiawStatus { get; set; }
    public DbSet<RfqSignatures> RfqSignatures { get; set; }
    public DbSet<ProjectCiawSite> ProjectCiawSite { get; set; }
    public DbSet<CabNationality> CabNationality { get; set; }
    public DbSet<Nationality> Nationality { get; set; }
    public DbSet<PmolAssignTime> PmolAssignTime { get; set; }
    public DbSet<WHRockCpc> WHRockCpc { get; set; }
    public DbSet<PmolPersonCommentCard> PmolPersonCommentCard { get; set; }
    public DbSet<WHRockCpcImage> WHRockCpcImage { get; set; }
    public DbSet<PmolPersonComment> PmolPersonComment { get; set; }
    public DbSet<CpcVehicleTrackingNo> CpcVehicleTrackingNo { get; set; }
    public DbSet<TemporaryTeamName> TemporaryTeamName { get; set; }
    public DbSet<CiawError> CiawError { get; set; }
    public DbSet<ProjectConfiguration> ProjectConfiguration { get; set; }
    public DbSet<CiawCancelStatus> CiawCancelStatus { get; set; }
    public DbSet<ExactOnline.ExactOnline> ExactOnline { get; set; }
    public DbSet<CiawResponseJson> CiawResponseJson { get; set; }
    public DbSet<CiawCancelJson> CiawCancelJson { get; set; }
    public DbSet<ThProductWithTrucks> ThProductWithTrucks { get; set; }
    public DbSet<ThTrucksSchedule> ThTrucksSchedule { get; set; }
    public DbSet<PbsDynamicAttributes> PbsDynamicAttributes { get; set; }
    public DbSet<CPCVelocity> CPCVelocity { get; set; }
    public DbSet<ThCustomerOrganizations> ThCustomerOrganizations { get; set; }
    public DbSet<WeTransfer> WeTransfer { get; set; }
    public DbSet<ThTest> ThTest { get; set; }
    public DbSet<PriceCalculatorTaxonomy> PriceCalculatorTaxonomy { get; set; }
    public DbSet<PriceCalculatorTaxonomyLevel> PriceCalculatorTaxonomyLevel { get; set; }
    public DbSet<ProjectDefaultMembers> ProjectDefaultMembers { get; set; }
    public DbSet<PmolCbcResources> PmolCbcResources { get; set; }
    public DbSet<WorfFlowStatusLocalizedData> WorfFlowStatusLocalizedData { get; set; }




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBulder)
    {
        //uprincev4staging

        //optionsBulder.UseSqlServer("Server=tcp:uprincev4einstein.database.windows.net,1433;Initial Catalog=UPrinceV4ProjectTemplate;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //optionsBulder.UseSqlServer("Server=tcp:192.168.1.4,1433;Initial Catalog=UPRINCE;User ID=sa;Password=reallyStrongPwd123;");
        //optionsBulder.UseSqlServer("Server=tcp:uprincev4staging.database.windows.net,1433;Initial Catalog=UPrinceV4Staging; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
        optionsBulder.UseSqlServer(
            "Server=tcp:uprincev4uatdb.database.windows.net,1433;Initial Catalog=P0057; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
        //optionsBulder.UseSqlServer(
        //    "Server=tcp:uprincev4training.database.windows.net,1433;Initial Catalog=P0057; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
        //optionsBulder.UseSqlServer("Server=tcp:bmengineering.database.windows.net,1433;Initial Catalog=UPrinceV4ProjectTemplate;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //optionsBulder.UseSqlServer("Server=tcp:uprincev4staging.database.windows.net,1433;Initial Catalog=ProjectTest;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //optionsBulder.UseSqlServer("Server=tcp:192.168.1.4,1433;Initial Catalog=UPRINCE;User ID=sa;Password=reallyStrongPwd123;");
        //optionsBulder.UseSqlServer("Server=tcp:uprincev4staging.database.windows.net,1433;Initial Catalog=COM0001; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
        // optionsBulder.UseSqlServer("Server=tcp:uprincev4staging.database.windows.net,1433;Initial Catalog=COM0002; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
        //optionsBulder.UseSqlServer("Server=tcp:bmengineering.database.windows.net,1433;Initial Catalog=COM-0002;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=True");
        //optionsBulder.UseSqlServer("Server = tcp:uprincev4training.database.windows.net, 1433; Initial Catalog = UPrinceV4ProjectTemplate; Persist Security Info = False; User ID = uprincedbuser; Password = UPrince2017!; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; ");
        // optionsBulder.UseSqlServer("Server=tcp:uprincev4einstein.database.windows.net,1433;Initial Catalog=UPrinceV4Einstein;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //optionsBulder.UseSqlServer("Server=tcp:uprincev4einstein.database.windows.net,1433;Initial Catalog=P0017;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ProjectDefinition>().HasOne(b => b.ProjectKpi).WithOne(a => a.ProjectDefinition)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ProjectDefinition>().HasOne(b => b.ProjectFinance).WithOne(a => a.ProjectDefinition)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ProjectDefinition>().HasOne(b => b.ProjectTime).WithOne(a => a.ProjectDefinition)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ProjectDefinition>().HasIndex(u => u.SequenceCode).IsUnique();
        builder.Entity<Country>().HasIndex(u => u.CountryName).IsUnique();
        builder.Entity<CabEmail>().HasIndex(u => u.EmailAddress).IsUnique();
        builder.Entity<CabLandPhone>().HasIndex(u => u.LandPhoneNumber).IsUnique();
        builder.Entity<CabMobilePhone>().HasIndex(u => u.MobilePhoneNumber).IsUnique();
        builder.Entity<CabWhatsApp>().HasIndex(u => u.WhatsAppNumber).IsUnique();
        builder.Entity<CabSkype>().HasIndex(u => u.SkypeNumber).IsUnique();
        builder.Entity<CorporateProductCatalog>().HasIndex(u => u.ResourceNumber).IsUnique();
        builder.Entity<Bor>().HasIndex(u => u.ItemId).IsUnique();
        builder.Entity<PbsProduct>().HasIndex(u => u.ProductId).IsUnique();
        builder.Entity<Pmol>().HasIndex(u => u.ProjectMoleculeId).IsUnique();
        builder.Entity<Shift>().HasMany(b => b.TimeClocks).WithOne(a => a.Shift).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CpcMaterial>().HasData(new
            { Id = "123d2354a-8d13-4041-b756-d25f1bc0e890", Name = "PVC", LocaleCode = "", DisplayOrder = 0 });
        builder.Entity<CpcMaterial>().HasData(new
            { Id = "123d2354a-8d13-4041-b756-d25f1bc0e444", Name = "PVC-U1", LocaleCode = "", DisplayOrder = 0 });

        builder.Entity<CpcBasicUnitOfMeasure>().HasData(new
            { Id = "3ba6dc81-dcdf-4749-b13f-482cvsje75262", Name = "m", LocaleCode = "", DisplayOrder = 0 });
        builder.Entity<CpcBasicUnitOfMeasure>().HasData(new
            { Id = "b48dkcn3-dcdf-cdfg-b13f-482cvsje75262", Name = "(stuk)", LocaleCode = "", DisplayOrder = 0 });

        builder.Entity<PbsProductItemType>().HasData(new
        {
            Id = "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Name = "External Products",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14"
        });
        builder.Entity<PbsProductItemType>().HasData(new
        {
            Id = "aa0c8e3c-f716-4f92-afee-851d485164da",
            Name = "Internal Products",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da"
        });

        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "d60aad0b-2e84-482b-ad25-618d80d49477",
            Name = "Pending Development",
            LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Name = "In Development",
            LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Name = "In Review",
            LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Name = "Approved",
            LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "4010e768-3e06-4702-b337-ee367a82addb",
            Name = "Handed Over",
            LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<CpcBrand>().HasData(new
            { Id = "013f0f14-2675-41e5-8219-ff91c9d2c688", Name = "Geberit", LocaleCode = "" });
        builder.Entity<CpcBrand>().HasData(new
            { Id = "141f28dd-6fea-4d76-a07c-7e7c65d52a3b", Name = "Mepla", LocaleCode = "" });


        builder.Entity<Properties>().HasData(new { Id = 5, Key = "PbsSequenceCode", Value = "PBS-0001" });
        builder.Entity<Properties>().HasData(new { Id = 6, Key = "PbsSequenceCode", Value = "PBS-0001" });
        builder.Entity<Properties>().HasData(new { Id = 8, Key = "BorSequenceCode", Value = "BOR-0001" });
        builder.Entity<Properties>().HasData(new { Id = 9, Key = "PmolSequenceCode", Value = "PMOL-0002" });
        builder.Entity<Properties>().HasData(new { Id = 12, Key = "PsSequenceCode", Value = "PS-0001" });
        builder.Entity<Properties>().HasData(new { Id = 13, Key = "InvoiceSequenceCode", Value = "INV-0001" });

        builder.Entity<PbsToleranceState>().HasData(new
        {
            Id = "004eb795-8bba-47e8-9049-d14774ab0b18",
            Name = "Within Tolerance (green)",
            LocaleCode = "ProjectToleranceState.csvWithin Tolerance (green)"
        });
        builder.Entity<PbsToleranceState>().HasData(new
        {
            Id = "8f33bdf6-7600-4ad7-b558-c98899c1e5b2",
            Name = "Out of Tolerance (red)",
            LocaleCode = "ProjectToleranceState.csvOut of Tolerance (red)"
        });
        builder.Entity<PbsToleranceState>().HasData(new
        {
            Id = "d9712fb3-02b6-4c2a-991c-ee904c87d8a8",
            Name = "Tolerance limit (orange)",
            LocaleCode = "ProjectToleranceState.csvTolerance limit (orange)"
        });

        builder.Entity<ErrorMessage>().HasData(new
        {
            Id = "NoProjectBreakdownStructureAvailable",
            Message = "No available project breakdown structure",
            LocaleCode = "NoProjectBreakdownStructureAvailable"
        });
        builder.Entity<ErrorMessage>().HasData(new
            { Id = "NoPmolAvailable", Message = "No available Pmol", LocaleCode = "NoPmolAvailable" });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1315,
            LanguageCode = "es",
            LocaleCode = "NoProjectBreakdownStructureAvailable12",
            Label = "No available project breakdown structure (es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1316,
            LanguageCode = "nl",
            LocaleCode = "NoProjectBreakdownStructureAvailable",
            Label = "No available project breakdown structure (nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1317,
            LanguageCode = "nl-BE",
            LocaleCode = "NoProjectBreakdownStructureAvailable",
            Label = "No available project breakdown structure (nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1632,
            LanguageCode = "zh",
            LocaleCode = "NoProjectBreakdownStructureAvailable",
            Label = "No available project breakdown structure (zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1633,
            LanguageCode = "es",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1634,
            LanguageCode = "nl",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1635,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1636,
            LanguageCode = "zh",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1637,
            LanguageCode = "es",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1638,
            LanguageCode = "nl",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1639,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1640,
            LanguageCode = "zh",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1641,
            LanguageCode = "es",
            LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1643,
            LanguageCode = "nl",
            LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1644,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1645,
            LanguageCode = "zh",
            LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1646,
            LanguageCode = "es",
            LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1647,
            LanguageCode = "nl",
            LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1648,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1649,
            LanguageCode = "zh",
            LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1650,
            LanguageCode = "es",
            LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1651,
            LanguageCode = "nl",
            LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1652,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1653,
            LanguageCode = "zh",
            LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1654,
            LanguageCode = "es",
            LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1655,
            LanguageCode = "nl",
            LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1656,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1657,
            LanguageCode = "zh",
            LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1658,
            LanguageCode = "es",
            LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1659,
            LanguageCode = "nl",
            LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1660,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1661,
            LanguageCode = "zh",
            LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1704, LanguageCode = "nl-BE", LocaleCode = "scope.inscope", Label = "In Scope(nl-BE)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1705, LanguageCode = "nl", LocaleCode = "scope.inscope", Label = "In Scope(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1706, LanguageCode = "zh", LocaleCode = "scope.inscope", Label = "In Scope(zh)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1707, LanguageCode = "nl-BE", LocaleCode = "scope.outofscope", Label = "Out of Scope(nl-BE)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1708, LanguageCode = "nl", LocaleCode = "scope.outofscope", Label = "Out of Scope(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1709, LanguageCode = "zh", LocaleCode = "scope.outofscope", Label = "Out of Scope(zh)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1894, LanguageCode = "nl", LocaleCode = "NoPmolAvailable", Label = "No available Pmol(nl)" });


        //Competencies Experience Dropdown
        builder.Entity<PbsExperience>().HasData(new
            { Id = "46e02a0c-4c87-437b-8342-b16c2fa6cf45", Name = "Novice", LocaleCode = "PbsExperienceNovice" });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "e2ce864c-564c-49a4-8860-b79dbbffb673", Name = "Beginner", LocaleCode = "PbsExperienceBeginner" });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "c98e47d8-6b1f-4bee-97a1-fd1207e3670d", Name = "Talented", LocaleCode = "PbsExperienceTalented" });
        builder.Entity<PbsExperience>().HasData(new
        {
            Id = "3417e806-8e97-46d3-adb6-34426cd93bf4",
            Name = "Intermediate",
            LocaleCode = "PbsExperienceIntermediate"
        });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "b08b0641-e260-4750-8141-3cd8c25f6344", Name = "Skilful", LocaleCode = "PbsExperienceSkilful" });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "ea27ee00-8b38-48b6-8cc7-6872dc3cf09c", Name = "Seasoned", LocaleCode = "PbsExperienceSeasoned" });
        builder.Entity<PbsExperience>().HasData(new
        {
            Id = "df186961-6453-4c42-af53-c8866684a075",
            Name = "Proficient",
            LocaleCode = "PbsExperienceProficient"
        });
        builder.Entity<PbsExperience>().HasData(new
        {
            Id = "ee146eff-0f1f-44b1-a6ba-73b267416973",
            Name = "Experienced",
            LocaleCode = "PbsExperienceExperienced"
        });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "42325533-9834-4fd8-ac51-5b4e02fc0494", Name = "Advanced", LocaleCode = "PbsExperienceAdvanced" });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "cec1293c-7f89-48ed-865c-65cc7cbe526f", Name = "Senior", LocaleCode = "PbsExperienceSenior" });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "8c4bd8eb-f087-4904-8507-0f494dcf7d86", Name = "Expert", LocaleCode = "PbsExperienceExpert" });

        builder.Entity<LocalizedData>().HasData(new
            { Id = 1663, LanguageCode = "nl", LocaleCode = "PbsExperienceNovice", Label = "Nieuweling" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1665, LanguageCode = "nl", LocaleCode = "PbsExperienceBeginner", Label = "Beginner" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1666, LanguageCode = "nl", LocaleCode = "PbsExperienceTalented", Label = "Getalenteerd" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1668, LanguageCode = "nl", LocaleCode = "PbsExperienceIntermediate", Label = "Intermediate(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1669, LanguageCode = "nl", LocaleCode = "PbsExperienceSkilful", Label = "Skilful(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1670, LanguageCode = "nl", LocaleCode = "PbsExperienceSeasoned", Label = "Seasoned(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1671, LanguageCode = "nl", LocaleCode = "PbsExperienceProficient", Label = "Proficient(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1672, LanguageCode = "nl", LocaleCode = "PbsExperienceExperienced", Label = "Experienced(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1673, LanguageCode = "nl", LocaleCode = "PbsExperienceAdvanced", Label = "Advanced(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1674, LanguageCode = "nl", LocaleCode = "PbsExperienceSenior", Label = "Senior(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1675, LanguageCode = "nl", LocaleCode = "PbsExperienceExpert", Label = "Expert(nl)" });

        //Competencies Skill Dropdown
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "8b145fdc-b666-488c-beec-f335627024601",
            Name = "Team Building Skills",
            LocaleCode = "PbsSkillTeam Building Skills",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "bfd3f176-cc91-4d01-b27f-bef8888fc21c1",
            Name = "Collaboration",
            LocaleCode = "PbsSkillCollaboration",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "0ffe382d-fe7d-4ac7-91b3-204570427c371",
            Name = "Communication",
            LocaleCode = "PbsSkillCommunication",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "8f992d6e-7fee-43a3-b06c-430fa4d9d8e41",
            Name = "Flexibility",
            LocaleCode = "PbsSkillFlexibility",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d1",
            Name = "Listening",
            LocaleCode = "PbsSkillListening",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1",
            Name = "Analytical Skills",
            LocaleCode = "PbsSkillAnalytical Skills",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "7fd2a1b0-c559-4727-a596-dbc0af7c171e1",
            Name = "Critical thinking",
            LocaleCode = "PbsSkillCritical thinking",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "a1e3c91b-a8cf-43b1-b551-8bba9f64c3351",
            Name = "Data analysis",
            LocaleCode = "PbsSkillData analysis",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b61",
            Name = "Numeracy",
            LocaleCode = "PbsSkillNumeracy",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "74e9f3ce-5338-467c-add0-ba7116cd300b1",
            Name = "Reporting",
            LocaleCode = "PbsSkillReporting",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1676,
            LanguageCode = "nl",
            LocaleCode = "PbsSkillTeam Building Skills",
            Label = "Team Building Skills(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1677, LanguageCode = "nl", LocaleCode = "PbsSkillCollaboration", Label = "Collaboration(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1678, LanguageCode = "nl", LocaleCode = "PbsSkillCommunication", Label = "Communication(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1679, LanguageCode = "nl", LocaleCode = "PbsSkillFlexibility", Label = "Flexibility(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1680, LanguageCode = "nl", LocaleCode = "PbsSkillListening", Label = "Listening(nl)" });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1681,
            LanguageCode = "nl",
            LocaleCode = "PbsSkillAnalytical Skills",
            Label = "Analytical Skills(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1682,
            LanguageCode = "nl",
            LocaleCode = "PbsSkillCritical thinking",
            Label = "Critical thinking(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1683, LanguageCode = "nl", LocaleCode = "PbsSkillData analysis", Label = "Data analysis(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1684, LanguageCode = "nl", LocaleCode = "PbsSkillNumeracy", Label = "Numeracy(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1685, LanguageCode = "nl", LocaleCode = "PbsSkillReporting", Label = "Reporting(nl)" });

        // Pbs Taxonomy Dropdown
        builder.Entity<PbsTaxonomy>().HasData(new
            { Id = "6e54725c-e396-4ce4-88f3-a6e9678a0389", Name = "Utility", LocaleCode = "PbsTaxonomy.Utility" });
        builder.Entity<PbsTaxonomy>().HasData(new
            { Id = "ab294299-f251-41a8-94bd-3ae0150df134", Name = "Location", LocaleCode = "PbsTaxonomy.Location" });
        builder.Entity<PbsTaxonomy>().HasData(new
            { Id = "b08d229c-0c03-4593-8bd6-652939986a22", Name = "Scope", LocaleCode = "PbsTaxonomy.Scope" });

        builder.Entity<LocalizedData>().HasData(new
            { Id = 1690, LanguageCode = "nl", LocaleCode = "PbsTaxonomy.Utility", Label = "Utility(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1691, LanguageCode = "nl", LocaleCode = "PbsTaxonomy.Location", Label = "Location(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1692, LanguageCode = "nl", LocaleCode = "PbsTaxonomy.Scope", Label = "Scope(nl)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1693, LanguageCode = "nl-BE", LocaleCode = "PbsTaxonomy.Utility", Label = "Utility(nl-BE)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1694, LanguageCode = "nl-BE", LocaleCode = "PbsTaxonomy.Location", Label = "Location(nl-BE)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1695, LanguageCode = "nl-BE", LocaleCode = "PbsTaxonomy.Scope", Label = "Scope(nl-BE)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1696, LanguageCode = "zh", LocaleCode = "PbsTaxonomy.Utility", Label = "Utility(zh)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1697, LanguageCode = "zh", LocaleCode = "PbsTaxonomy.Location", Label = "Location(zh)" });
        builder.Entity<LocalizedData>().HasData(new
            { Id = 1698, LanguageCode = "zh", LocaleCode = "PbsTaxonomy.Scope", Label = "Scope(zh)" });

        // Pbs Instruction family Dropdown
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "26f71a21-b062-4fc8-b47a-f50038e71fe1",
            Family = "Family 01",
            LocaleCode = "PbsInstructionFamilyTechnical instructions"
        });
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "fc925493-c443-437d-a367-b88e81941aaa",
            Family = "Family 02",
            LocaleCode = "PbsInstructionFamilySafety instructions"
        });
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "48ec5849-1daf-425c-8fcf-fb0dd9748b8c",
            Family = "Family 03",
            LocaleCode = "PbsInstructionFamilyEnvironmental instructions"
        });
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "e286e905-c157-4d19-ac7c-55550df0ee9a",
            Family = "Family 04",
            LocaleCode = "PbsInstructionFamilyHealth instructions"
        });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1686,
            LanguageCode = "nl",
            LocaleCode = "PbsInstructionFamilyTechnical instructions",
            Label = "Family 01(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1687,
            LanguageCode = "nl",
            LocaleCode = "PbsInstructionFamilySafety instructions",
            Label = "Family 02(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1688,
            LanguageCode = "nl",
            LocaleCode = "PbsInstructionFamilyEnvironmental instructions",
            Label = "Family 03(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1689,
            LanguageCode = "nl",
            LocaleCode = "PbsInstructionFamilyHealth instructions",
            Label = "Family 04(nl)"
        });

        // Pbs Risk dropdown data
        builder.Entity<RiskType>().HasData(new { Id = "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210", Type = "Threat" });
        builder.Entity<RiskType>().HasData(new { Id = "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0", Type = "Opportunity" });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1700,
            LanguageCode = "nl",
            LocaleCode = "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210",
            Label = "Threat(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1701,
            LanguageCode = "nl",
            LocaleCode = "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0",
            Label = "Opportunity(nl)"
        });

        builder.Entity<RiskStatus>().HasData(new { Id = "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae", Status = "Active" });
        builder.Entity<RiskStatus>().HasData(new { Id = "8b0d0513-6111-466f-86c8-b26278c3c4f7", Status = "Closed" });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1702,
            LanguageCode = "nl",
            LocaleCode = "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae",
            Label = "Active(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1703,
            LanguageCode = "nl",
            LocaleCode = "8b0d0513-6111-466f-86c8-b26278c3c4f7",
            Label = "Closed(nl)"
        });

        //Pbs Taxonomy Level data
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "f0d64941-145a-4a8a-8619-165c965a16eb",
            Type = "Location",
            Name = "Product",
            Order = 100,
            LocaleCode = "PbsTaxonomyLevel.Location.Product"
        });
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "077845b7-79a7-4883-a02d-6094fc6d6563",
            Type = "Location",
            Name = "Separation",
            Order = 200,
            LocaleCode = "PbsTaxonomyLevel.Location.Separation"
        });
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "8bb27024-ce91-4406-8e48-db08286f0b4b",
            Type = "Utility",
            Name = "Product",
            Order = 100,
            LocaleCode = "PbsTaxonomyLevel.Location.Product"
        });
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "cd8418c0-502e-4893-b387-1426a5edd3a4",
            Type = "Utility",
            Name = "Traject Part",
            Order = 200,
            LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart"
        });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1710,
            LanguageCode = "nl",
            LocaleCode = "PbsTaxonomyLevel.Location.Product",
            Label = "Product(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1711,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsTaxonomyLevel.Location.Product",
            Label = "Product(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1712,
            LanguageCode = "zh",
            LocaleCode = "PbsTaxonomyLevel.Location.Product",
            Label = "Product(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1713,
            LanguageCode = "nl",
            LocaleCode = "PbsTaxonomyLevel.Location.Separation",
            Label = "Separation(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1714,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsTaxonomyLevel.Location.Separation",
            Label = "Separation(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1715,
            LanguageCode = "zh",
            LocaleCode = "PbsTaxonomyLevel.Location.Separation",
            Label = "Separation(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1716,
            LanguageCode = "nl",
            LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart",
            Label = "Traject Part(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1717,
            LanguageCode = "nl-BE",
            LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart",
            Label = "Traject Part(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1718,
            LanguageCode = "zh",
            LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart",
            Label = "Traject Part()"
        });

        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "in voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "In Development",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Pending Development",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "in uitvoering",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<PmolType>().HasData(new
        {
            Id = "015bb656-f708-4a0d-9973-3d834ffe757d",
            Name = "Work",
            LanguageCode = "en",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01",
            DisplayOrder = 5,
            Type = 1
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "03f7c556-2d73-4283-8fc3-634233943bb9",
            Name = "Werk",
            LanguageCode = "nl",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01",
            DisplayOrder = 5,
            Type = 1
        });
        //builder.Entity<PmolType>().HasData(new { Id = "17e4fc8f-2531-4c24-a289-e3360d8e481f", Name = "Personal", LanguageCode = "en", TypeId = "e4fc8f-2531-4c24-a289-e3360d8e481f17", DisplayOrder = 5 });
        //builder.Entity<PmolType>().HasData(new { Id = "278a6814-2097-4f7b-9ebf-f17e5416911b", Name = "persoonlijk", LanguageCode = "nl", TypeId = "e4fc8f-2531-4c24-a289-e3360d8e481f17", DisplayOrder = 5 });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf",
            Name = "Travel",
            LanguageCode = "en",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1",
            DisplayOrder = 5,
            Type = 0
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "c80b2d63-f3d0-4cd4-8353-5d7a089dba98",
            Name = "Verplaatsen",
            LanguageCode = "nl",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1",
            DisplayOrder = 5,
            Type = 0
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "f3d04255-1cc1-4cdc-b8a7-5423972a3dda",
            Name = "(Un)load",
            LanguageCode = "en",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
            DisplayOrder = 5,
            Type = 2
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "ff848e5e-622d-4783-95e6-4092004eb5ea",
            Name = "Laden en lossen",
            LanguageCode = "nl",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
            DisplayOrder = 5,
            Type = 2
        });

        builder.Entity<Roles>().HasData(new
            { Id = "5e622d-4783-95e6-4092004eb5e-aff848e", TenantId = 1, RoleName = "Welder" });
        builder.Entity<Roles>().HasData(new
        {
            Id = "18d10b25-57ea-4f4e-9e76-56df09554a95",
            TenantId = 1,
            RoleName = "Lasser",
            LanguageCode = "nl",
            RoleId = "5e622d-4783-95e6-4092004eb5e-aff848e"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "be432c54-5860-4f56-832d-ceb3435d8b7e",
            TenantId = 1,
            RoleName = "Administrator",
            LanguageCode = "nl",
            RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "476127cb-62db-4af7-ac8e-d4a722f8e142",
            TenantId = 1,
            RoleName = "Project Manager",
            LanguageCode = "nl",
            RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "245d23fe-4864-4cc5-b53b-0a3b3843f0e1",
            TenantId = 1,
            RoleName = "Gebruiker",
            LanguageCode = "nl",
            RoleId = "4837043c-119c-47e1-bbf2-1f32557fdf30"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "1666e217-2b80-4acd-b48b-b041fe263fb9",
            TenantId = 1,
            RoleName = "Project Eigenaar",
            LanguageCode = "nl",
            RoleId = "6f56c794-7f88-48a7-9aba-a3f95f940be4"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "3178903c-bf36-40f7-b870-724e238684ff",
            TenantId = 1,
            RoleName = "Project Ingenieur",
            LanguageCode = "nl",
            RoleId = "f9a0cee5-f09a-44a5-93e8-d78f84bbcbf3"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "907b7af0-b132-4951-a2dc-6ab82d4cd40d",
            TenantId = 1,
            RoleName = "Customer Invoice Contact",
            LanguageCode = "en",
            RoleId = "907b7af0-b132-4951-a2dc-6ab82d4cd40d"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "78b84ad9-6757-405a-9729-5d2af8615e07",
            TenantId = 1,
            RoleName = "Customer Invoice Contact(nl)",
            LanguageCode = "nl",
            RoleId = "907b7af0-b132-4951-a2dc-6ab82d4cd40d"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "67897af0-b132-4951-a2dc-6ab82d4cd40d",
            TenantId = 1,
            RoleName = "Customer Project Contact",
            LanguageCode = "en",
            RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "56784ad9-6757-405a-9729-5d2af8615e07",
            TenantId = 1,
            RoleName = "Customer Project Contact(nl)",
            LanguageCode = "nl",
            RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "h5c51857-acb7-r4b4-cd0e-t62ba4c80t0c",
            TenantId = 1,
            RoleName = "Architect",
            LanguageCode = "en",
            RoleId = "tec51857-arch-44b4-8d0e-362ba468000c"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "g5c51857-gcb7-g4b4-cd0e-g62ba4c80t0c",
            TenantId = 1,
            RoleName = "Architect(nl)",
            LanguageCode = "nl",
            RoleId = "tec51857-arch-44b4-8d0e-362ba468000c"
        });

        builder.Entity<WorkflowState>().HasData(new
            { Id = 4, State = "Handed Over", LocaleCode = "WorkflowState.csvHandedOver", IsDeleted = false });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1895,
            LanguageCode = "nl",
            LocaleCode = "WorkflowState.csvHandedOver",
            Label = "Handed Over(nl)"
        });

        builder.Entity<PsStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "in voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "In Development",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Pending Development",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "in uitvoering",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<PsType>().HasData(new
        {
            Id = "015bb656-f708-4a0d-9973-3d834ffe757d",
            Name = "Quotation",
            LanguageCode = "en",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01",
            DisplayOrder = 5,
            Type = 1
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "03f7c556-2d73-4283-8fc3-634233943bb9",
            Name = "Offerte",
            LanguageCode = "nl",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01",
            DisplayOrder = 5,
            Type = 1
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf",
            Name = "Time and Material",
            LanguageCode = "en",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1",
            DisplayOrder = 5,
            Type = 0
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "c80b2d63-f3d0-4cd4-8353-5d7a089dba98",
            Name = "Regie",
            LanguageCode = "nl",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1",
            DisplayOrder = 5,
            Type = 0
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "f3d04255-1cc1-4cdc-b8a7-5423972a3dda",
            Name = "Extra Work",
            LanguageCode = "en",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
            DisplayOrder = 5,
            Type = 2
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "ff848e5e-622d-4783-95e6-4092004eb5ea",
            Name = "Meerwerk op offerte",
            LanguageCode = "nl",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
            DisplayOrder = 5,
            Type = 2
        });

        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4aby8714-4087-45d4-beff-2d63c756688f1", Name = "9.0" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-beff-2d63c756682f2", Name = "9.1" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-beff-2d63c756688f3", Name = "9.2" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-beff-2d63c7566w8f4", Name = "9.3" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4287-45d4-beff-2d63c7566d8f5", Name = "9.4" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4287-45d4-beff-2d63c7566f8f6", Name = "9.5" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4187-45d4-beff-2d63c7566g8f7", Name = "9.6" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-yu5d-beff-2d63c7566y8f8", Name = "9.7" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-y5d4-beff-2d63c7566u8f9", Name = "9.8" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-tyd4-beff-2d63c756gh8f10", Name = "9.9" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-4frd4-beff-2d63c756rd8f11", Name = "9.10" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-4zd4-beff-2d63c756nm8f12", Name = "9.11" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-4jd4-beff-2d63c756tt8f13", Name = "9.12" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-4md4-beff-2d63c756688f14", Name = "9.13" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-4ld4-beff-2d63c756688f15", Name = "9.14" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-49d4-beff-2d63c756688f16", Name = "9.15" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-48d4-beff-2d63c756688f17", Name = "9.16" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-47d4-beff-2d63c756688f18", Name = "9.17" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-46d4-beff-2d63c756688f19", Name = "9.18" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-44d4-beff-2d63c756688f20", Name = "9.19" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-beff-2d63c756688f21", Name = "9.20" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-boff-2d63c756688f22", Name = "9.21" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-boff-2d63c756688f23", Name = "9.22" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-bqff-2d63c756688f24", Name = "9.23" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-baff-2d63c756688f25", Name = "9.24" });
        builder.Entity<GeneralLedgerNumber>()
            .HasData(new { Id = "4ab98714-4087-45d4-bzff-2d63c756688f26", Name = "9.25" });

        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "in voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "In Development",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Pending Development",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "in uitvoering",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-bqff-2d63c756688f24", Name = "0%", Order = 0, IsDefault = false });
        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-baff-2d63c756688f25", Name = "6%", Order = 1, IsDefault = false });
        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-bzff-2d63c756688f26", Name = "12%", Order = 2, IsDefault = false });
        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-bzff-2d63c756688f27", Name = "21%", Order = 3, IsDefault = true });

        // builder.Entity<POStatus>().HasData(new { Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb", Name = "in voorbereiding", LanguageCode = "nl", StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1 });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "In Development",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        //   builder.Entity<POStatus>().HasData(new { Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276", Name = "Pending Development", LanguageCode = "en", StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1 });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "in uitvoering",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<POStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af34",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback",
            DisplayOrder = 6
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed35",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback",
            DisplayOrder = 6
        });

        builder.Entity<POStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af56",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-accept",
            DisplayOrder = 7
        });
        builder.Entity<POStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed78",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-accept",
            DisplayOrder = 7
        });

        builder.Entity<POType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "Resources",
            LanguageCode = "en",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<POType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
            Name = "Subcontractor Product",
            LanguageCode = "en",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<POType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae1",
            Name = "Resources",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<POType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
            Name = "Subcontractor Product",
            LanguageCode = "nl",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<POType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-prod-d27008688bae",
            Name = "Godderies Product",
            LanguageCode = "en",
            TypeId = "94282458-0b40-capa-b0f9-c2e40344c8f1",
            DisplayOrder = 3
        });
        builder.Entity<POType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-reso-e40dbe6a51ac3",
            Name = "Godderies Product",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-capa-b0f9-c2e40344c8f1",
            DisplayOrder = 3
        });

        builder.Entity<WFShortCutPane>().HasData(new
            { Id = "a35ab9fe-df57-4088-82a9-d27008688bae1", Name = "All", LanguageCode = "en", DisplayOrder = 1 });
        builder.Entity<WFShortCutPane>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2",
            Name = "Good Picking",
            LanguageCode = "en",
            DisplayOrder = 2,
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1"
        });
        builder.Entity<WFShortCutPane>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae3",
            Name = "Good Reception",
            LanguageCode = "en",
            DisplayOrder = 3,
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb"
        });
        builder.Entity<WFShortCutPane>().HasData(new
        {
            Id = "a35ab9fe-df57-4nwre-82a9-d27008688bae3",
            Name = "Stock Counting",
            LanguageCode = "en",
            DisplayOrder = 4,
            TypeId = "4010e768-fety-4702-bnew-ee367a82addb"
        });
        builder.Entity<WFShortCutPane>().HasData(new
            { Id = "a35ab9fe-df57-4088-82a9-d27008688bae4", Name = "All", LanguageCode = "nl", DisplayOrder = 1 });
        builder.Entity<WFShortCutPane>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
            Name = "Versturen Goederen",
            LanguageCode = "nl",
            DisplayOrder = 2,
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1"
        });
        builder.Entity<WFShortCutPane>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae6",
            Name = "Ontvangst goederen",
            LanguageCode = "nl",
            DisplayOrder = 3,
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb"
        });
        builder.Entity<WFShortCutPane>().HasData(new
        {
            Id = "a35ab9fe-df57-4nwre-0nl8-d27008688bae3",
            Name = "Stock Counting(nl)",
            LanguageCode = "nl",
            DisplayOrder = 4,
            TypeId = "4010e768-fety-4702-bnew-ee367a82addb"
        });

        builder.Entity<StockShortCutPane>().HasData(new
            { Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac1", Name = "All", LanguageCode = "en", DisplayOrder = 1 });
        builder.Entity<StockShortCutPane>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af34",
            Name = "Material",
            LanguageCode = "en",
            DisplayOrder = 2,
            TypeId = "c46c3a26-39a5-42cc-n7k1-89655304eh6"
        });
        builder.Entity<StockShortCutPane>().HasData(new
        {
            Id = "bdd9e477-75b3-40c6-ad61-e40dbe6a51ac3",
            Name = "Tool",
            LanguageCode = "en",
            DisplayOrder = 3,
            TypeId = "c46c3a26-39a5-42cc-n9wn-89655304eh6"
        });
        builder.Entity<StockShortCutPane>().HasData(new
        {
            Id = "bdd9e476-75b3-40c6-ad61-e40dbe6a51ac4",
            Name = "Consumables",
            LanguageCode = "en",
            DisplayOrder = 4,
            TypeId = "c46c3a26-39a5-42cc-m06g-89655304eh6"
        });
        builder.Entity<StockShortCutPane>().HasData(new
            { Id = "bdd9e475-75b3-40c6-ad61-e40dbe6a51ac5", Name = "All", LanguageCode = "nl", DisplayOrder = 1 });
        builder.Entity<StockShortCutPane>().HasData(new
        {
            Id = "bdd9e474-75b3-40c6-ad61-e40dbe6a51ac6",
            Name = "Materialen",
            LanguageCode = "nl",
            DisplayOrder = 2,
            TypeId = "c46c3a26-39a5-42cc-n7k1-89655304eh6"
        });
        builder.Entity<StockShortCutPane>().HasData(new
        {
            Id = "bdd9e473-75b3-40c6-ad61-e40dbe6a51ac7",
            Name = "Gereedschap",
            LanguageCode = "nl",
            DisplayOrder = 3,
            TypeId = "c46c3a26-39a5-42cc-n9wn-89655304eh6"
        });
        builder.Entity<StockShortCutPane>().HasData(new
        {
            Id = "bdd9e472-75b3-40c6-ad61-e40dbe6a51ac8",
            Name = "Verbruiksgoederen",
            LanguageCode = "nl",
            DisplayOrder = 4,
            TypeId = "c46c3a26-39a5-42cc-m06g-89655304eh6"
        });


        builder.Entity<WHShortCutPane>().HasData(new
            { Id = "a35ab9fe-df57-4088-82a9-d27008688bae11", Name = "All", LanguageCode = "en", DisplayOrder = 1 });
        builder.Entity<WHShortCutPane>().HasData(new
            { Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac12", Name = "Fixed", LanguageCode = "en", DisplayOrder = 2 });
        builder.Entity<WHShortCutPane>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae13",
            Name = "Mobile",
            LanguageCode = "en",
            DisplayOrder = 3
        });
        builder.Entity<WHShortCutPane>().HasData(new
            { Id = "a35ab9fe-df57-4088-82a9-d27008688bae14", Name = "All", LanguageCode = "nl", DisplayOrder = 1 });
        builder.Entity<WHShortCutPane>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac15",
            Name = "Gebouw",
            LanguageCode = "nl",
            DisplayOrder = 2
        });
        builder.Entity<WHShortCutPane>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae16",
            Name = "Bestelwagen",
            LanguageCode = "nl",
            DisplayOrder = 3
        });

        builder.Entity<WHStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "Open",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<WHStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "Closed",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<WHStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Open",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<WHStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "Gesloten",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });


        builder.Entity<StockStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "Available",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<StockStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Out of Stock",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<StockStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-uuhh-ce7fbf10e27c",
            Name = "beschikbaar",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<StockStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-ssdd-f5fdca2ab276",
            Name = "Niet meer voorradig",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });

        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "in voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "In Development",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Pending Development",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "in uitvoering",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<WFActivityStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<WHType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "Fixed",
            LanguageCode = "en",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<WHType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
            Name = "Mobile",
            LanguageCode = "en",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<WHType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-hgkf-d27008688bae",
            Name = "Gebouw",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<WHType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ldmc-e40dbe6a51ac3",
            Name = "Bestelwagen",
            LanguageCode = "nl",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });

        builder.Entity<StockType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "Materials",
            LanguageCode = "en",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<StockType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
            Name = "Tools",
            LanguageCode = "en",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<StockType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae1",
            Name = "Consumables",
            LanguageCode = "en",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 3
        });
        builder.Entity<StockType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688b44",
            Name = "Materialen",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<StockType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51a44",
            Name = "Gereedschap",
            LanguageCode = "nl",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<StockType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688ba44",
            Name = "Verbruiksgoederen",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 3
        });

        builder.Entity<WFType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "Good Picking",
            LanguageCode = "en",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<WFType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
            Name = "Good Reception",
            LanguageCode = "en",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<WFType>().HasData(new
        {
            Id = "bdd9e479-fety-4702-bnew-e40dbe6a51ac3",
            Name = "Stock Counting",
            LanguageCode = "en",
            TypeId = "4010e768-fety-4702-bnew-ee367a82addb",
            DisplayOrder = 3
        });
        builder.Entity<WFType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-tyur-d27008688bae",
            Name = "Versturen Goederen ",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<WFType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3",
            Name = "Ontvangst goederen",
            LanguageCode = "nl",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<WFType>().HasData(new
        {
            Id = "bdd9e479-fety-L70N-bnew-e40dbe6a51anl",
            Name = "Stock Counting(nl)",
            LanguageCode = "nl",
            TypeId = "4010e768-fety-4702-bnew-ee367a82addb",
            DisplayOrder = 3
        });

        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "Warehouse",
            LanguageCode = "en",
            LevelId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "Zone",
            LanguageCode = "en",
            LevelId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab2en",
            Name = "Block",
            LanguageCode = "en",
            LevelId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 3,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1cen",
            Name = "Corridor",
            LanguageCode = "en",
            LevelId = "a35ab9fe-df57-4088-82a9-d27008688bae11",
            DisplayOrder = 4,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl",
            Name = "Blok",
            LanguageCode = "nl",
            LevelId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 3,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1cnl",
            Name = "Gang",
            LanguageCode = "nl",
            LevelId = "a35ab9fe-df57-4088-82a9-d27008688bae11",
            DisplayOrder = 4,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Rack",
            LanguageCode = "en",
            LevelId = "60aad0b-2e84-482b-ad25-618d80d49488",
            DisplayOrder = 5,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "Shelf",
            LanguageCode = "en",
            LevelId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 6,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Name = "Box",
            LanguageCode = "en",
            LevelId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 7,
            IsChildren = false
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "098cf409-7cb8-4076-00nl-657dd897f5bb",
            Name = "Magazijn",
            LanguageCode = "nl",
            LevelId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
            Name = "Zone",
            LanguageCode = "nl",
            LevelId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
            Name = "Rek",
            LanguageCode = "nl",
            LevelId = "60aad0b-2e84-482b-ad25-618d80d49488",
            DisplayOrder = 5,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
            Name = "Legplank",
            LanguageCode = "nl",
            LevelId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 6,
            IsChildren = true
        });
        builder.Entity<WHTaxonomyLevel>().HasData(new
        {
            Id = "5015743d-a2e6-4531-00nl-d4e1400e1eed",
            Name = "Doos",
            LanguageCode = "nl",
            LevelId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 7,
            IsChildren = false
        });

        builder.Entity<StockActiveType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "Active",
            LanguageCode = "en",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<StockActiveType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
            Name = "Spare",
            LanguageCode = "en",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<StockActiveType>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-hmnb-d27008688bae",
            Name = "Picking ",
            LanguageCode = "nl",
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<StockActiveType>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-fgdd-e40dbe6a51ac3",
            Name = "Reserve",
            LanguageCode = "nl",
            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 2
        });

        builder.Entity<StockActivityType>().HasData(new
        {
            Id = "4e01a893-0267-48af-inst-b7a93ebd1ccf",
            ActivityName = "In To The Stock",
            LanguageCode = "en",
            ActivityTypeId = "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da",
            DisplayOrder = 1
        });
        builder.Entity<StockActivityType>().HasData(new
        {
            Id = "4e01a893-0267-48af-outs-b7a93ebd1ccf",
            ActivityName = "Out To The Stock",
            LanguageCode = "en",
            ActivityTypeId = "7bcb4e8d-8e8c-outs-8170-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<StockActivityType>().HasData(new
        {
            Id = "4e01a893-0267-48af-inst-b7a93ebd1cnl",
            ActivityName = "In Stock",
            LanguageCode = "nl",
            ActivityTypeId = "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da",
            DisplayOrder = 1
        });
        builder.Entity<StockActivityType>().HasData(new
        {
            Id = "4e01a893-0267-48af-outs-b7a93ebd1cnl",
            ActivityName = "Uit Stock",
            LanguageCode = "nl",
            ActivityTypeId = "7bcb4e8d-8e8c-outs-8170-6b91c89fc3da",
            DisplayOrder = 2
        });

        builder.Entity<StockActivityType>().HasData(new
        {
            Id = "4e01a893-0001-48af-inst-b7a93ebd1cnl",
            ActivityName = "Stock Counting",
            LanguageCode = "en",
            ActivityTypeId = "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da",
            DisplayOrder = 3
        });
        builder.Entity<StockActivityType>().HasData(new
        {
            Id = "4e01a893-0002-48af-outs-b7a93ebd1cnl",
            ActivityName = "Stock Counting(nl)",
            LanguageCode = "nl",
            ActivityTypeId = "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da",
            DisplayOrder = 3
        });


        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Name = "New",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae1",
            Name = "Nieuw",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Name = "In Preparation",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 2
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-00nl-657dd897f5bb",
            Name = "In Voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            DisplayOrder = 2
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Name = "In Execution",
            LanguageCode = "en",
            StatusId = "jj282458-0b40-jja3-b0f9-c2e40344c8jj",
            DisplayOrder = 3
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c",
            Name = "In Uitvoering",
            LanguageCode = "nl",
            StatusId = "jj282458-0b40-jja3-b0f9-c2e40344c8jj",
            DisplayOrder = 3
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab2en",
            Name = "In Pre Commisioning",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 4
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl",
            Name = "In voorlopige bedrijfstelling",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            DisplayOrder = 4
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1cen",
            Name = "In Commisioning",
            LanguageCode = "en",
            StatusId = "a35ab9fe-df57-4088-82a9-d27008688bae11",
            DisplayOrder = 5
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1cnl",
            Name = "In bedrijfstelling",
            LanguageCode = "nl",
            StatusId = "a35ab9fe-df57-4088-82a9-d27008688bae11",
            DisplayOrder = 5
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Name = "Provisional Accepted",
            LanguageCode = "en",
            StatusId = "60aad0b-2e84-482b-ad25-618d80d49488",
            DisplayOrder = 6
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-00nl-f5fdca2ab276",
            Name = "Voorlopige opgeleverd",
            LanguageCode = "nl",
            StatusId = "60aad0b-2e84-482b-ad25-618d80d49488",
            DisplayOrder = 6
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "Accepted",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 7
        });
        builder.Entity<ProjectScopeStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-00nl-b7a93ebd1ccf",
            Name = "Opgeleverd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 7
        });


        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb",
            Label = "Sanitary",
            LanguageCode = "en",
            PbsSkillId = "d60aad0b-2e84-482b-ad25-618d80d49477"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
            Label = "Sanitair",
            LanguageCode = "nl",
            PbsSkillId = "d60aad0b-2e84-482b-ad25-618d80d49477"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
            Label = "Cooling Fitter",
            LanguageCode = "en",
            PbsSkillId = "94282458-0b40-40a3-b0f9-c2e40344c8f1"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Label = "Koelmonteur",
            LanguageCode = "nl",
            PbsSkillId = "94282458-0b40-40a3-b0f9-c2e40344c8f1"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed",
            Label = "Cooling Technician",
            LanguageCode = "en",
            PbsSkillId = "7143ff01-d173-4a20-8c17-cacdfecdb84c"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a",
            Label = "Koeltechnieker",
            LanguageCode = "nl",
            PbsSkillId = "7143ff01-d173-4a20-8c17-cacdfecdb84c"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af",
            Label = "Gas Fitter",
            LanguageCode = "en",
            PbsSkillId = "4010e768-3e06-4702-b337-ee367a82addb"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a",
            Label = "Gasmonteur",
            LanguageCode = "nl",
            PbsSkillId = "4010e768-3e06-4702-b337-ee367a82addb"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae",
            Label = "Gas Technician",
            LanguageCode = "en",
            PbsSkillId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
            Label = "Gastechnieker",
            LanguageCode = "nl",
            PbsSkillId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "8882cd5a-0941-4c56-9c13-f5fdca2ab888",
            Label = "Ventilation",
            LanguageCode = "en",
            PbsSkillId = "85cb4e8d-8e8c-487d-8170-6b91c89fc123"
        });
        builder.Entity<PbsSkillLocalizedData>().HasData(new
        {
            Id = "4dd9e479-75b3-40c6-ad61-e40dbe6a5111",
            Label = "Ventilatie",
            LanguageCode = "nl",
            PbsSkillId = "85cb4e8d-8e8c-487d-8170-6b91c89fc123"
        });

        //Competencies Skill Dropdown
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "d60aad0b-2e84-482b-ad25-618d80d49477",
            Title = "Sanitary",
            LocaleCode = "PbsSkillSanitary",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Title = "Cooling Fitter",
            LocaleCode = "PbsSkillCooling Fitter",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Title = "Cooling Technician",
            LocaleCode = "PbsSkillCooling Technician",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "4010e768-3e06-4702-b337-ee367a82addb",
            Title = "Gas Fitter",
            LocaleCode = "PbsSkillGas Fitter",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Title = "Gas Technician",
            LocaleCode = "PbsSkillGas Technician",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "85cb4e8d-8e8c-487d-8170-6b91c89fc123",
            Title = "Ventilation",
            LocaleCode = "PbsSkillVentilation",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });

        builder.Entity<PORequestType>().HasData(new
        {
            Id = "a35ab9fe-po57-4088-82a9-d27008688bae",
            Name = "Purchase Request",
            LanguageCode = "en",
            RequestTypeId = "94282458-0b40-poa3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "bdd9e479-pob3-40c6-ad61-e40dbe6a51ac3",
            Name = "Return Request",
            LanguageCode = "en",
            RequestTypeId = "4010e768-3e06-po02-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "a35ab9fe-ponl-0000-82a9-d27008688bae",
            Name = "Bestelaanvraag",
            LanguageCode = "nl",
            RequestTypeId = "94282458-0b40-poa3-b0f9-c2e40344c8f1",
            DisplayOrder = 1
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "bdd9e479-ponl-0000-ad61-e40dbe6a51ac3",
            Name = "Terug name",
            LanguageCode = "nl",
            RequestTypeId = "4010e768-3e06-po02-b337-ee367a82addb",
            DisplayOrder = 2
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "jhkfab9fe-po57-jhoe-82a9-d27008688nfr",
            Name = "Capacity Request",
            LanguageCode = "en",
            RequestTypeId = "lll82458-0b40-poa3-b0f9-c2e40344clll",
            DisplayOrder = 3
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "lkj9e479-pob3-lhds-ad61-e40dbe6a51t67",
            Name = "Reservatie",
            LanguageCode = "nl",
            RequestTypeId = "lll82458-0b40-poa3-b0f9-c2e40344clll",
            DisplayOrder = 3
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "ehwbjdjd-pob3-lhds-ad61-e40dbe6a51www",
            Name = "Purchase Order",
            LanguageCode = "en",
            RequestTypeId = "343482458-0spr-poa3-b0f9-c2e40344clll",
            DisplayOrder = 4
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "hjnnjdjd-pob3-lhds-ad61-e40dbe6a51tfg",
            Name = "Bestelling",
            LanguageCode = "nl",
            RequestTypeId = "343482458-0spr-poa3-b0f9-c2e40344clll",
            DisplayOrder = 4
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "fbbdc374-aa0c-4ee7-818e-da26f7352e23",
            Name = "RFQ",
            LanguageCode = "en",
            RequestTypeId = "f4d6ba08-3937-44ca-a0a1-7cf33c03e290",
            DisplayOrder = 5
        });
        builder.Entity<PORequestType>().HasData(new
        {
            Id = "5385cab3-755b-4c6c-9422-7e99464294ce",
            Name = "RFQ",
            LanguageCode = "nl",
            RequestTypeId = "f4d6ba08-3937-44ca-a0a1-7cf33c03e290",
            DisplayOrder = 5
        });


        builder.Entity<BorType>().HasData(new
        {
            Id = "335ab9fe-po57-4088-82a9-d27008688b55",
            Name = "Regular",
            LanguageCode = "en",
            BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
            DisplayOrder = 1
        });
        builder.Entity<BorType>().HasData(new
        {
            Id = "77d9e479-pob3-40c6-ad61-e40dbe6a5177",
            Name = "Return",
            LanguageCode = "en",
            BorTypeId = "6610e768-3e06-po02-b337-ee367a82ad66",
            DisplayOrder = 2
        });

        builder.Entity<VPOrganisationShortcutPane>().HasData(new
        {
            Id = "098cf409-7cb8-40vp-vpdf-657dd897f5bb",
            Name = "Organisation",
            LanguageCode = "en",
            OrganisationId = "d60aad0b-vp84-482b-ad25-vp8d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<VPOrganisationShortcutPane>().HasData(new
        {
            Id = "12e2d6c5-1ada-4evp-vpba-ce7fbf10e27c",
            Name = "CU",
            LanguageCode = "en",
            OrganisationId = "94282458-vp40-40a3-b0f9-vpe40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<VPOrganisationShortcutPane>().HasData(new
        {
            Id = "2732cd5a-0941-4cvp-vp13-f5fdca2ab2en",
            Name = "BU",
            LanguageCode = "en",
            OrganisationId = "4010e768-vp06-4702-b337-vp367a82addb",
            DisplayOrder = 3
        });
        builder.Entity<VPOrganisationShortcutPane>().HasData(new
        {
            Id = "4e01a893-0267-48vp-vp5a-b7a93ebd1cen",
            Name = "Department",
            LanguageCode = "en",
            OrganisationId = "a35ab9fe-vp57-4088-82a9-vp7008688bae11",
            DisplayOrder = 4
        });
        builder.Entity<VPOrganisationShortcutPane>().HasData(new
        {
            Id = "2732cd5a-0941-4cvp-vp13-f5fdca2ab276",
            Name = "Team",
            LanguageCode = "en",
            OrganisationId = "60aad0b-vp84-482b-ad25-vp8d80d49488",
            DisplayOrder = 5
        });
        builder.Entity<VPOrganisationShortcutPane>().HasData(new
        {
            Id = "4e01a893-0267-48vp-vp5a-b7a93ebd1ccf",
            Name = "Person",
            LanguageCode = "en",
            OrganisationId = "7bcb4e8d-8c-487d-8170-vp91c89fc3da",
            DisplayOrder = 6
        });


        builder.Entity<CompetenciesTaxonomyLevel>().HasData(new
        {
            Id = "335ab9fe-po57-4088-82a9-d27008688b55",
            Name = "Type",
            LanguageCode = "en",
            LevelId = "88282458-0b40-poa3-b0f9-c2e40344c888",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<CompetenciesTaxonomyLevel>().HasData(new
        {
            Id = "77d9e479-pob3-40c6-ad61-e40dbe6a5177",
            Name = "Level",
            LanguageCode = "en",
            LevelId = "6610e768-3e06-po02-b337-ee367a82ad66",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<CompetenciesTaxonomyLevel>().HasData(new
        {
            Id = "335ab9fe-po57-4088-82a9-d27008688ttt",
            Name = "Type nl",
            LanguageCode = "nl",
            LevelId = "88282458-0b40-poa3-b0f9-c2e40344c888",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<CompetenciesTaxonomyLevel>().HasData(new
        {
            Id = "77d9e479-pob3-40c6-ad61-e40dbe6a5432",
            Name = "Level nl",
            LanguageCode = "nl",
            LevelId = "6610e768-3e06-po02-b337-ee367a82ad66",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<CompetenciesTaxonomyLevel>().HasData(new
        {
            Id = "kkkab9fe-po57-4088-82a9-d27008688b55",
            Name = "Competencies",
            LanguageCode = "en",
            LevelId = "4010e768-3e06-po02-b337-ee367a82addb",
            DisplayOrder = 3,
            IsChildren = true
        });
        builder.Entity<CompetenciesTaxonomyLevel>().HasData(new
        {
            Id = "www9e479-pob3-40c6-ad61-e40dbe6a5177",
            Name = "Competencies nl",
            LanguageCode = "nl",
            LevelId = "4010e768-3e06-po02-b337-ee367a82addb",
            DisplayOrder = 3,
            IsChildren = true
        });

        builder.Entity<CertificationTaxonomyLevel>().HasData(new
        {
            Id = "vv5ab9fe-po57-4088-82a9-d27008688bvv",
            Name = "Type",
            LanguageCode = "en",
            LevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<CertificationTaxonomyLevel>().HasData(new
        {
            Id = "uud9e479-pob3-40c6-ad61-e40dbe6a51uu",
            Name = "Level",
            LanguageCode = "en",
            LevelId = "2210e768-3e06-po02-b337-ee367a82ad22",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<CertificationTaxonomyLevel>().HasData(new
        {
            Id = "gg5ab9fe-po57-4088-82a9-d27008688tgg",
            Name = "Type nl",
            LanguageCode = "nl",
            LevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<CertificationTaxonomyLevel>().HasData(new
        {
            Id = "kkd9e479-pob3-40c6-ad61-e40dbe6a54kk",
            Name = "Level nl",
            LanguageCode = "nl",
            LevelId = "2210e768-3e06-po02-b337-ee367a82ad22",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<CertificationTaxonomyLevel>().HasData(new
        {
            Id = "ttkab9fe-po57-4088-82a9-d27008688btt",
            Name = "Certification",
            LanguageCode = "en",
            LevelId = "oo10e768-3e06-po02-b337-ee367a82adoo",
            DisplayOrder = 3,
            IsChildren = true
        });
        builder.Entity<CertificationTaxonomyLevel>().HasData(new
        {
            Id = "eew9e479-pob3-40c6-ad61-e40dbe6a51ee",
            Name = "Certification nl",
            LanguageCode = "nl",
            LevelId = "oo10e768-3e06-po02-b337-ee367a82adoo",
            DisplayOrder = 3,
            IsChildren = true
        });

        builder.Entity<CertificationTaxonomy>().HasData(new
        {
            Id = "115ab9fe-po57-4088-82a9-d27008688bvv",
            Title = "Qualification",
            CetificationId = "115ab9fe-po57-4088-82a9-d27008688bvv",
            CertificationTaxonomyLevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq",
            ParentCertificationId = "115ab9fe-po57-4088-82a9-d27008688bvv"
        });
        builder.Entity<CertificationTaxonomy>().HasData(new
        {
            Id = "22d9e479-pob3-40c6-ad61-e40dbe6a51uu",
            Title = "Qualification Type 1",
            CetificationId = "22d9e479-pob3-40c6-ad61-e40dbe6a51uu",
            ParentId = "115ab9fe-po57-4088-82a9-d27008688bvv",
            CertificationTaxonomyLevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq",
            ParentCertificationId = "22d9e479-pob3-40c6-ad61-e40dbe6a51uu"
        });
        builder.Entity<CertificationTaxonomy>().HasData(new
        {
            Id = "335ab9fe-po57-4088-82a9-d27008688tgg",
            Title = "Qualification X",
            CetificationId = "335ab9fe-po57-4088-82a9-d27008688tgg",
            CertificationTaxonomyLevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq",
            ParentCertificationId = "335ab9fe-po57-4088-82a9-d27008688tgg"
        });
        builder.Entity<CertificationTaxonomy>().HasData(new
        {
            Id = "44d9e479-pob3-40c6-ad61-e40dbe6a54kk",
            Title = "Qualifiaction Type 2",
            CetificationId = "44d9e479-pob3-40c6-ad61-e40dbe6a54kk",
            ParentId = "335ab9fe-po57-4088-82a9-d27008688tgg",
            CertificationTaxonomyLevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq",
            ParentCertificationId = "44d9e479-pob3-40c6-ad61-e40dbe6a54kk"
        });

        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "vv5ab9fe-po57-4088-82a9-d27008688bbb",
            Name = "Organization",
            LanguageCode = "en",
            LevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8kk",
            DisplayOrder = 1,
            IsChildren = false
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "uud9e479-pob3-40c6-ad61-e40dbe6a5111",
            Name = "CU",
            LanguageCode = "en",
            LevelId = "2210e768-3e06-po02-b337-ee367a82adjj",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "gg5ab9fe-po57-4088-82a9-d27008688ttt",
            Name = "Organization nl",
            LanguageCode = "nl",
            LevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8kk",
            DisplayOrder = 1,
            IsChildren = false
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "kkd9e479-pob3-40c6-ad61-e40dbe6a5444",
            Name = "CU nl",
            LanguageCode = "nl",
            LevelId = "2210e768-3e06-po02-b337-ee367a82adjj",
            DisplayOrder = 2,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "ttkab9fe-po57-4088-82a9-d27008688bbb",
            Name = "BU",
            LanguageCode = "en",
            LevelId = "oo10e768-3e06-po02-b337-ee367a82admn",
            DisplayOrder = 3,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "eew9e479-pob3-40c6-ad61-e40dbe6a5111",
            Name = "BU nl",
            LanguageCode = "nl",
            LevelId = "oo10e768-3e06-po02-b337-ee367a82admn",
            DisplayOrder = 3,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "jvfkab9fe-po57-4088-82a9-d27008688jvf",
            Name = "Department",
            LanguageCode = "en",
            LevelId = "1210e768-3e06-po02-b337-ee367a82ad12",
            DisplayOrder = 4,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "nbv9e479-pob3-40c6-ad61-e40dbe6a5nbv",
            Name = "Department nl",
            LanguageCode = "nl",
            LevelId = "1210e768-3e06-po02-b337-ee367a82ad12",
            DisplayOrder = 4,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "slaab9fe-po57-4088-82a9-d27008688kgd",
            Name = "Team",
            LanguageCode = "en",
            LevelId = "fg10e768-3e06-po02-b337-ee367a82adfg",
            DisplayOrder = 5,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks",
            Name = "Team nl",
            LanguageCode = "nl",
            LevelId = "fg10e768-3e06-po02-b337-ee367a82adfg",
            DisplayOrder = 5,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "aqwab9fe-po57-4088-82a9-d27008688mvk",
            Name = "Person",
            LanguageCode = "en",
            LevelId = "we10e768-3e06-po02-b337-ee367a82adwe",
            DisplayOrder = 6,
            IsChildren = true
        });
        builder.Entity<OrganizationTaxonomyLevel>().HasData(new
        {
            Id = "bds9e479-pob3-40c6-ad61-e40dbe6a5gtu",
            Name = "Person nl",
            LanguageCode = "nl",
            LevelId = "we10e768-3e06-po02-b337-ee367a82adwe",
            DisplayOrder = 6,
            IsChildren = true
        });
        //builder.Entity<OrganizationTaxonomyLevel>().HasData(new { Id = "uryab9fe-po57-4088-82a9-d27008688kde", Name = "Org settings", LanguageCode = "en", LevelId = "yr10e768-3e06-po02-b337-ee367a82adjh", DisplayOrder = 7, IsChildren = true });
        //builder.Entity<OrganizationTaxonomyLevel>().HasData(new { Id = "ywd9e479-pob3-40c6-ad61-e40dbe6a5lad", Name = "Org settings nl", LanguageCode = "nl", LevelId = "yr10e768-3e06-po02-b337-ee367a82adjh", DisplayOrder = 7, IsChildren = true });

        builder.Entity<HRRoles>().HasData(new
        {
            Id = "uu5ab9fe-po57-4088-82a9-d27008688bvv",
            Name = "Manager",
            RoleId = "115ab9fe-po57-4088-82a9-d27008688bvv",
            LanguageCode = "en",
            Label = "Manager"
        });
        builder.Entity<HRRoles>().HasData(new
        {
            Id = "lld9e479-pob3-40c6-ad61-e40dbe6a51uu",
            Name = "Manager(nl)",
            RoleId = "115ab9fe-po57-4088-82a9-d27008688bvv",
            LanguageCode = "nl",
            Label = "Manager"
        });
        builder.Entity<HRRoles>().HasData(new
        {
            Id = "jj5ab9fe-po57-4088-82a9-d27008688tgg",
            Name = "Worker",
            RoleId = "335ab9fe-po57-4088-82a9-d27008688tgg",
            LanguageCode = "en",
            Label = "Worker"
        });
        builder.Entity<HRRoles>().HasData(new
        {
            Id = "ffd9e479-pob3-40c6-ad61-e40dbe6a54kk",
            Name = "Worker(nl)",
            RoleId = "335ab9fe-po57-4088-82a9-d27008688tgg",
            LanguageCode = "nl",
            Label = "Worker"
        });


        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "aqwab9fe-po57-4088-82a9-d27008688mvk",
            Name = "Casual",
            TypeId = "lkkab9fe-po57-4088-82a9-d27008688lkk",
            LanguageCode = "en",
            Label = "Casual",
            DisplayOrder = 1
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "slaab9fe-po57-4088-82a9-d27008688kgd",
            Name = "Casual(nl)",
            TypeId = "lkkab9fe-po57-4088-82a9-d27008688lkk",
            LanguageCode = "nl",
            Label = "Casual",
            DisplayOrder = 1
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks",
            Name = "Annual",
            TypeId = "fg10e768-3e06-po02-b337-ee367a82afff",
            LanguageCode = "en",
            Label = "Annual",
            DisplayOrder = 2
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "eew9e479-pob3-40c6-ad61-e40dbe6a5111",
            Name = "Annual(nl)",
            TypeId = "fg10e768-3e06-po02-b337-ee367a82afff",
            LanguageCode = "nl",
            Label = "Annual",
            DisplayOrder = 2
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "qqqab9fe-qq57-4088-82a9-d27008688qqq",
            Name = "Medical",
            TypeId = "oo10e768-3e06-po02-b337-ee367a82adooo",
            LanguageCode = "en",
            Label = "Medical",
            DisplayOrder = 3
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "zzzab9fe-po57-4088-82a9-d27008688zzz",
            Name = "Medical(nl)",
            TypeId = "oo10e768-3e06-po02-b337-ee367a82adooo",
            LanguageCode = "nl",
            Label = "Medical",
            DisplayOrder = 3
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "bnb9e479-pob3-40c6-ad61-e40dbe6a5bnb",
            Name = "Maternity",
            TypeId = "2210e768-3e06-po02-b337-ee367a82ad22",
            LanguageCode = "en",
            Label = "Maternity",
            DisplayOrder = 4
        });
        builder.Entity<AbsenceLeaveType>().HasData(new
        {
            Id = "wer9e479-pob3-40c6-ad61-e40dbe6a5wer",
            Name = "Maternity(nl)",
            TypeId = "2210e768-3e06-po02-b337-ee367a82ad22",
            LanguageCode = "nl",
            Label = "Maternity",
            DisplayOrder = 4
        });

        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "aqwab9fe-msms-4088-82a9-d27008688mvk",
            Name = "Scope : Product States",
            TypeId = "lkkab9fe-msms-4088-82a9-d27008688lkk",
            LanguageCode = "en",
            Label = "Scope : Product States",
            DisplayOrder = 1
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "slaab9fe-msms-4088-82a9-d27008688kgd",
            Name = "Scope : Product States(nl)",
            TypeId = "lkkab9fe-msms-4088-82a9-d27008688lkk",
            LanguageCode = "nl",
            Label = "Scope : Product States",
            DisplayOrder = 1
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "qwe9e479-msms-40c6-ad61-e40dbe6a5lks",
            Name = "Finance : invoices and payments",
            TypeId = "fg10e768-msms-po02-b337-ee367a82afff",
            LanguageCode = "en",
            Label = "Finance : invoices and payments",
            DisplayOrder = 2
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "eew9e479-msms-40c6-ad61-e40dbe6a5111",
            Name = "Finance : invoices and payments(nl)",
            TypeId = "fg10e768-msms-po02-b337-ee367a82afff",
            LanguageCode = "nl",
            Label = "Finance : invoices and payments",
            DisplayOrder = 2
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "qqqab9fe-msms-4088-82a9-d27008688qqq",
            Name = "Quality Activities",
            TypeId = "oo10e768-msms-po02-b337-ee367a82adooo",
            LanguageCode = "en",
            Label = "Quality Activities",
            DisplayOrder = 3
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "zzzab9fe-msms-4088-82a9-d27008688zzz",
            Name = "Quality Activities(nl)",
            TypeId = "oo10e768-msms-po02-b337-ee367a82adooo",
            LanguageCode = "nl",
            Label = "Quality Activities",
            DisplayOrder = 3
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "bnb9e479-msms-40c6-ad61-e40dbe6a5bnb",
            Name = "Risk Assessments",
            TypeId = "2210e768-msms-po02-b337-ee367a82ad22",
            LanguageCode = "en",
            Label = "Risk Assessments",
            DisplayOrder = 4
        });
        builder.Entity<MilestoneType>().HasData(new
        {
            Id = "wer9e479-msms-40c6-ad61-e40dbe6a5wer",
            Name = "Risk Assessments(nl)",
            TypeId = "2210e768-msms-po02-b337-ee367a82ad22",
            LanguageCode = "nl",
            Label = "Risk Assessments",
            DisplayOrder = 4
        });

        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "098cf409-msms-4076-8ddf-657dd897f5bb",
            Name = "in voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-msms-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "12e2d6c5-msms-4e74-88ba-ce7fbf10e27c",
            Name = "In Development",
            LanguageCode = "en",
            StatusId = "94282458-msms-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "2732cd5a-msms-4c56-9c13-f5fdca2ab276",
            Name = "Pending Development",
            LanguageCode = "en",
            StatusId = "d60aad0b-msms-482b-ad25-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "4e01a893-msms-48af-b65a-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-msms-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "5015743d-msms-4531-808d-d4e1400e1eed",
            Name = "ter goedkeuring",
            LanguageCode = "nl",
            StatusId = "7143ff01-msms-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "77143c60-msms-4ca2-8723-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-msms-487d-8170-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "813a0c70-msms-433d-8945-9cb166ae42af",
            Name = "In Review",
            LanguageCode = "en",
            StatusId = "7143ff01-msms-4a20-8c17-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "8d109134-msms-4c7b-84c5-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-msms-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "a35ab9fe-msms-4088-82a9-d27008688bae",
            Name = "in uitvoering",
            LanguageCode = "nl",
            StatusId = "94282458-msms-40a3-b0f9-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<MilestoneStatus>().HasData(new
        {
            Id = "bdd9e479-msms-40c6-ad61-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-msms-4702-b337-ee367a82addb",
            DisplayOrder = 5
        });

        builder.Entity<MachineTaxonomyLevel>().HasData(new
        {
            Id = "335ab9fe-mc57-4088-82a9-d27008688b55",
            Name = "Machine",
            LanguageCode = "en",
            LevelId = "88282458-0b40-poa3-b0f9-c2e40344c888",
            DisplayOrder = 1,
            IsChildren = true
        });
        builder.Entity<MachineTaxonomyLevel>().HasData(new
        {
            Id = "335ab9fe-mcmc-4088-82a9-d27008688b55",
            Name = "Machine-nl",
            LanguageCode = "nl",
            LevelId = "88282458-0b40-poa3-b0f9-c2e40344c888",
            DisplayOrder = 1,
            IsChildren = true
        });

        builder.Entity<MachineTaxonmy>().HasData(new
        {
            Id = "335ab9fe-mc57-4088-82a9-d27008688mchine1",
            Title = "Machine-1",
            MachineTaxonomyLevelId = "88282458-0b40-poa3-b0f9-c2e40344c888"
        });
        builder.Entity<MachineTaxonmy>().HasData(new
        {
            Id = "335ab9fe-mc57-4088-82a9-d27008688mchine2",
            Title = "Machine-2",
            MachineTaxonomyLevelId = "88282458-0b40-poa3-b0f9-c2e40344c888"
        });
        builder.Entity<MachineTaxonmy>().HasData(new
        {
            Id = "335ab9fe-mc57-4088-82a9-d27008688mchine3",
            Title = "Machine-3",
            MachineTaxonomyLevelId = "88282458-0b40-poa3-b0f9-c2e40344c888"
        });

        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "aqwab9fe-po57-4088-82a9-d27008688mvk",
            Name = "Project",
            TypeId = "5",
            LanguageCode = "en",
            Label = "Project",
            DisplayOrder = 1
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "slaab9fe-po57-4088-82a9-d27008688kgd",
            Name = "Project(nl)",
            TypeId = "5",
            LanguageCode = "nl",
            Label = "Project",
            DisplayOrder = 1
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks",
            Name = "Last Week",
            TypeId = "1",
            LanguageCode = "en",
            Label = "Last Week",
            DisplayOrder = 2
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "eew9e479-pob3-40c6-ad61-e40dbe6a5111",
            Name = "Last Week(nl)",
            TypeId = "1",
            LanguageCode = "nl",
            Label = "Last Week",
            DisplayOrder = 2
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "qqqab9fe-qq57-4088-82a9-d27008688qqq",
            Name = "Current Week",
            TypeId = "2",
            LanguageCode = "en",
            Label = "Current Week",
            DisplayOrder = 3
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "zzzab9fe-po57-4088-82a9-d27008688zzz",
            Name = "Current Week(nl)",
            TypeId = "2",
            LanguageCode = "nl",
            Label = "Current Week",
            DisplayOrder = 3
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "bnb9e479-pob3-40c6-ad61-e40dbe6a5bnb",
            Name = "Last Month",
            TypeId = "3",
            LanguageCode = "en",
            Label = "Last Month",
            DisplayOrder = 4
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "wer9e479-pob3-40c6-ad61-e40dbe6a5wer",
            Name = "Last Month(nl)",
            TypeId = "3",
            LanguageCode = "nl",
            Label = "Last Month",
            DisplayOrder = 4
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "jfdjjf79-pob3-40c6-ad61-e40dbehdhbfh",
            Name = "Current Month",
            TypeId = "4",
            LanguageCode = "en",
            Label = "Current Month",
            DisplayOrder = 5
        });
        builder.Entity<VpFilterDropdown>().HasData(new
        {
            Id = "ytisid79-pob3-40c6-ad61-e40dbejfsjjd",
            Name = "Current Month(nl)",
            TypeId = "4",
            LanguageCode = "nl",
            Label = "Current Month",
            DisplayOrder = 5
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "hjkdab9fe-po57-4088-82a9-d27008688dhjh",
            Title = "HVAC",
            ContractId = "335ab9fe-po57-4088-82a9-d27008688tgg",
            ContractTaxonomyLevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8qq"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "gdab9fe-po57-4088-82a9-d27008dfghhj",
            Title = "Electricity",
            ContractId = "gyjkab9fe-po57-4088-82a9-d27008688fss",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "lgkf9fe-po57-4088-82a9-d27008dflfdkg",
            Title = "Sprinklers",
            ContractId = "gyjkab9fe-po57-4088-82a9-d27008688fss",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "lgkf9fe-po57-4088-SANI-d27008dflfdkg",
            Title = "Sanitary",
            ContractId = "gyjkab9fe-po57-4088-SANI-d27008688fss",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "dgb9fe-po57-4088-82a9-d27008dfgjujj",
            Title = "All -Air Systems",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "hjkdab9fe-po57-4088-82a9-d27008688dhjh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "lj989fe-afmd-lsdd-kafd-d27008dfgjksfn",
            Title = "Single Zone",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "dgb9fe-po57-4088-82a9-d27008dfgjujj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "mvnd9fe-afmd-lsdd-kafd-d27008dfgjajfnd",
            Title = "MultiZone",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "dgb9fe-po57-4088-82a9-d27008dfgjujj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "bfjs9fe-afmd-lsdd-kafd-d27008dfgjaadid",
            Title = "Terminal Reheat",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "dgb9fe-po57-4088-82a9-d27008dfgjujj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "kdca9fe-afmd-lsdd-kafd-d27008dfgjaawfm",
            Title = "Dual duct",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "dgb9fe-po57-4088-82a9-d27008dfgjujj"
        });

        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "bfer9fe-afmd-lsdd-kafd-d27008dfgjaasgkv",
            Title = "Variable Air Volume(VAV)",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "dgb9fe-po57-4088-82a9-d27008dfgjujj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "5569fe-po57-4088-82a9-d27008dfgj3ien",
            Title = "Air-Water Systems",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "hjkdab9fe-po57-4088-82a9-d27008688dhjh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "lmdfkfe-po57-4088-82a9-d27008dfgasdk",
            Title = "Fan Coil Units",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "5569fe-po57-4088-82a9-d27008dfgj3ien"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "mnnckfe-po57-4088-82a9-d27008dfgksck",
            Title = "Induction Units",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "5569fe-po57-4088-82a9-d27008dfgj3ien"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "mcd9fe-po57-4088-82a9-d27008dfgjabfad",
            Title = "All-Water Systems",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "hjkdab9fe-po57-4088-82a9-d27008688dhjh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "qertfe-po57-4088-82a9-d27008dfgjabqert",
            Title = "Fan Coil Units",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "mcd9fe-po57-4088-82a9-d27008dfgjabfad"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "yiuo9fe-po57-4088-82a9-d27008dfgjabksvn",
            Title = "Water-source Heat Pumps",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "hjkdab9fe-po57-4088-82a9-d27008688dhjh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "jfgf9fe-po57-4088-82a9-d27008dfgjabktyk",
            Title = "Heating and Cooling panels",
            ContractId = "jgjkab9fe-po57-4088-82a9-d2700868uhfhf",
            ContractTaxonomyLevelId = "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh",
            ParentId = "hjkdab9fe-po57-4088-82a9-d27008688dhjh"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "mbgej9fe-po57-4088-82a9-d27008dfgrthg",
            Title = "Lightning & Power",
            ContractId = "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs",
            ContractTaxonomyLevelId = "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf",
            ParentId = "gdab9fe-po57-4088-82a9-d27008dfghhj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "qcfj9fe-po57-4088-82a9-d27008dfoiy",
            Title = "Low Voltage Ground",
            ContractId = "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs",
            ContractTaxonomyLevelId = "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf",
            ParentId = "mbgej9fe-po57-4088-82a9-d27008dfgrthg"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "lgkf9fe-po57-4088-82a9-d27008dtyjh",
            Title = "Low Voltage Connection",
            ContractId = "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs",
            ContractTaxonomyLevelId = "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf",
            ParentId = "mbgej9fe-po57-4088-82a9-d27008dfgrthg"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "mkngj9fe-po57-4088-82a9-d27008dfgrwwff",
            Title = "Low Power Installation",
            ContractId = "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs",
            ContractTaxonomyLevelId = "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf",
            ParentId = "gdab9fe-po57-4088-82a9-d27008dfghhj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "kjhgj9fe-po57-4088-82a9-d27008dfgrytut",
            Title = "High Voltage",
            ContractId = "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs",
            ContractTaxonomyLevelId = "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf",
            ParentId = "gdab9fe-po57-4088-82a9-d27008dfghhj"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "fhgjj9fe-po57-4088-82a9-d27008dfgfdgdf",
            Title = "Wet Pipe System",
            ContractId = "dgdgsb9fe-afffg-dfg-sdd-d2700868uhsfs",
            ContractTaxonomyLevelId = "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf",
            ParentId = "lgkf9fe-po57-4088-82a9-d27008dflfdkg"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "ujgkkjj9fe-po57-4088-82a9-d27008dfgfsdmkf",
            Title = "Dry Pipe System",
            ContractId = "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs",
            ContractTaxonomyLevelId = "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff",
            ParentId = "lgkf9fe-po57-4088-82a9-d27008dflfdkg"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "lkdfkjj9fe-po57-4088-82a9-d27008dfgfsdfkk",
            Title = "Pre-Action System",
            ContractId = "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs",
            ContractTaxonomyLevelId = "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff",
            ParentId = "lgkf9fe-po57-4088-82a9-d27008dflfdkg"
        });
        builder.Entity<ContractTaxonomy>().HasData(new
        {
            Id = "mvffkjj9fe-po57-4088-82a9-d27008dfgfsdmmd",
            Title = "Deluge System",
            ContractId = "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs",
            ContractTaxonomyLevelId = "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff",
            ParentId = "lgkf9fe-po57-4088-82a9-d27008dflfdkg"
        });

        builder.Entity<ProjectClassificationBuisnessUnit>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "Goddeeris",
            LanguageCode = "en",
            TypeId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 1,
            Label = "Goddeeris"
        });
        builder.Entity<ProjectClassificationBuisnessUnit>().HasData(new
        {
            Id = "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg",
            Name = "Goddeeris",
            LanguageCode = "nl",
            TypeId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 1,
            Label = "Goddeeris"
        });


        builder.Entity<ProjectClassificationSize>().HasData(new
        {
            Id = "iisia893-0267-48af-b65a-b7a93ebd1ccf",
            Name = "1-999",
            LanguageCode = "en",
            TypeId = "ifnfk4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 1,
            Label = "1-999"
        });
        builder.Entity<ProjectClassificationSize>().HasData(new
        {
            Id = "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg",
            Name = "1-999(nl)",
            LanguageCode = "nl",
            TypeId = "ifnfk4e8d-8e8c-487d-8170-6b91c89fc3da",
            DisplayOrder = 1,
            Label = "1-999"
        });
        builder.Entity<ProjectClassificationSize>().HasData(new
        {
            Id = "nnn6a893-0267-48af-b65a-b7a93ebddsndsgk",
            Name = "Size 2",
            LanguageCode = "en",
            TypeId = "kjsdkjg4e8d-fhhd-487d-8170-6b91c89fdddaad",
            DisplayOrder = 2,
            Label = "Size 2"
        });
        builder.Entity<ProjectClassificationSize>().HasData(new
        {
            Id = "dmagsa893-0267-48af-b65a-b7a93ebdfdmgmmm",
            Name = "Size 2(nl)",
            LanguageCode = "nl",
            TypeId = "kjsdkjg4e8d-fhhd-487d-8170-6b91c89fdddaad",
            DisplayOrder = 2,
            Label = "Size 2"
        });


        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "uurya893-0267-48af-b65a-b7a93ebd1wem",
            Name = "Warehouse",
            LanguageCode = "en",
            TypeId = "zzzbk4e8d-8e8c-487d-8170-6b91c89fczzz",
            DisplayOrder = 1,
            Label = "Warehouse"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "qqqgsa893-0267-48af-b65a-b7a93ebdfdgg",
            Name = "Magazijn",
            LanguageCode = "nl",
            TypeId = "zzzbk4e8d-8e8c-487d-8170-6b91c89fczzz",
            DisplayOrder = 1,
            Label = "Warehouse"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "ooo6a893-0267-48af-b65a-b7a93ebddsndsgk",
            Name = "Production",
            LanguageCode = "en",
            TypeId = "eeedkjg4e8d-fhhd-487d-8170-6b91c89fdddeee",
            DisplayOrder = 2,
            Label = "Production"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "ncngsa893-0267-48af-b65a-b7a93ebdfdmgmmm",
            Name = "Productie",
            LanguageCode = "nl",
            TypeId = "eeedkjg4e8d-fhhd-487d-8170-6b91c89fdddeee",
            DisplayOrder = 2,
            Label = "Production"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "001d99d9-1dc2-4735-92ae-c23bd131d095",
            Name = "Office",
            LanguageCode = "en",
            TypeId = "54302053-1edd-43f8-a203-e52285a52a16",
            DisplayOrder = 3,
            Label = "Office"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "00266462-f21a-43d5-ba8a-5c755a33339a",
            Name = "Bureaus",
            LanguageCode = "nl",
            TypeId = "54302053-1edd-43f8-a203-e52285a52a16",
            DisplayOrder = 3,
            Label = "Office"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "888d99d9-1dc2-4735-92ae-c23bd131d999",
            Name = "Appartments",
            LanguageCode = "en",
            TypeId = "bbb02053-1edd-43f8-a203-e52285a52bbb",
            DisplayOrder = 4,
            Label = "Appartments"
        });
        builder.Entity<ProjectClassificationConstructionType>().HasData(new
        {
            Id = "11166462-f21a-43d5-ba8a-5c755a333111",
            Name = "Appartementen",
            LanguageCode = "nl",
            TypeId = "bbb02053-1edd-43f8-a203-e52285a52bbb",
            DisplayOrder = 4,
            Label = "Appartments"
        });


        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "ya893-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "Harbour",
            LanguageCode = "en",
            TypeId = "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb",
            DisplayOrder = 1,
            Label = "Harbour"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "kkk93-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "Harbour(nl)",
            LanguageCode = "nl",
            TypeId = "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb",
            DisplayOrder = 1,
            Label = "Harbour"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "ppp93-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "Office & Commercial",
            LanguageCode = "en",
            TypeId = "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv",
            DisplayOrder = 2,
            Label = "Office & Commercial"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "zzz93-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "Office & Commercial(nl)",
            LanguageCode = "nl",
            TypeId = "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv",
            DisplayOrder = 2,
            Label = "Office & Commercial"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "d21f11f8-cf12-46ca-85c8-804faf7e70da",
            Name = "Residential",
            LanguageCode = "en",
            TypeId = "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1",
            DisplayOrder = 3,
            Label = "Residential"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "d4f6116f-5026-424b-94c3-e404d39db195",
            Name = "Residential(nl)",
            LanguageCode = "nl",
            TypeId = "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1",
            DisplayOrder = 3,
            Label = "Residential"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "cddca425-a124-4935-b1f3-c8db6b325058",
            Name = "Industry",
            LanguageCode = "en",
            TypeId = "f73b0526-3271-49a1-a924-84861f96b5d9",
            DisplayOrder = 4,
            Label = "Industry"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "e59146f9-8be1-4cac-81b0-076e5fd5bfa0",
            Name = "Industry(nl)",
            LanguageCode = "nl",
            TypeId = "f73b0526-3271-49a1-a924-84861f96b5d9",
            DisplayOrder = 4,
            Label = "Industry"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "yyyy012b-e80e-4947-b5b7-9f979a53bca4",
            Name = "Care & education",
            LanguageCode = "en",
            TypeId = "1bf365f3-072a-412e-b424-635452b4bd59",
            DisplayOrder = 5,
            Label = "Care & education"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "134669a5-ca20-4d33-ab17-827bc0011e0f",
            Name = "Care & education(nl)",
            LanguageCode = "nl",
            TypeId = "1bf365f3-072a-412e-b424-635452b4bd59",
            DisplayOrder = 5,
            Label = "Care & education"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "qqqq73f7-8cf7-4601-9b15-d4ce46022a78",
            Name = "Marine & Civil works",
            LanguageCode = "en",
            TypeId = "398ae077-ea6e-4619-9a14-dfa47e12aa39",
            DisplayOrder = 6,
            Label = "Marine & Civil works"
        });
        builder.Entity<ProjectClassificationSector>().HasData(new
        {
            Id = "2d56177d-cf1f-4b56-9565-c641eed3be52",
            Name = "Marine & Civil works(nl)",
            LanguageCode = "nl",
            TypeId = "398ae077-ea6e-4619-9a14-dfa47e12aa39",
            DisplayOrder = 6,
            Label = "Marine & Civil works"
        });
        builder.Entity<ProjectLanguage>().HasData(new
        {
            Id = "yyy93-jsjj-fmms-amdm-b7a93ebd1www",
            Name = "en",
            LanguageCode = "en",
            TypeId = "tttdkjg4e8d-fhhd-487d-8170-6b91c89fdddttt",
            DisplayOrder = 1
        });
        builder.Entity<ProjectLanguage>().HasData(new
        {
            Id = "iii93-jsjj-fmms-amdm-b7a93ebd1iii",
            Name = "nl",
            LanguageCode = "en",
            TypeId = "tttdkjg4e8d-fhhd-487d-8170-6b91c89fdddttt",
            DisplayOrder = 2
        });

        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-Lota2ab276",
            Name = "Contractor added to lot",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-con1-ad25-Lot0d49477",
            DisplayOrder = 1
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "iii93-jsjj-nnnn-amdm-b7a93ebd1iii",
            Name = "Requested To Join Tender",
            LanguageCode = "en",
            StatusId = "bvxbdkjg4e8d-fhhd-487d-8170-6b91c89fdvnfd",
            DisplayOrder = 11
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-Lot897f5bb",
            Name = "Joined Tender",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-con2-ad25-Lot0d49477",
            DisplayOrder = 2
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-Lotf10e27c",
            Name = "Reminder Sent To Request To Join Tender",
            LanguageCode = "en",
            StatusId = "94282458-0b40-con3-b0f9-Lot344c8f1",
            DisplayOrder = 3
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-Lot8688bae",
            Name = "Dossier Sent",
            LanguageCode = "en",
            StatusId = "94282458-0b40-con4-b0f9-Lot344c8f1",
            DisplayOrder = 4
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-Lot6ae42af",
            Name = "Confirmation Dossier Received",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-con5-8c17-Lotecdb84c",
            DisplayOrder = 5
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-Lot00e1eed",
            Name = "Reminder Sent To Confirm Dossier Received",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-con6-8c17-Lotecdb84c",
            DisplayOrder = 6
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-Lotd1c5f5a",
            Name = "Dossier Published",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-con7-8170-Lot89fc3da",
            DisplayOrder = 7
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-Lotebd1ccf",
            Name = "Reminder Sent To Publish Dossier",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-con8-8170-Lot89fc3da",
            DisplayOrder = 8
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-Lot1e2391a",
            Name = "Open Comment For BM Engineering",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-con9-b337-Lota82addb",
            DisplayOrder = 9
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-Lote6a51ac",
            Name = "Open Comment For Contractor",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-con10-b337-Lota82addb",
            DisplayOrder = 10
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "bdd9wawa-75b3-40c6-ad61-Lote6a51ac",
            Name = "Added to Contract",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-added-b337-Lota82addb",
            DisplayOrder = 11
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "bdnlwawa-75b3-40c6-ad61-Lote6a51ac",
            Name = "Added to Contract(nl)",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-added-b337-Lota82addb",
            DisplayOrder = 11
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "hdhdhhcd5a-0941-4c56-9c13-Lota2ab276",
            Name = "Awarded",
            LanguageCode = "en",
            StatusId = "nnnnad0b-2e84-con1-ad25-Lot0d49477",
            DisplayOrder = 14
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "qewrt3-jsjj-nnnn-amdm-b7a93ebd1iii",
            Name = "Awarded nl",
            LanguageCode = "nl",
            StatusId = "nnnnad0b-2e84-con1-ad25-Lot0d49477",
            DisplayOrder = 14
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "nvfjjsjhhcd5a-0941-4c56-9c13-Lota2ab276",
            Name = "Not Awarded",
            LanguageCode = "en",
            StatusId = "xxxxad0b-2e84-con1-ad25-Lot0d49477",
            DisplayOrder = 15
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "ndjjd3-jsjj-nnnn-amdm-b7a93ebd1iii",
            Name = " Not Awarded nl",
            LanguageCode = "nl",
            StatusId = "xxxxad0b-2e84-con1-ad25-Lot0d49477",
            DisplayOrder = 15
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "qqqab9fe-msms-4088-Lot1-d27008688qqq",
            Name = "pdf",
            TypeId = "oo10e768-msms-po02-Lot1-ee367a82adooo",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "zzzab9fe-msms-4088-Lot2-d27008688zzz",
            Name = "Image",
            TypeId = "oo10e768-msms-po02-Lot2-ee367a82adooo",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnb",
            Name = "URL",
            TypeId = "2210e768-msms-po02-Lot3-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 3
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "wer9e479-msms-40c6-Lot4-e40dbe6a5wer",
            Name = "Word Document",
            TypeId = "2210e768-msms-po02-Lot4-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 4
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "qqqab9fe-msms-4088-Lot1-d27008688qnl",
            Name = "pdf(nl)",
            TypeId = "oo10e768-msms-po02-Lot1-ee367a82adooo",
            LanguageCode = "nl",
            DisplayOrder = 1
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "zzzab9fe-msms-4088-Lot2-d27008688znl",
            Name = "Image(nl)",
            TypeId = "oo10e768-msms-po02-Lot2-ee367a82adooo",
            LanguageCode = "nl",
            DisplayOrder = 2
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "bnb9e479-msms-40c6-Lot3-e40dbe6a5bnl",
            Name = "URL(nl)",
            TypeId = "2210e768-msms-po02-Lot3-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 3
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "wer9e479-msms-40c6-Lot4-e40dbe6a5wnl",
            Name = "Word Document(nl)",
            TypeId = "2210e768-msms-po02-Lot4-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 4
        });
        builder.Entity<ContractorProductItemType>().HasData(new
        {
            Id = "wer9e479-msms-Item-Lot1-e40dbe6a5wer",
            Name = "Lot",
            TypeId = "2210e768-msms-Item-Lot1-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<ContractorProductItemType>().HasData(new
        {
            Id = "wer9e479-msms-Item-Lot2-e40dbe6a5wer",
            Name = "Contractor",
            TypeId = "2210e768-msms-Item-Lot2-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<ContractorProductItemType>().HasData(new
        {
            Id = "wer9e4nl-msms-Item-Lot1-e40dbe6a5wer",
            Name = "Lot (nl)",
            TypeId = "2210e768-msms-Item-Lot1-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 1
        });
        builder.Entity<ContractorProductItemType>().HasData(new
        {
            Id = "wer9e4nl-msms-Item-Lot2-e40dbe6a5wer",
            Name = "Contractor (nl)",
            TypeId = "2210e768-msms-Item-Lot2-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 2
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-cowf-f5fdca2ab276",
            Name = "Pending Development",
            LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-cowf-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-cowf-657dd897f5bb",
            Name = "in voorbereiding",
            LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-cowf-618d80d49477",
            DisplayOrder = 1
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-cowf-ce7fbf10e27c",
            Name = "Dossier Published",
            LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-cowf-c2e40344c8f1",
            DisplayOrder = 2
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-cowf-d27008688bae",
            Name = "Dossier Published(nl)",
            LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-cowf-c2e40344c8f1",
            DisplayOrder = 2
        });

        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-cowf-9cb166ae42af",
            Name = "Tender Closed",
            LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-cowf-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-cowf-d4e1400e1eed",
            Name = "Tender Closed(nl)",
            LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-cowf-cacdfecdb84c",
            DisplayOrder = 3
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-n32n-213d2d1c5f5a",
            Name = "Tender Decided",
            LanguageCode = "en",
            StatusId = "8e8c4e8d-7bcb-487d-cowf-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-n32n-b7a93ebd1ccf",
            Name = "Tender Decided(nl)",
            LanguageCode = "nl",
            StatusId = "8e8c4e8d-7bcb-487d-cowf-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "77143c60-ff45-n32n-cowf-213d2d1c5f5a",
            Name = "In Construction",
            LanguageCode = "en",
            StatusId = "487d7e8d-8e8c-bcb4-cowf-6b91c89fc3da",
            DisplayOrder = 5
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "4e01a893-n32n-48af-cowf-b7a93ebd1ccf",
            Name = "In Construction(nl)",
            LanguageCode = "nl",
            StatusId = "487d7e8d-8e8c-bcb4-cowf-6b91c89fc3da",
            DisplayOrder = 5
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-cowf-213d2d1c5f5a",
            Name = "Approved",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da",
            DisplayOrder = 6
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-cowf-b7a93ebd1ccf",
            Name = "goedgekeurd",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-cowf-6b91c89fc3da",
            DisplayOrder = 6
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a",
            Name = "Handed Over",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-cowf-ee367a82addb",
            DisplayOrder = 7
        });
        builder.Entity<ContractorStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac",
            Name = "afgewerkt en doorgegeven",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-cowf-ee367a82addb",
            DisplayOrder = 7
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-cowf-213d2d1c5f5a",
            Name = "New",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-cowf-b7a93ebd1ccf",
            Name = "In preparation",
            LanguageCode = "en",
            StatusId = "7bcbAcce-8e8c-487d-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-cowf-dd1bf1e2391a",
            Name = "In review",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-Acce-ee367a82addb",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-cowf-e40dbe6a51ac",
            Name = "Accepted",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-Acce-cowf-ee367a82addb",
            DisplayOrder = 4
        });

        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-lost-213d2d1c5f5a",
            Name = "New(nl)",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-lost-b7a93ebd1ccf",
            Name = "In preparation(nl)",
            LanguageCode = "nl",
            StatusId = "7bcbAcce-8e8c-487d-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-lost-dd1bf1e2391a",
            Name = "In review(nl)",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-Acce-ee367a82addb",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-lost-e40dbe6a51ac",
            Name = "Accepted(nl)",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-Acce-cowf-ee367a82addb",
            DisplayOrder = 4
        });

        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "77143c60-Field-4ca2-cowf-213d2d1c5f5a",
            Name = "Article Number",
            LanguageCode = "en",
            FieldId = "7bcb4e8d-Field-487d-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "4e01a893-Field-48af-cowf-b7a93ebd1ccf",
            Name = "Article Description",
            LanguageCode = "en",
            FieldId = "7bcbAcce-Field-487d-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel3-cowf-dd1bf1e2391a",
            Name = "Measuring code",
            LanguageCode = "en",
            FieldId = "4010e768-Field-4702-Acce-ee367a82addb",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel4-cowf-dd1bf1e2391a",
            Name = "Var. 1",
            LanguageCode = "en",
            FieldId = "4010e768-Field-Var1-Acce-ee367a82addb",
            DisplayOrder = 4
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field5-cowf-dd1bf1e2391a",
            Name = "Var. 2",
            LanguageCode = "en",
            FieldId = "4010e768-Field5-4702-Acce-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field6-cowf-dd1bf1e2391a",
            Name = "Var. 3",
            LanguageCode = "en",
            FieldId = "4010e768-Field6-4702-Acce-ee367a82addb",
            DisplayOrder = 6
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field7-cowf-dd1bf1e2391a",
            Name = "Var. 4",
            LanguageCode = "en",
            FieldId = "4010e768-Field7-4702-Acce-ee367a82addb",
            DisplayOrder = 7
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field8-cowf-dd1bf1e2391a",
            Name = "Var. 5",
            LanguageCode = "en",
            FieldId = "4010e768-Field8-4702-Acce-ee367a82addb",
            DisplayOrder = 8
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field9-cowf-dd1bf1e2391a",
            Name = "Amount",
            LanguageCode = "en",
            FieldId = "4010e768-Field9-4702-Acce-ee367a82addb",
            DisplayOrder = 9
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel10-cowf-dd1bf1e2391a",
            Name = "Unit Price",
            LanguageCode = "en",
            FieldId = "4010e768-Fiel10-4702-Acce-ee367a82addb",
            DisplayOrder = 10
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel11-cowf-dd1bf1e2391a",
            Name = "Total Price",
            LanguageCode = "en",
            FieldId = "4010e768-Fiel11-4702-Acce-ee367a82addb",
            DisplayOrder = 11
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "77143c60-Field-4ca2-clfi-213d2d1c5f5a",
            Name = "Article Number(nl)",
            LanguageCode = "nl",
            FieldId = "7bcb4e8d-Field-487d-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "4e01a893-Field-48af-clfi-b7a93ebd1ccf",
            Name = "Article Description(nl)",
            LanguageCode = "nl",
            FieldId = "7bcbAcce-Field-487d-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel3-clfi-dd1bf1e2391a",
            Name = "Measuring code(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field-4702-Acce-ee367a82addb",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel4-clfi-dd1bf1e2391a",
            Name = "Var. 1(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field-Var1-Acce-ee367a82addb",
            DisplayOrder = 4
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field5-clfi-dd1bf1e2391a",
            Name = "Var. 2(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field5-4702-Acce-ee367a82addb",
            DisplayOrder = 5
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field6-clfi-dd1bf1e2391a",
            Name = "Var. 3(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field6-4702-Acce-ee367a82addb",
            DisplayOrder = 6
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field7-clfi-dd1bf1e2391a",
            Name = "Var. 4(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field7-4702-Acce-ee367a82addb",
            DisplayOrder = 7
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field8-clfi-dd1bf1e2391a",
            Name = "Var. 5(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field8-4702-Acce-ee367a82addb",
            DisplayOrder = 8
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Field9-clfi-dd1bf1e2391a",
            Name = "Amount(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Field9-4702-Acce-ee367a82addb",
            DisplayOrder = 9
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel10-clfi-dd1bf1e2391a",
            Name = "Unit Price(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Fiel10-4702-Acce-ee367a82addb",
            DisplayOrder = 10
        });
        builder.Entity<CommentLogField>().HasData(new
        {
            Id = "8d109134-ee8d-Fiel11-clfi-dd1bf1e2391a",
            Name = "Total Price(nl)",
            LanguageCode = "nl",
            FieldId = "4010e768-Fiel11-4702-Acce-ee367a82addb",
            DisplayOrder = 11
        });

        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "77143c60-Field-4ca2-Seve-213d2d1c5f5a",
            Name = "Severe",
            LanguageCode = "en",
            SeverityId = "7bcb4e8d-Field-Seve-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Field-Sign-cowf-b7a93ebd1ccf",
            Name = "Significant",
            LanguageCode = "en",
            SeverityId = "7bcbAcce-Field-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Fiel3-Sign-cowf-b7a93ebd1ccf",
            Name = "Moderate",
            LanguageCode = "en",
            SeverityId = "7bcbAcce-Fiel3-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Fiel4-Sign-cowf-b7a93ebd1ccf",
            Name = "Minor",
            LanguageCode = "en",
            SeverityId = "7bcbAcce-Fiel4-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Fiel5-Sign-cowf-b7a93ebd1ccf",
            Name = "Negligible",
            LanguageCode = "en",
            SeverityId = "7bcbAcce-Fiel5-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 5
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "77143c60-Field-4ca2-Seve-asas2d1c5f5a",
            Name = "Severe(nl)",
            LanguageCode = "nl",
            SeverityId = "7bcb4e8d-Field-Seve-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Field-Sign-cowf-asas3ebd1ccf",
            Name = "Significant(nl)",
            LanguageCode = "nl",
            SeverityId = "7bcbAcce-Field-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Fiel3-Sign-cowf-asas3ebd1ccf",
            Name = "Moderate(nl)",
            LanguageCode = "nl",
            SeverityId = "7bcbAcce-Fiel3-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Fiel4-Sign-cowf-asas3ebd1ccf",
            Name = "Minor(nl)",
            LanguageCode = "nl",
            SeverityId = "7bcbAcce-Fiel4-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<CommentLogSeverity>().HasData(new
        {
            Id = "4e01a893-Fiel5-Sign-cowf-asas3ebd1ccf",
            Name = "Negligible(nl)",
            LanguageCode = "nl",
            SeverityId = "7bcbAcce-Fiel5-Sign-cowf-6b91c89fc3da",
            DisplayOrder = 5
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "77143c60-Fiel1-4ca2-Very-213d2d1c5f5a",
            Name = "Very High",
            LanguageCode = "en",
            PriorityId = "7bcb4e8d-Fiel1-Very-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel2-High-cowf-b7a93ebd1ccf",
            Name = "High",
            LanguageCode = "en",
            PriorityId = "7bcbAcce-Fiel2-High-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel3-High-cowf-b7a93ebd1ccf",
            Name = "Medium",
            LanguageCode = "en",
            PriorityId = "7bcbAcce-Fiel3-High-cowf-6b91c89fc3da",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel4-High-cowf-b7a93ebd1ccf",
            Name = "Low",
            LanguageCode = "en",
            PriorityId = "7bcbAcce-Fiel4-High-cowf-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel5-High-cowf-b7a93ebd1ccf",
            Name = "Very Low",
            LanguageCode = "en",
            PriorityId = "7bcbAcce-Fiel5-High-cowf-6b91c89fc3da",
            DisplayOrder = 5
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "77143c60-Fiel1-4ca2-Very-asas2d1c5f5a",
            Name = "Very High(nl)",
            LanguageCode = "nl",
            PriorityId = "7bcb4e8d-Fiel1-Very-cowf-6b91c89fAcce",
            DisplayOrder = 1
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel2-High-cowf-asas3ebd1ccf",
            Name = "High(nl)",
            LanguageCode = "nl",
            PriorityId = "7bcbAcce-Fiel2-High-cowf-6b91c89fc3da",
            DisplayOrder = 2
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel3-High-cowf-asas3ebd1ccf",
            Name = "Medium(nl)",
            LanguageCode = "nl",
            PriorityId = "7bcbAcce-Fiel3-High-cowf-6b91c89fc3da",
            DisplayOrder = 3
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel4-High-cowf-asas3ebd1ccf",
            Name = "Low(nl)",
            LanguageCode = "nl",
            PriorityId = "7bcbAcce-Fiel4-High-cowf-6b91c89fc3da",
            DisplayOrder = 4
        });
        builder.Entity<CommentLogPriority>().HasData(new
        {
            Id = "4e01a893-Fiel5-High-cowf-asas3ebd1ccf",
            Name = "Very Low(nl)",
            LanguageCode = "nl",
            PriorityId = "7bcbAcce-Fiel5-High-cowf-6b91c89fc3da",
            DisplayOrder = 5
        });

        builder.Entity<CommentChangeType>().HasData(new
        {
            Id = "hkdsg-jsjj-fmms-amdm-b7a93ebrghthh",
            Name = "Change Request",
            LanguageCode = "en",
            TypeId = "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb",
            DisplayOrder = 1
        });

        builder.Entity<CommentChangeType>().HasData(new
        {
            Id = "kkk93-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "veranderingsaanvraag",
            LanguageCode = "nl",
            TypeId = "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb",
            DisplayOrder = 1
        });
        builder.Entity<CommentChangeType>().HasData(new
        {
            Id = "ppp93-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "Comment",
            LanguageCode = "en",
            TypeId = "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv",
            DisplayOrder = 2
        });
        builder.Entity<CommentChangeType>().HasData(new
        {
            Id = "zzz93-jsjj-fmms-amdm-b7a93ebd1wem",
            Name = "commentaar",
            LanguageCode = "nl",
            TypeId = "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv",
            DisplayOrder = 2
        });
        builder.Entity<CommentChangeType>().HasData(new
        {
            Id = "d21f11f8-cf12-46ca-85c8-804faf7e70da",
            Name = "Issue",
            LanguageCode = "en",
            TypeId = "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1",
            DisplayOrder = 3
        });
        builder.Entity<CommentChangeType>().HasData(new
        {
            Id = "d4f6116f-5026-424b-94c3-e404d39db195",
            Name = "Kwestie",
            LanguageCode = "nl",
            TypeId = "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1",
            DisplayOrder = 3
        });
        builder.Entity<OrganizationTeamRole>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-e40dbe6a5wer",
            Name = "Foreman",
            RoleId = "2210e768-msms-Item-team1-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<OrganizationTeamRole>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-e40dbe6a5wer",
            Name = "Worker",
            RoleId = "2210e768-msms-Item-team2-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<OrganizationTeamRole>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-nl0dbe6a5wer",
            Name = "Voorman",
            RoleId = "2210e768-msms-Item-team1-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 1
        });
        builder.Entity<OrganizationTeamRole>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-nl0dbe6a5wer",
            Name = "Arbeider",
            RoleId = "2210e768-msms-Item-team2-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 2
        });

        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-e40dbe6a5wer",
            Name = "1",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad21",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-e40dbe6a5wer",
            Name = "2",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad22",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-nl0dbe6a5wer",
            Name = "3",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad23",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-nl0dbe6a5wer",
            Name = "4",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad24",
            LanguageCode = "en"
        });

        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-e40dbe6a5we5",
            Name = "5",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad25",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-e40dbe6a5we6",
            Name = "6",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad26",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-nl0dbe6a5we7",
            Name = "7",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad27",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-nl0dbe6a5we8",
            Name = "8",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad28",
            LanguageCode = "en"
        });

        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-e40dbe6a5we9",
            Name = "9",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad29",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-e40dbe6a5w10",
            Name = "10",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad26",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-nl0dbe6a5w11",
            Name = "11",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad11",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-nl0dbe6a5w12",
            Name = "12",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad12",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-e40dbe6a5w13",
            Name = "13",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad13",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-e40dbe6a5w14",
            Name = "14",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad14",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-nl0dbe6a5w15",
            Name = "15",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad15",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-nl0dbe6a5w16",
            Name = "16",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad16",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-e40dbe6a5w17",
            Name = "17",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad17",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-e40dbe6a5w18",
            Name = "18",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad18",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org1-Item-team1-nl0dbe6a5w19",
            Name = "19",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad19",
            LanguageCode = "en"
        });
        builder.Entity<ContractorPsOrderNumber>().HasData(new
        {
            Id = "wer9e479-org2-Item-team2-nl0dbe6a5w20",
            Name = "20",
            TypeId = "2210e768-msms-kknk-jhhk-ee367a82ad20",
            LanguageCode = "en"
        });

        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "2210e768-tool-kknk-jhhk-ee367a82ad17",
            LocaleCode = "Tools",
            DisplayOrder = 0,
            Title = "Tools"
        });
        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "2210e768-mate-kknk-jhhk-ee367a82ad17",
            LocaleCode = "Materials",
            DisplayOrder = 0,
            Title = "Materials"
        });
        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "2210e768-cons-kknk-jhhk-ee367a82ad17",
            LocaleCode = "Consumables",
            DisplayOrder = 0,
            Title = "Consumables"
        });
        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "2210e768-human-kknk-jhhk-ee367a82ad17",
            LocaleCode = "Human Resources",
            DisplayOrder = 0,
            Title = "Human Resources"
        });

        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "wer9e479-org2-TOOL-QWERT-nl0dbe6a5w16",
            Label = "Tools",
            LanguageCode = "en",
            CpcResourceFamilyId = "2210e768-tool-kknk-jhhk-ee367a82ad17"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "wer9e479-org2-Mate-QWERT-nl0dbe6a5w16",
            Label = "Materials",
            LanguageCode = "en",
            CpcResourceFamilyId = "2210e768-mate-kknk-jhhk-ee367a82ad17"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "wer9e479-org2-Cons-QWERT-nl0dbe6a5w16",
            Label = "Consumables",
            LanguageCode = "en",
            CpcResourceFamilyId = "2210e768-cons-kknk-jhhk-ee367a82ad17"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "wer9e479-org2-Human-QWERT-nl0dbe6a5w16",
            Label = "Human Resources",
            LanguageCode = "en",
            CpcResourceFamilyId = "2210e768-human-kknk-jhhk-ee367a82ad17"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "wer9e479-org2-truck-1WERT-nl0dbe6a5w16",
            Label = "Truck",
            LanguageCode = "en",
            CpcResourceFamilyId = "2210e768-human-kknk-truck-ee367a82ad17"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "wer9e479-org2-truck-2WERT-nl0dbe6a5w16",
            Label = "Truck-nl",
            LanguageCode = "nl",
            CpcResourceFamilyId = "2210e768-human-kknk-truck-ee367a82ad17"
        });

        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "euuhde479-org2-mixer-1WERT-nl0dbe6a5w16",
            Label = "Concrete Mixer",
            LanguageCode = "en",
            CpcResourceFamilyId = "nbn0e768-human-kknk-mixer-ee367a82ad17"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "lkl9e479-org2-mixer-2WERT-nl0dbe6a5w16",
            Label = "Concrete Mixer-nl",
            LanguageCode = "nl",
            CpcResourceFamilyId = "nbn0e768-human-kknk-mixer-ee367a82ad17"
        });

        builder.Entity<CpcBasicUnitOfMeasure>().HasData(new
        {
            Id = "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16",
            Name = "Cubuic Meters",
            LocaleCode = "m3",
            DisplayOrder = 0
        });

        builder.Entity<CpcBasicUnitOfMeasureLocalizedData>().HasData(new
        {
            Id = "euuhde479-org2-m3-1WERT-nl0dbe6a5w16",
            Label = "Cubic Meters",
            LanguageCode = "en",
            CpcBasicUnitOfMeasureId = "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16"
        });
        builder.Entity<CpcBasicUnitOfMeasureLocalizedData>().HasData(new
        {
            Id = "jfsee479-org2-m3-2WERT-nl0dbe6a5w16",
            Label = "Cubic Meters-nl",
            LanguageCode = "nl",
            CpcBasicUnitOfMeasureId = "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16"
        });

        builder.Entity<CiawStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ciaws-e40dbe6a51ac",
            Name = "Pending",
            LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-ciaws-ee367a82addb",
            DisplayOrder = "1"
        });
        builder.Entity<CiawStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-ciaws-213d2d1c5f5a",
            Name = "Completed",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce",
            DisplayOrder = "2"
        });
        builder.Entity<CiawStatus>().HasData(new
        {
            Id = "77143c60-ff45-cancl-ciaws-213d2d1c5f5a",
            Name = "Cancelled",
            LanguageCode = "en",
            StatusId = "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce",
            DisplayOrder = "2"
        });
        builder.Entity<CiawStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ciaws-e40dbe6a51nl",
            Name = "Pending(nl)",
            LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-ciaws-ee367a82addb",
            DisplayOrder = "1"
        });
        builder.Entity<CiawStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-ciaws-213d2d1c5fnl",
            Name = "Completed(nl)",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce",
            DisplayOrder = "2"
        });
        builder.Entity<CiawStatus>().HasData(new
        {
            Id = "77143c60-ff00-cancl-ciaws-213d2d1c5f5a",
            Name = "Cancelled(nl)",
            LanguageCode = "nl",
            StatusId = "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce",
            DisplayOrder = "2"
        });

        builder.Entity<TemporaryTeamName>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-team2-213d2d1c5fnl",
            Name = "Tijdelijk team",
            LanguageCode = "nl",
            NameId = "7bcb4e8d-8e8c-487d-team-6b91c89fAcce"
        });
        builder.Entity<TemporaryTeamName>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-team1-213d2d1c5fnl",
            Name = "Temporary Team",
            LanguageCode = "en",
            NameId = "7bcb4e8d-8e8c-487d-team-6b91c89fAcce"
        });


        builder.Entity<PriceCalculatorTaxonomyLevel>().HasData(new
        {
            Id = "vv5ab9fe-po57-4088-82a9-d27008688bbb",
            Name = "Project",
            LanguageCode = "en",
            LevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8kk",
            DisplayOrder = 1
        });
        builder.Entity<PriceCalculatorTaxonomyLevel>().HasData(new
        {
            Id = "uud9e479-pob3-40c6-ad61-e40dbe6a5111",
            Name = "Project nl",
            LanguageCode = "nl",
            LevelId = "qq282458-0b40-poa3-b0f9-c2e40344c8kk",
            DisplayOrder = 1
        });
        builder.Entity<PriceCalculatorTaxonomyLevel>().HasData(new
        {
            Id = "gg5ab9fe-po57-4088-82a9-d27008688ttt",
            Name = "Phase",
            LanguageCode = "en",
            LevelId = "vvvv82458-0b40-poa3-b0f9-c2e40344cvvv",
            DisplayOrder = 2
        });
        builder.Entity<PriceCalculatorTaxonomyLevel>().HasData(new
        {
            Id = "kkd9e479-pob3-40c6-ad61-e40dbe6a5444",
            Name = "Phase nl",
            LanguageCode = "nl",
            LevelId = "vvvv82458-0b40-poa3-b0f9-c2e40344cvvv",
            DisplayOrder = 2
        });
        builder.Entity<PriceCalculatorTaxonomyLevel>().HasData(new
        {
            Id = "ttkab9fe-po57-4088-82a9-d27008688bbb",
            Name = "Floor",
            LanguageCode = "en",
            LevelId = "oo10e768-3e06-po02-b337-ee367a82admn",
            DisplayOrder = 3
        });
        builder.Entity<PriceCalculatorTaxonomyLevel>().HasData(new
        {
            Id = "eew9e479-pob3-40c6-ad61-e40dbe6a5111",
            Name = "Floor nl",
            LanguageCode = "nl",
            LevelId = "oo10e768-3e06-po02-b337-ee367a82admn",
            DisplayOrder = 3
        });

        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "0c35b890-94f6-48c9-8010-921a48b6ba04",
            LocaleCode = "Truck",
            ParentId = "0c355800-91fd-4d99-8010-921a42f0ba04",
            DisplayOrder = 0,
            Title = "Truck"
        });
        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "0c312c90-94f6-48c9-8410-921a43c2aa04",
            LocaleCode = "Pump",
            ParentId = "0c355800-91fd-4d99-8010-921a42f0ba04",
            DisplayOrder = 0,
            Title = "Pump"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "0c3b6870-94f6-48c9-8c40-921a58b6b4c4",
            Label = "Truck",
            LanguageCode = "En",
            CpcResourceFamilyId = "0c35b890-94f6-48c9-8010-921a48b6ba04",
            ParentId = "0c355800-91fd-4d99-8010-921a42f0ba04"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "1b3b66a0-94f6-48c9-8c40-921ac786b4c4",
            Label = "Pump",
            LanguageCode = "En",
            CpcResourceFamilyId = "0c312c90-94f6-48c9-8410-921a43c2aa04",
            ParentId = "0c355800-91fd-4d99vi-8010-921a42f0ba04"
        });

        builder.Entity<CpcResourceFamily>().HasData(new
        {
            Id = "hjkl2c90-94f6-pump-8410-921a43c2lkjl",
            LocaleCode = "Pump",
            ParentId = "0c355800-91fd-4d99-8010-921a42f0ba04",
            DisplayOrder = 0,
            Title = "Pump"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "nddd66a0-94f6-48c9-8c40-921ac786llll",
            Label = "Pump",
            LanguageCode = "en",
            CpcResourceFamilyId = "hjkl2c90-94f6-pump-8410-921a43c2lkjl"
        });
        builder.Entity<CpcResourceFamilyLocalizedData>().HasData(new
        {
            Id = "sssa66a0-94f6-48c9-8c40-921ac786nvvvv",
            Label = "Pump(nl)",
            LanguageCode = "nl",
            CpcResourceFamilyId = "hjkl2c90-94f6-pump-8410-921a43c2lkjl"
        });
        
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "nvfjjsjhhcd5a-0941-nsub-9c13-Lota2ab276",
            Name = "Not Subscribed",
            LanguageCode = "en",
            StatusId = "xxxxad0b-2e84-nsub-ad25-Lot0d49477",
            DisplayOrder = 16
        });
        builder.Entity<ConstructorWorkFlowStatus>().HasData(new
        {
            Id = "ndjjd3-jsjj-nnnn-nsub-b7a93ebd1iii",
            Name = "Not Subscribed nl",
            LanguageCode = "nl",
            StatusId = "xxxxad0b-2e84-nsub-ad25-Lot0d49477",
            DisplayOrder = 16
        });

    }

}