using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Shared.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.GL;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.ProjectClassification;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Data.Translations;

namespace UPrinceV4.Web.Data;

public class ApplicationDbContext : IdentityDbContext, IMultitenantDbContext
{
    private readonly Tenant _tenant;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenantProvider) :
        base(options)
    {
        _tenant = tenantProvider.GetTenant();
    }


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
    public DbSet<ProjectTeam> ProjectTeam { get; set; }
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
    public DbSet<CpcBrand> CpcBrand { get; set; }
    public DbSet<PbsInstructionFamily> PbsInstructionFamily { get; set; }
    public DbSet<PbsInstructionLink> PbsInstructionLink { get; set; }
    public DbSet<PbsToleranceState> PbsToleranceState { get; set; }
    public DbSet<PbsProductTaxonomy> PbsProductTaxonomy { get; set; }
    public DbSet<PbsTaxonomy> PbsTaxonomy { get; set; }
    public DbSet<PbsRisk> PbsRisk { get; set; }
    public DbSet<PbsQuality> PbsQuality { get; set; }
    public DbSet<PbsHistoryLog> PbsHistoryLog { get; set; }
    public DbSet<QualityHistoryLog> QualityHistoryLog { get; set; }
    public DbSet<RiskHistoryLog> RiskHistoryLog { get; set; }
    public DbSet<PbsTaxonomyLevel> PbsTaxonomyLevel { get; set; }
    public DbSet<PbsQualityResponsibility> PbsQualityResponsibility { get; set; }
    public DbSet<ProjectToleranceStateLocalizedData> ProjectToleranceStateLocalizedData { get; set; }
    public DbSet<ProjectStateLocalizedData> ProjectStateLocalizedData { get; set; }
    public DbSet<ProjectTypeLocalizedData> ProjectTypeLocalizedData { get; set; }
    public DbSet<ProjectManagementLevelLocalizedData> ProjectManagementLevelLocalizedData { get; set; }
    public DbSet<ProjectTemplateLocalizedData> ProjectTemplateLocalizedData { get; set; }
    public DbSet<AllProjectAttributes> AllProjectAttributes { get; set; }
    public DbSet<BorConsumable> BorConsumable { get; set; }
    public DbSet<BorLabour> BorLabour { get; set; }
    public DbSet<BorMaterial> BorMaterial { get; set; }
    public DbSet<Bor> Bor { get; set; }
    public DbSet<BorTools> BorTools { get; set; }
    public DbSet<BorRequiredConsumable> BorRequiredConsumable { get; set; }
    public DbSet<BorRequiredTools> BorRequiredTools { get; set; }
    public DbSet<BorRequiredLabour> BorRequiredLabour { get; set; }
    public DbSet<BorRequiredMaterial> BorRequiredMaterial { get; set; }
    public DbSet<CpcResourceTypeLocalizedData> CpcResourceTypeLocalizedData { get; set; }
    public DbSet<TimeClockActivityTypeLocalizedData> TimeClockActivityTypeLocalizedData { get; set; }
    public DbSet<ProjectDefinitionDatabaseName> ProjectDefinitionDatabaseName { get; set; }
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
    public DbSet<PbsTaxonomyLevelLocalizedData> PbsTaxonomyLevelLocalizedData { get; set; }
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
    public DbSet<QrHistoryLog> QrHistoryLog { get; set; }

    public DbSet<WebTranslation> WebTranslation { get; set; }
    public DbSet<Language> Language { get; set; }

    public DbSet<GeneralLedgerNumber> GenaralLederNumber { get; set; }

    public DbSet<POType> POType { get; set; }
    public DbSet<POStatus> POStatus { get; set; }
    public DbSet<ProjectClassificationHeader> ProjectClassification { get; set; }


    public object CpcBasicUnitOfMeasureLocalizedData { get; internal set; }


    public int TenantId => _tenant.Id;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_tenant.DatabaseType == 1)
        {
            optionsBuilder.UseSqlServer(_tenant.ConnectionString);
        }
        else if (_tenant.DatabaseType == 2)
        {
            // optionsBuilder.UseMySql(_tenant.ConnectionString);
        }
        // optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //var navigation = builder.Entity<ProductCategory>()
        //                        .Metadata
        //                        .FindNavigation(nameof(ProductCategory.Products));

        //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        // builder.Entity<UserRole>().HasKey(r => new { r.RoleId, r.ApplicationUserOid });

        base.OnModelCreating(builder);
    }
}