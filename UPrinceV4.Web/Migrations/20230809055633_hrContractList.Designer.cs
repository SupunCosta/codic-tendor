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
    [Migration("20230809055633_hrContractList")]
    partial class hrContractList
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
#pragma warning restore 612, 618
        }
    }
}
