﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UPrinceV4.Web.Data;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    [DbContext(typeof(UprinceV5Context))]
    [Migration("20231018062838_wbsProduct")]
    partial class wbsProduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UPrinceV4.Web.Data.CIAW.CiawFeatchStatus", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("CiawFeatchStatus");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Status = false
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.CIAW.CiawRemark", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CiawId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CiawRemark");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.CPC.CpcSerialNumber", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CPCId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CpcSerialNumber");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.Contractor.ContractorFileType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContractorFileType");

                    b.HasData(
                        new
                        {
                            Id = "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
                            DisplayOrder = 5,
                            LanguageCode = "en",
                            Name = "ZIP",
                            TypeId = "2210e768-msms-po02-Lot5-ee367a82ad22"
                        },
                        new
                        {
                            Id = "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                            DisplayOrder = 5,
                            LanguageCode = "nl",
                            Name = "ZIP(nl)",
                            TypeId = "2210e768-msms-po02-Lot5-ee367a82ad22"
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.HR.HRContractTypes", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HRContractTypes");

                    b.HasData(
                        new
                        {
                            Id = "005b42ba-574d-4afd-a034-347858e53c9d",
                            LanguageCode = "en",
                            Name = "Tempory",
                            TypeId = "12a22319-8ca7-temp-b588-6fab99474130"
                        },
                        new
                        {
                            Id = "1377a17d-3f18-46c1-bc7c-c11edcf65b5c",
                            LanguageCode = "nl",
                            Name = "Tempory(nl)",
                            TypeId = "12a22319-8ca7-temp-b588-6fab99474130"
                        },
                        new
                        {
                            Id = "222e3dab-576d-4f53-b976-a9b5c97ee165",
                            LanguageCode = "en",
                            Name = "Permenant",
                            TypeId = "41ce52c0-058b-perm-afbd-1d2d24105ebc"
                        },
                        new
                        {
                            Id = "3263aa4e-12a8-4c59-bc99-d561a603748e",
                            LanguageCode = "nl",
                            Name = "Permenant(nl)",
                            TypeId = "41ce52c0-058b-perm-afbd-1d2d24105ebc"
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.HR.HRContractorList", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContractTypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("HRId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HRContractorList");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.PBS_.PbsAssignedLabour", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AssignedHoursPerDay")
                        .HasColumnType("int");

                    b.Property<string>("CabPersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CpcId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DayOfWeek")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PbsLabourId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PbsProduct")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Project")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectManager")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Week")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PbsAssignedLabour");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.PBS_.PbsDisplayOrder", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("SequenceId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PbsDisplayOrder");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.PdfToExcel.ContractorsTotalPriceErrors", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractorPdfId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("CorrectTotalPrice")
                        .HasColumnType("real");

                    b.Property<string>("LotId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quantity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("TotalPrice")
                        .HasColumnType("real");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("UnitPrice")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("ContractorsTotalPriceErrors");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.ThAutomation.ThColors", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Font")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ThColors");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Code = "#c56834",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "2",
                            Code = "#c4d8e5",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "3",
                            Code = "#97c8ea",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "4",
                            Code = "#3b9b36",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "5",
                            Code = "#a5982c",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "6",
                            Code = "#97ac0f",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "7",
                            Code = "#b13748",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "8",
                            Code = "#ea716d",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "9",
                            Code = "#166fdb",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "10",
                            Code = "#f69b68",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "11",
                            Code = "#b3da90",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "12",
                            Code = "#b65ab3",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "13",
                            Code = "#08272B",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "14",
                            Code = "#19D65B",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "15",
                            Code = "#1C6675",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "16",
                            Code = "#AE9675",
                            Font = "#fffffff"
                        },
                        new
                        {
                            Id = "17",
                            Code = "#8C2581",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "18",
                            Code = "#7EF6CE",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "19",
                            Code = "#1C85CF",
                            Font = "#000000"
                        },
                        new
                        {
                            Id = "20",
                            Code = "#0F264F",
                            Font = "#fffffff"
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.ThAutomation.ThTruckAvailability", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActivityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ETime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResourceFamilyId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("STime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SortingOrder")
                        .HasColumnType("int");

                    b.Property<string>("StockId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ThTruckAvailability");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsProduct", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryStatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsFavourite")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WbsTaxonomyId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsProduct");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTask", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryStatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsFavourite")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WbsTaxonomyId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTask");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskCc", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskCc");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskDeliveryStatus", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskDeliveryStatus");

                    b.HasData(
                        new
                        {
                            Id = "d263aa4e-12a8-issu-bc99-d561a603748e",
                            DisplayOrder = 0,
                            LanguageCode = "en",
                            Name = "Overdue",
                            StatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc"
                        },
                        new
                        {
                            Id = "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
                            DisplayOrder = 1,
                            LanguageCode = "en",
                            Name = "Within 7 Days",
                            StatusId = "2210e768-msms-po02-Lot5-ee367a82ad22"
                        },
                        new
                        {
                            Id = "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                            DisplayOrder = 2,
                            LanguageCode = "en",
                            Name = "More than 7 days",
                            StatusId = "12a22319-8ca7-temp-b588-6fab99474130"
                        },
                        new
                        {
                            Id = "lsj3aa4e-12a8-issu-bc99-d561a603748e",
                            DisplayOrder = 0,
                            LanguageCode = "nl",
                            Name = "Overdue(nl)",
                            StatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc"
                        },
                        new
                        {
                            Id = "poie479-msms-40c6-Lot5-e40dbe6a5wer",
                            DisplayOrder = 1,
                            LanguageCode = "nl",
                            Name = "Within 7 Days(nl)",
                            StatusId = "2210e768-msms-po02-Lot5-ee367a82ad22"
                        },
                        new
                        {
                            Id = "bcfe479-msms-4ZIP-Lot5-e40dbe6a5wer",
                            DisplayOrder = 2,
                            LanguageCode = "nl",
                            Name = "More than 7 days(nl)",
                            StatusId = "12a22319-8ca7-temp-b588-6fab99474130"
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskDocuments", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskDocuments");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskEmail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmailId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskEmail");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskInstruction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InstructionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskInstruction");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskStatus", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskStatus");

                    b.HasData(
                        new
                        {
                            Id = "d263aa4e-12a8-issu-bc99-d561a603748e",
                            DisplayOrder = 0,
                            LanguageCode = "en",
                            Name = "full",
                            StatusId = "0e1b34bc-f2c3-4778-8250-9666ee96ae59"
                        },
                        new
                        {
                            Id = "wer9e479-msms-40c6-Lot5-e40dbe6a5wer",
                            DisplayOrder = 1,
                            LanguageCode = "en",
                            Name = "quater",
                            StatusId = "3960193f-99e0-43c6-a6cc-4919e5d345c5"
                        },
                        new
                        {
                            Id = "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer",
                            DisplayOrder = 2,
                            LanguageCode = "en",
                            Name = "half",
                            StatusId = "40843898-54EE-473D-A661-194F1DA0CE48"
                        },
                        new
                        {
                            Id = "273D6023-0643-4F16-8605-652AF0B658A2",
                            DisplayOrder = 3,
                            LanguageCode = "en",
                            Name = "three quater",
                            StatusId = "5684969c-f3e8-49ac-9746-51e7e23f2782"
                        },
                        new
                        {
                            Id = "ehwraa4e-12a8-issu-bc99-d561a603748e",
                            DisplayOrder = 0,
                            LanguageCode = "nl",
                            Name = "full",
                            StatusId = "0e1b34bc-f2c3-4778-8250-9666ee96ae59"
                        },
                        new
                        {
                            Id = "wfede479-msms-40c6-Lot5-e40dbe6a5wer",
                            DisplayOrder = 1,
                            LanguageCode = "nl",
                            Name = "quater",
                            StatusId = "3960193f-99e0-43c6-a6cc-4919e5d345c5"
                        },
                        new
                        {
                            Id = "lksdm479-msms-4ZIP-Lot5-e40dbe6a5wer",
                            DisplayOrder = 2,
                            LanguageCode = "nl",
                            Name = "half",
                            StatusId = "40843898-54EE-473D-A661-194F1DA0CE48"
                        },
                        new
                        {
                            Id = "wecv6023-0643-4F16-8605-652AF0B658A2",
                            DisplayOrder = 3,
                            LanguageCode = "nl",
                            Name = "three quater",
                            StatusId = "5684969c-f3e8-49ac-9746-51e7e23f2782"
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskTags", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskTags");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaskTo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaskTo");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaxonomy", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SequenceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("WbsTaxonomyLevelId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaxonomy");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTaxonomyLevel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DisplayOrder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WbsTaxonomyLevels");

                    b.HasData(
                        new
                        {
                            Id = "3263aa4e-12a8-wbs-bc99-d561a603748e",
                            DisplayOrder = "1",
                            LanguageCode = "en",
                            LevelId = "41ce52c0-058b-wbs-afbd-1d2d24105ebc",
                            Name = "WBS"
                        },
                        new
                        {
                            Id = "p263aa4e-12a8-prod-bc99-d561a603748e",
                            DisplayOrder = "2",
                            LanguageCode = "en",
                            LevelId = "e1ce52c0-058b-prod-afbd-1d2d24105ebc",
                            Name = "Product"
                        },
                        new
                        {
                            Id = "d263aa4e-12a8-issu-bc99-d561a603748e",
                            DisplayOrder = "3",
                            LanguageCode = "en",
                            LevelId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc",
                            Name = "Task"
                        });
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.WBS.WbsTemplate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SequenceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("WbsTemplate");
                });
#pragma warning restore 612, 618
        }
    }
}
