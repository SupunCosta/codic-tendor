using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data.CIAW;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.DossierData;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.Issue;
using UPrinceV4.Web.Data.Mecops;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Data.WBS;

namespace UPrinceV4.Web.Data;

public class UprinceV5Context : DbContext
{
    public DbSet<CiawFeatchStatus> CiawFeatchStatus { get; set; }
    public DbSet<CiawRemark> CiawRemark { get; set; }
    public DbSet<PbsDisplayOrder> PbsDisplayOrder { get; set; }
    public DbSet<ThTruckAvailability> ThTruckAvailability { get; set; }
    public DbSet<CpcSerialNumber> CpcSerialNumber { get; set; }
    public DbSet<ThColors> ThColors { get; set; }
    public DbSet<HRContractorList> HRContractorList { get; set; }
    public DbSet<HRContractTypes> HRContractTypes { get; set; }
    public DbSet<WbsTaxonomy> WbsTaxonomy { get; set; }
    public DbSet<WbsTaxonomyLevel> WbsTaxonomyLevels { get; set; }
    public DbSet<WbsTemplate> WbsTemplate { get; set; }
    public DbSet<ContractorsTotalPriceErrors> ContractorsTotalPriceErrors { get; set; }
    public DbSet<WbsTask> WbsTask { get; set; }
    public DbSet<WbsTaskTags> WbsTaskTags { get; set; }
    public DbSet<WbsTaskDocuments> WbsTaskDocuments { get; set; }
    public DbSet<WbsTaskCc> WbsTaskCc { get; set; }
    public DbSet<PbsAssignedLabour> PbsAssignedLabour { get; set; }
    public DbSet<WbsTaskEmail> WbsTaskEmail { get; set; }
    public DbSet<WbsTaskStatus> WbsTaskStatus { get; set; }
    public DbSet<WbsTaskDeliveryStatus> WbsTaskDeliveryStatus { get; set; }
    public DbSet<WbsTaskInstruction> WbsTaskInstruction { get; set; }
    public DbSet<WbsTaskTo> WbsTaskTo { get; set; }
    public DbSet<WbsProduct> WbsProduct { get; set; }
    public DbSet<WbsProductCc> WbsProductCc { get; set; }
    public DbSet<WbsProductTo> WbsProductTo { get; set; }
    public DbSet<WbsProductTags> WbsProductTags { get; set; }
    public DbSet<WbsProductDocuments> WbsProductDocuments { get; set; }
    public DbSet<Dossier> Dossier { get; set; }
    public DbSet<DossierProjects> DossierProjects { get; set; }
    public DbSet<Mecop> CustomerOrderToPo { get; set; }
    public DbSet<MecopConversion> CustomerOrderToPoConversion { get; set; }
    public DbSet<MecopMetaData> CustomerOrderToPoMetaData { get; set; }
    public DbSet<WbsConversation> WbsConversation { get; set; }
    public DbSet<WbsConversationCc> WbsConversationCc { get; set; }
    public DbSet<WbsConversationTo> WbsConversationTo { get; set; }
    public DbSet<WbsConversationAttachments> WbsConversationAttachments { get; set; }
    public DbSet<WbsProductEmai> WbsProductEmai { get; set; }
    public DbSet<IssueHeader> IssueHeader { get; set; }
    public DbSet<IssuePriority> IssuePriority { get; set; }
    public DbSet<IssueSeverity> IssueSeverity { get; set; }
    public DbSet<IssueStatus> IssueStatus { get; set; }
    public DbSet<IssueTags> IssueTags { get; set; }
    public DbSet<IssueType> IssueType { get; set; }
    public DbSet<WbsDocument> WbsDocument { get; set; }
    public DbSet<IssueDocument> IssueDocument { get; set; }
    public DbSet<IssueCc> IssueCc { get; set; }
    public DbSet<IssueTo> IssueTo { get; set; }
    public DbSet<PbsCbcResources> PbsCbcResources { get; set; }



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
        builder.Entity<CiawFeatchStatus>().HasData(new
        {
            Id = "1",
            Status = false
        });

