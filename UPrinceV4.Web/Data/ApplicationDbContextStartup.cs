using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.GL;
using UPrinceV4.Web.Data.INV;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Data.TAX;
using UPrinceV4.Web.Data.Translations;

namespace UPrinceV4.Web.Data;

public class ApplicationDbContextStartUp : DbContext
{
    public ApplicationDbContextStartUp(DbContextOptions<UPrinceCustomerContex> options,
        IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }


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
            Id = "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14", Name = "External Products",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14"
        });
        builder.Entity<PbsProductItemType>().HasData(new
        {
            Id = "aa0c8e3c-f716-4f92-afee-851d485164da", Name = "Internal Products",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da"
        });

        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "d60aad0b-2e84-482b-ad25-618d80d49477", Name = "Pending Development",
            LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "94282458-0b40-40a3-b0f9-c2e40344c8f1", Name = "In Development",
            LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "7143ff01-d173-4a20-8c17-cacdfecdb84c", Name = "In Review",
            LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", Name = "Approved",
            LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<PbsProductStatus>().HasData(new
        {
            Id = "4010e768-3e06-4702-b337-ee367a82addb", Name = "Handed Over",
            LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
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
            Id = "004eb795-8bba-47e8-9049-d14774ab0b18", Name = "Within Tolerance (green)",
            LocaleCode = "ProjectToleranceState.csvWithin Tolerance (green)"
        });
        builder.Entity<PbsToleranceState>().HasData(new
        {
            Id = "8f33bdf6-7600-4ad7-b558-c98899c1e5b2", Name = "Out of Tolerance (red)",
            LocaleCode = "ProjectToleranceState.csvOut of Tolerance (red)"
        });
        builder.Entity<PbsToleranceState>().HasData(new
        {
            Id = "d9712fb3-02b6-4c2a-991c-ee904c87d8a8", Name = "Tolerance limit (orange)",
            LocaleCode = "ProjectToleranceState.csvTolerance limit (orange)"
        });

        builder.Entity<ErrorMessage>().HasData(new
        {
            Id = "NoProjectBreakdownStructureAvailable", Message = "No available project breakdown structure",
            LocaleCode = "NoProjectBreakdownStructureAvailable"
        });
        builder.Entity<ErrorMessage>().HasData(new
            { Id = "NoPmolAvailable", Message = "No available Pmol", LocaleCode = "NoPmolAvailable" });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1315, LanguageCode = "es", LocaleCode = "NoProjectBreakdownStructureAvailable12",
            Label = "No available project breakdown structure (es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1316, LanguageCode = "nl", LocaleCode = "NoProjectBreakdownStructureAvailable",
            Label = "No available project breakdown structure (nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1317, LanguageCode = "nl-BE", LocaleCode = "NoProjectBreakdownStructureAvailable",
            Label = "No available project breakdown structure (nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1632, LanguageCode = "zh", LocaleCode = "NoProjectBreakdownStructureAvailable",
            Label = "No available project breakdown structure (zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1633, LanguageCode = "es", LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1634, LanguageCode = "nl", LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1635, LanguageCode = "nl-BE",
            LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1636, LanguageCode = "zh", LocaleCode = "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Label = "External Products(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1637, LanguageCode = "es", LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1638, LanguageCode = "nl", LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1639, LanguageCode = "nl-BE",
            LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1640, LanguageCode = "zh", LocaleCode = "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da",
            Label = "Internal Products(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1641, LanguageCode = "es", LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1643, LanguageCode = "nl", LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1644, LanguageCode = "nl-BE", LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1645, LanguageCode = "zh", LocaleCode = "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477",
            Label = "Pending Development(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1646, LanguageCode = "es", LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1647, LanguageCode = "nl", LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1648, LanguageCode = "nl-BE", LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1649, LanguageCode = "zh", LocaleCode = "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1",
            Label = "In Development(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1650, LanguageCode = "es", LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1651, LanguageCode = "nl", LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1652, LanguageCode = "nl-BE", LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1653, LanguageCode = "zh", LocaleCode = "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c",
            Label = "In Review(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1654, LanguageCode = "es", LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1655, LanguageCode = "nl", LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1656, LanguageCode = "nl-BE", LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1657, LanguageCode = "zh", LocaleCode = "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
            Label = "Approved(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1658, LanguageCode = "es", LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(es)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1659, LanguageCode = "nl", LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1660, LanguageCode = "nl-BE", LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
            Label = "Handed Over(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1661, LanguageCode = "zh", LocaleCode = "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb",
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
            Id = "3417e806-8e97-46d3-adb6-34426cd93bf4", Name = "Intermediate",
            LocaleCode = "PbsExperienceIntermediate"
        });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "b08b0641-e260-4750-8141-3cd8c25f6344", Name = "Skilful", LocaleCode = "PbsExperienceSkilful" });
        builder.Entity<PbsExperience>().HasData(new
            { Id = "ea27ee00-8b38-48b6-8cc7-6872dc3cf09c", Name = "Seasoned", LocaleCode = "PbsExperienceSeasoned" });
        builder.Entity<PbsExperience>().HasData(new
        {
            Id = "df186961-6453-4c42-af53-c8866684a075", Name = "Proficient", LocaleCode = "PbsExperienceProficient"
        });
        builder.Entity<PbsExperience>().HasData(new
        {
            Id = "ee146eff-0f1f-44b1-a6ba-73b267416973", Name = "Experienced",
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
            Id = "8b145fdc-b666-488c-beec-f335627024601", Name = "Team Building Skills",
            LocaleCode = "PbsSkillTeam Building Skills", ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "bfd3f176-cc91-4d01-b27f-bef8888fc21c1", Name = "Collaboration",
            LocaleCode = "PbsSkillCollaboration", ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "0ffe382d-fe7d-4ac7-91b3-204570427c371", Name = "Communication",
            LocaleCode = "PbsSkillCommunication", ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "8f992d6e-7fee-43a3-b06c-430fa4d9d8e41", Name = "Flexibility", LocaleCode = "PbsSkillFlexibility",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "1ae3028d-ab5b-4d88-bf4a-5bf53d969c4d1", Name = "Listening", LocaleCode = "PbsSkillListening",
            ParentId = "8b145fdc-b666-488c-beec-f335627024601"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1", Name = "Analytical Skills",
            LocaleCode = "PbsSkillAnalytical Skills", ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "7fd2a1b0-c559-4727-a596-dbc0af7c171e1", Name = "Critical thinking",
            LocaleCode = "PbsSkillCritical thinking", ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "a1e3c91b-a8cf-43b1-b551-8bba9f64c3351", Name = "Data analysis",
            LocaleCode = "PbsSkillData analysis", ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "4a2cb5e5-e9ab-47a6-b1c5-080bdc5d60b61", Name = "Numeracy", LocaleCode = "PbsSkillNumeracy",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });
        builder.Entity<PbsSkill>().HasData(new
        {
            Id = "74e9f3ce-5338-467c-add0-ba7116cd300b1", Name = "Reporting", LocaleCode = "PbsSkillReporting",
            ParentId = "fb88dff8-cf84-4cdb-acae-4a8b9241178f1"
        });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1676, LanguageCode = "nl", LocaleCode = "PbsSkillTeam Building Skills",
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
            Id = 1681, LanguageCode = "nl", LocaleCode = "PbsSkillAnalytical Skills",
            Label = "Analytical Skills(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1682, LanguageCode = "nl", LocaleCode = "PbsSkillCritical thinking",
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
            Id = "26f71a21-b062-4fc8-b47a-f50038e71fe1", Family = "Family 01",
            LocaleCode = "PbsInstructionFamilyTechnical instructions"
        });
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "fc925493-c443-437d-a367-b88e81941aaa", Family = "Family 02",
            LocaleCode = "PbsInstructionFamilySafety instructions"
        });
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "48ec5849-1daf-425c-8fcf-fb0dd9748b8c", Family = "Family 03",
            LocaleCode = "PbsInstructionFamilyEnvironmental instructions"
        });
        builder.Entity<PbsInstructionFamily>().HasData(new
        {
            Id = "e286e905-c157-4d19-ac7c-55550df0ee9a", Family = "Family 04",
            LocaleCode = "PbsInstructionFamilyHealth instructions"
        });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1686, LanguageCode = "nl", LocaleCode = "PbsInstructionFamilyTechnical instructions",
            Label = "Family 01(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1687, LanguageCode = "nl", LocaleCode = "PbsInstructionFamilySafety instructions",
            Label = "Family 02(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1688, LanguageCode = "nl", LocaleCode = "PbsInstructionFamilyEnvironmental instructions",
            Label = "Family 03(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1689, LanguageCode = "nl", LocaleCode = "PbsInstructionFamilyHealth instructions",
            Label = "Family 04(nl)"
        });

        // Pbs Risk dropdown data
        builder.Entity<RiskType>().HasData(new { Id = "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210", Type = "Threat" });
        builder.Entity<RiskType>().HasData(new { Id = "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0", Type = "Opportunity" });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1700, LanguageCode = "nl", LocaleCode = "4dba0e61-15f8-47a9-8fcd-0ced2e2ce210",
            Label = "Threat(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1701, LanguageCode = "nl", LocaleCode = "ac9f4655-f14c-43c7-8e4a-5390bfdc16d0",
            Label = "Opportunity(nl)"
        });

        builder.Entity<RiskStatus>().HasData(new { Id = "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae", Status = "Active" });
        builder.Entity<RiskStatus>().HasData(new { Id = "8b0d0513-6111-466f-86c8-b26278c3c4f7", Status = "Closed" });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1702, LanguageCode = "nl", LocaleCode = "00b0a1c6-e5c8-4143-90f1-7dec0b0397ae",
            Label = "Active(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1703, LanguageCode = "nl", LocaleCode = "8b0d0513-6111-466f-86c8-b26278c3c4f7",
            Label = "Closed(nl)"
        });

        //Pbs Taxonomy Level data
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "f0d64941-145a-4a8a-8619-165c965a16eb", Type = "Location", Name = "Product", Order = 100,
            LocaleCode = "PbsTaxonomyLevel.Location.Product"
        });
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "077845b7-79a7-4883-a02d-6094fc6d6563", Type = "Location", Name = "Separation", Order = 200,
            LocaleCode = "PbsTaxonomyLevel.Location.Separation"
        });
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "8bb27024-ce91-4406-8e48-db08286f0b4b", Type = "Utility", Name = "Product", Order = 100,
            LocaleCode = "PbsTaxonomyLevel.Location.Product"
        });
        builder.Entity<PbsTaxonomyLevel>().HasData(new
        {
            Id = "cd8418c0-502e-4893-b387-1426a5edd3a4", Type = "Utility", Name = "Traject Part", Order = 200,
            LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart"
        });

        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1710, LanguageCode = "nl", LocaleCode = "PbsTaxonomyLevel.Location.Product", Label = "Product(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1711, LanguageCode = "nl-BE", LocaleCode = "PbsTaxonomyLevel.Location.Product",
            Label = "Product(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1712, LanguageCode = "zh", LocaleCode = "PbsTaxonomyLevel.Location.Product", Label = "Product(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1713, LanguageCode = "nl", LocaleCode = "PbsTaxonomyLevel.Location.Separation",
            Label = "Separation(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1714, LanguageCode = "nl-BE", LocaleCode = "PbsTaxonomyLevel.Location.Separation",
            Label = "Separation(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1715, LanguageCode = "zh", LocaleCode = "PbsTaxonomyLevel.Location.Separation",
            Label = "Separation(zh)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1716, LanguageCode = "nl", LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart",
            Label = "Traject Part(nl)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1717, LanguageCode = "nl-BE", LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart",
            Label = "Traject Part(nl-BE)"
        });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1718, LanguageCode = "zh", LocaleCode = "PbsTaxonomyLevel.Utility.TrajectPart",
            Label = "Traject Part()"
        });

        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb", Name = "in voorbereiding", LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", Name = "In Development", LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276", Name = "Pending Development", LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf", Name = "goedgekeurd", LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed", Name = "ter goedkeuring", LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a", Name = "Approved", LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af", Name = "In Review", LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", Name = "Handed Over", LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae", Name = "in uitvoering", LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<PmolStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", Name = "afgewerkt en doorgegeven", LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
        });

        builder.Entity<PmolType>().HasData(new
        {
            Id = "015bb656-f708-4a0d-9973-3d834ffe757d", Name = "Work", LanguageCode = "en",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01", DisplayOrder = 5, Type = 1
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "03f7c556-2d73-4283-8fc3-634233943bb9", Name = "Werk", LanguageCode = "nl",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01", DisplayOrder = 5, Type = 1
        });
        //builder.Entity<PmolType>().HasData(new { Id = "17e4fc8f-2531-4c24-a289-e3360d8e481f", Name = "Personal", LanguageCode = "en", TypeId = "e4fc8f-2531-4c24-a289-e3360d8e481f17", DisplayOrder = 5 });
        //builder.Entity<PmolType>().HasData(new { Id = "278a6814-2097-4f7b-9ebf-f17e5416911b", Name = "persoonlijk", LanguageCode = "nl", TypeId = "e4fc8f-2531-4c24-a289-e3360d8e481f17", DisplayOrder = 5 });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf", Name = "Travel", LanguageCode = "en",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1", DisplayOrder = 5, Type = 0
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "c80b2d63-f3d0-4cd4-8353-5d7a089dba98", Name = "Verplaatsen", LanguageCode = "nl",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1", DisplayOrder = 5, Type = 0
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "f3d04255-1cc1-4cdc-b8a7-5423972a3dda", Name = "(Un)load", LanguageCode = "en",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff", DisplayOrder = 5, Type = 2
        });
        builder.Entity<PmolType>().HasData(new
        {
            Id = "ff848e5e-622d-4783-95e6-4092004eb5ea", Name = "Laden en lossen", LanguageCode = "nl",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff", DisplayOrder = 5, Type = 2
        });

        builder.Entity<Roles>().HasData(new
            { Id = "5e622d-4783-95e6-4092004eb5e-aff848e", TenantId = 1, RoleName = "Welder" });
        builder.Entity<Roles>().HasData(new
        {
            Id = "18d10b25-57ea-4f4e-9e76-56df09554a95", TenantId = 1, RoleName = "Lasser", LanguageCode = "nl",
            RoleId = "5e622d-4783-95e6-4092004eb5e-aff848e"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "be432c54-5860-4f56-832d-ceb3435d8b7e", TenantId = 1, RoleName = "Administrator",
            LanguageCode = "nl", RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "476127cb-62db-4af7-ac8e-d4a722f8e142", TenantId = 1, RoleName = "Project Manager",
            LanguageCode = "nl", RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "245d23fe-4864-4cc5-b53b-0a3b3843f0e1", TenantId = 1, RoleName = "Gebruiker", LanguageCode = "nl",
            RoleId = "4837043c-119c-47e1-bbf2-1f32557fdf30"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "1666e217-2b80-4acd-b48b-b041fe263fb9", TenantId = 1, RoleName = "Project Eigenaar",
            LanguageCode = "nl", RoleId = "6f56c794-7f88-48a7-9aba-a3f95f940be4"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "3178903c-bf36-40f7-b870-724e238684ff", TenantId = 1, RoleName = "Project Ingenieur",
            LanguageCode = "nl", RoleId = "f9a0cee5-f09a-44a5-93e8-d78f84bbcbf3"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "907b7af0-b132-4951-a2dc-6ab82d4cd40d", TenantId = 1, RoleName = "Customer", LanguageCode = "en",
            RoleId = "907b7af0-b132-4951-a2dc-6ab82d4cd40d"
        });
        builder.Entity<Roles>().HasData(new
        {
            Id = "78b84ad9-6757-405a-9729-5d2af8615e07", TenantId = 1, RoleName = "Klant", LanguageCode = "nl",
            RoleId = "907b7af0-b132-4951-a2dc-6ab82d4cd40d"
        });
        builder.Entity<WorkflowState>().HasData(new
            { Id = 4, State = "Handed Over", LocaleCode = "WorkflowState.csvHandedOver", IsDeleted = false });
        builder.Entity<LocalizedData>().HasData(new
        {
            Id = 1895, LanguageCode = "nl", LocaleCode = "WorkflowState.csvHandedOver", Label = "Handed Over(nl)"
        });

        builder.Entity<PsStatus>().HasData(new
        {
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb", Name = "in voorbereiding", LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", Name = "In Development", LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276", Name = "Pending Development", LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf", Name = "goedgekeurd", LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed", Name = "ter goedkeuring", LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a", Name = "Approved", LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af", Name = "In Review", LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", Name = "Handed Over", LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae", Name = "in uitvoering", LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<PsStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", Name = "afgewerkt en doorgegeven", LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
        });

        builder.Entity<PsType>().HasData(new
        {
            Id = "015bb656-f708-4a0d-9973-3d834ffe757d", Name = "Work", LanguageCode = "en",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01", DisplayOrder = 5, Type = 1
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "03f7c556-2d73-4283-8fc3-634233943bb9", Name = "Werk", LanguageCode = "nl",
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01", DisplayOrder = 5, Type = 1
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "9d13f8ce-f268-4ce3-9f12-fa6b3adad2cf", Name = "Travel", LanguageCode = "en",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1", DisplayOrder = 5, Type = 0
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "c80b2d63-f3d0-4cd4-8353-5d7a089dba98", Name = "Verplaatsen", LanguageCode = "nl",
            TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1", DisplayOrder = 5, Type = 0
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "f3d04255-1cc1-4cdc-b8a7-5423972a3dda", Name = "(Un)load", LanguageCode = "en",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff", DisplayOrder = 5, Type = 2
        });
        builder.Entity<PsType>().HasData(new
        {
            Id = "ff848e5e-622d-4783-95e6-4092004eb5ea", Name = "Laden en lossen", LanguageCode = "nl",
            TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff", DisplayOrder = 5, Type = 2
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
            Id = "098cf409-7cb8-4076-8ddf-657dd897f5bb", Name = "in voorbereiding", LanguageCode = "nl",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", Name = "In Development", LanguageCode = "en",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "2732cd5a-0941-4c56-9c13-f5fdca2ab276", Name = "Pending Development", LanguageCode = "en",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", DisplayOrder = 1
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "4e01a893-0267-48af-b65a-b7a93ebd1ccf", Name = "goedgekeurd", LanguageCode = "nl",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "5015743d-a2e6-4531-808d-d4e1400e1eed", Name = "ter goedkeuring", LanguageCode = "nl",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "77143c60-ff45-4ca2-8723-213d2d1c5f5a", Name = "Approved", LanguageCode = "en",
            StatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", DisplayOrder = 4
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "813a0c70-b58f-433d-8945-9cb166ae42af", Name = "In Review", LanguageCode = "en",
            StatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c", DisplayOrder = 3
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", Name = "Handed Over", LanguageCode = "en",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "a35ab9fe-df57-4088-82a9-d27008688bae", Name = "in uitvoering", LanguageCode = "nl",
            StatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", DisplayOrder = 2
        });
        builder.Entity<InvoiceStatus>().HasData(new
        {
            Id = "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", Name = "afgewerkt en doorgegeven", LanguageCode = "nl",
            StatusId = "4010e768-3e06-4702-b337-ee367a82addb", DisplayOrder = 5
        });

        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-bqff-2d63c756688f24", Name = "0%", Order = 0, IsDefault = false });
        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-baff-2d63c756688f25", Name = "6%", Order = 1, IsDefault = false });
        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-bzff-2d63c756688f26", Name = "12%", Order = 2, IsDefault = false });
        builder.Entity<Tax>().HasData(new
            { Id = "4ab98714-4087-45d4-bzff-2d63c756688f27", Name = "21%", Order = 3, IsDefault = true });
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server = tcp:uprincev4uatdb.database.windows.net, 1433; Initial Catalog =P0002; Persist Security Info = False; User ID = uprincedbuser; Password = UPrince2017!; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; MultipleActiveResultSets = true;");
    }
}