        builder.Entity<ThColors>().HasData(new
        {
            Id = "1",
            Code = "#c56834",
            Font = "#fffffff"
        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "2",
            Code = "#c4d8e5",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "3",
            Code = "#97c8ea",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "4",
            Code = "#3b9b36",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "5",
            Code = "#a5982c",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "6",
            Code = "#97ac0f",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "7",
            Code = "#b13748",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "8",
            Code = "#ea716d",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "9",
            Code = "#166fdb",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "10",
            Code = "#f69b68",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "11",
            Code = "#b3da90",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "12",
            Code = "#b65ab3",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "13",
            Code = "#08272B",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "14",
            Code = "#19D65B",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "15",
            Code = "#1C6675",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "16",
            Code = "#AE9675",
            Font = "#fffffff"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "17",
            Code = "#8C2581",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "18",
            Code = "#7EF6CE",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "19",
            Code = "#1C85CF",
            Font = "#000000"

        });
        builder.Entity<ThColors>().HasData(new
        {
            Id = "20",
            Code = "#0F264F",
            Font = "#fffffff"

        });

        builder.Entity<HRContractTypes>().HasData(new
        {
            Id = "005b42ba-574d-4afd-a034-347858e53c9d",
            Name = "Tempory",
            TypeId = "12a22319-8ca7-temp-b588-6fab99474130",
            LanguageCode = "en"

        });
        builder.Entity<HRContractTypes>().HasData(new
        {
            Id = "1377a17d-3f18-46c1-bc7c-c11edcf65b5c",
            Name = "Tempory(nl)",
            TypeId = "12a22319-8ca7-temp-b588-6fab99474130",
            LanguageCode = "nl"

        });
        builder.Entity<HRContractTypes>().HasData(new
        {
            Id = "222e3dab-576d-4f53-b976-a9b5c97ee165",
            Name = "Permenant",
            TypeId = "41ce52c0-058b-perm-afbd-1d2d24105ebc",
            LanguageCode = "en"

        });
        builder.Entity<HRContractTypes>().HasData(new
        {
            Id = "3263aa4e-12a8-4c59-bc99-d561a603748e",
            Name = "Permenant(nl)",
            TypeId = "41ce52c0-058b-perm-afbd-1d2d24105ebc",
            LanguageCode = "nl"

        });
        
        builder.Entity<WbsTaxonomyLevel>().HasData(new
        {
            Id = "3263aa4e-12a8-wbs-bc99-d561a603748e",
            Name = "WBS",
            LevelId = "41ce52c0-058b-wbs-afbd-1d2d24105ebc",
            LanguageCode = "en",
            DisplayOrder = "1"
        });
        builder.Entity<WbsTaxonomyLevel>().HasData(new
        {
            Id = "p263aa4e-12a8-prod-bc99-d561a603748e",
            Name = "Product",
            LevelId = "e1ce52c0-058b-prod-afbd-1d2d24105ebc",
            LanguageCode = "en",
            DisplayOrder = "2"
        });
        builder.Entity<WbsTaxonomyLevel>().HasData(new
        {
            Id = "d263aa4e-12a8-issu-bc99-d561a603748e",
            Name = "Task",
            LevelId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc",
            LanguageCode = "en",
            DisplayOrder = "3"
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
            Name = "ZIP",
            TypeId = "2210e768-msms-po02-Lot5-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 5
        });
        builder.Entity<ContractorFileType>().HasData(new
        {
            Id = "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
            Name = "ZIP(nl)",
            TypeId = "2210e768-msms-po02-Lot5-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 5
        });

        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "d263aa4e-12a8-issu-bc99-d561a603748e",
            Name = "Overdue",
            StatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc",
            LanguageCode = "en",
            DisplayOrder = 0
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
            Name = "Within 7 Days",
            StatusId = "2210e768-msms-po02-Lot5-ee367a82ad22",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
            Name = "More than 7 days",
            StatusId = "12a22319-8ca7-temp-b588-6fab99474130",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "mcbaa4e-12a8-issu-bc99-d561a603748e",
            Name = "By Today",
            StatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc",
            LanguageCode = "en",
            DisplayOrder = 3
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "lsj3aa4e-12a8-issu-bc99-d561a603748e",
            Name = "Overdue(nl)",
            StatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc",
            LanguageCode = "nl",
            DisplayOrder = 0
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "poie479-msms-40c6-Lot5-e40dbe6a5wer",
            Name = "Within 7 Days(nl)",
            StatusId = "2210e768-msms-po02-Lot5-ee367a82ad22",
            LanguageCode = "nl",
            DisplayOrder = 1
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "bcfe479-msms-4ZIP-Lot5-e40dbe6a5wer",
            Name = "More than 7 days(nl)",
            StatusId = "12a22319-8ca7-temp-b588-6fab99474130",
            LanguageCode = "nl",
            DisplayOrder = 2
        });
        builder.Entity<WbsTaskDeliveryStatus>().HasData(new
        {
            Id = "iuefd4e-12a8-issu-bc99-d561a603748e",
            Name = "By Today(nl)",
            StatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc",
            LanguageCode = "nl",
            DisplayOrder = 3
        });

        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "d263aa4e-12a8-issu-bc99-d561a603748e",
            Name = "Pending Development",
            StatusId = "0e1b34bc-f2c3-4778-8250-9666ee96ae59",
            LanguageCode = "en",
            DisplayOrder = 0
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
            Name = "In Development",
            StatusId = "3960193f-99e0-43c6-a6cc-4919e5d345c5",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
            Name = "In Review",
            StatusId = "40843898-54EE-473D-A661-194F1DA0CE48",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "273D6023-0643-4F16-8605-652AF0B658A2",
            Name = "Approved",
            StatusId = "5684969c-f3e8-49ac-9746-51e7e23f2782",
            LanguageCode = "en",
            DisplayOrder = 3
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "ithhf023-0643-4F16-8605-652AF0B658A2",
            Name = "Handed Over",
            StatusId = "vvvv969c-f3e8-49ac-9746-51e7e23f2782",
            LanguageCode = "en",
            DisplayOrder = 4
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "ehwraa4e-12a8-issu-bc99-d561a603748e",
            Name = "in voorbereiding",
            StatusId = "0e1b34bc-f2c3-4778-8250-9666ee96ae59",
            LanguageCode = "nl",
            DisplayOrder = 0
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "wfede479-msms-40c6-Lot5-e40dbe6a5wer",
            Name = "in uitvoering",
            StatusId = "3960193f-99e0-43c6-a6cc-4919e5d345c5",
            LanguageCode = "nl",
            DisplayOrder = 1
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "lksdm479-msms-4ZIP-Lot5-e40dbe6a5wer",
            Name = "ter goedkeuring",
            StatusId = "40843898-54EE-473D-A661-194F1DA0CE48",
            LanguageCode = "nl",
            DisplayOrder = 2
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "wecv6023-0643-4F16-8605-652AF0B658A2",
            Name = "goedgekeurd",
            StatusId = "5684969c-f3e8-49ac-9746-51e7e23f2782",
            LanguageCode = "nl",
            DisplayOrder = 3
        });
        builder.Entity<WbsTaskStatus>().HasData(new
        {
            Id = "qwsdd023-0643-4F16-8605-652AF0B658A2",
            Name = "afgewerkt en doorgegeven",
            StatusId = "vvvv969c-f3e8-49ac-9746-51e7e23f2782",
            LanguageCode = "nl",
            DisplayOrder = 4
        });

        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "d263aa4e-12a8-isis-bc99-d561a603748e",
            Name = "Pending Development",
            StatusId = "0e1b34bc-f2c3-isis-8250-9666ee96ae59",
            LanguageCode = "en",
            DisplayOrder = 0
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "wer9e479-msms-isis-Lot5-e40dbe6a5wer",
            Name = "In Development",
            StatusId = "3960193f-99e0-isis-a6cc-4919e5d345c5",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "xer9e479-msms-isis-Lot5-e40dbe6a5wer",
            Name = "In Review",
            StatusId = "40843898-54EE-isis-A661-194F1DA0CE48",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "273D6023-0643-isis-8605-652AF0B658A2",
            Name = "Approved",
            StatusId = "5684969c-f3e8-isis-9746-51e7e23f2782",
            LanguageCode = "en",
            DisplayOrder = 3
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "ithhf023-0643-isis-8605-652AF0B658A2",
            Name = "Handed Over",
            StatusId = "vvvv969c-f3e8-isis-9746-51e7e23f2782",
            LanguageCode = "en",
            DisplayOrder = 4
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "ehwraa4e-12a8-isis-bc99-d561a603748e",
            Name = "in voorbereiding",
            StatusId = "0e1b34bc-f2c3-isis-8250-9666ee96ae59",
            LanguageCode = "nl",
            DisplayOrder = 0
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "wfede479-msms-isis-Lot5-e40dbe6a5wer",
            Name = "in uitvoering",
            StatusId = "3960193f-99e0-isis-a6cc-4919e5d345c5",
            LanguageCode = "nl",
            DisplayOrder = 1
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "lksdm479-msms-isis-Lot5-e40dbe6a5wer",
            Name = "ter goedkeuring",
            StatusId = "40843898-54EE-isis-A661-194F1DA0CE48",
            LanguageCode = "nl",
            DisplayOrder = 2
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "wecv6023-0643-isis-8605-652AF0B658A2",
            Name = "goedgekeurd",
            StatusId = "5684969c-f3e8-isis-9746-51e7e23f2782",
            LanguageCode = "nl",
            DisplayOrder = 3
        });
        builder.Entity<IssueStatus>().HasData(new
        {
            Id = "qwsdd023-0643-isis-8605-652AF0B658A2",
            Name = "afgewerkt en doorgegeven",
            StatusId = "vvvv969c-f3e8-isis-9746-51e7e23f2782",
            LanguageCode = "nl",
            DisplayOrder = 4
        });
        
        builder.Entity<IssueType>().HasData(new
        {
            Id = "d263aa4e-itit-issu-bc99-d561a603748e",
            Name = "Request for Change",
            TypeId = "0e1b34bc-itit-4778-8250-9666ee96ae59",
            LanguageCode = "en",
            DisplayOrder = 0
        });
        builder.Entity<IssueType>().HasData(new
        {
            Id = "wer9e479-itit-40c6-Lot5-e40dbe6a5wer",
            Name = "Off-Specification",
            TypeId = "3960193f-itit-43c6-a6cc-4919e5d345c5",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<IssueType>().HasData(new
        {
            Id = "wer9e479-itit-4ZIP-Lot5-e40dbe6a5wer",
            Name = "Problem",
            TypeId = "40843898-itit-473D-A661-194F1DA0CE48",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<IssueType>().HasData(new
        {
            Id = "273D6023-itit-4F16-8605-652AF0B658A2",
            Name = "Question",
            TypeId = "5684969c-itit-49ac-9746-51e7e23f2782",
            LanguageCode = "en",
            DisplayOrder = 3
        });
        
        builder.Entity<IssuePriority>().HasData(new
        {
            Id = "d263aa4e-ipip-issu-bc99-d561a603748e",
            Name = "Low",
            PriorityId = "0e1b34bc-ipip-4778-8250-9666ee96ae59",
            LanguageCode = "en",
            DisplayOrder = 0
        });
        builder.Entity<IssuePriority>().HasData(new
        {
            Id = "wer9e479-ipip-40c6-Lot5-e40dbe6a5wer",
            Name = "Medium",
            PriorityId = "3960193f-ipip-43c6-a6cc-4919e5d345c5",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<IssuePriority>().HasData(new
        {
            Id = "wer9e479-ipip-4ZIP-Lot5-e40dbe6a5wer",
            Name = "High",
            PriorityId = "40843898-ipip-473D-A661-194F1DA0CE48",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        
        builder.Entity<IssueSeverity>().HasData(new
        {
            Id = "d263aa4e-isis-issu-bc99-d561a603748e",
            Name = "None",
            SeverityId = "0e1b34bc-isis-4778-8250-9666ee96ae59",
            LanguageCode = "en",
            DisplayOrder = 0
        });
        builder.Entity<IssueSeverity>().HasData(new
        {
            Id = "wer9e479-isis-40c6-Lot5-e40dbe6a5wer",
            Name = "Minor",
            SeverityId = "3960193f-isis-43c6-a6cc-4919e5d345c5",
            LanguageCode = "en",
            DisplayOrder = 1
        });
        builder.Entity<IssueSeverity>().HasData(new
        {
            Id = "wer9e479-isis-4ZIP-Lot5-e40dbe6a5wer",
            Name = "Major",
            SeverityId = "40843898-isis-473D-A661-194F1DA0CE48",
            LanguageCode = "en",
            DisplayOrder = 2
        });
        builder.Entity<IssueSeverity>().HasData(new
        {
            Id = "273D6023-isis-4F16-8605-652AF0B658A2",
            Name = "Critical",
            SeverityId = "5684969c-isis-49ac-9746-51e7e23f2782",
            LanguageCode = "en",
            DisplayOrder = 3
        });
    }
}