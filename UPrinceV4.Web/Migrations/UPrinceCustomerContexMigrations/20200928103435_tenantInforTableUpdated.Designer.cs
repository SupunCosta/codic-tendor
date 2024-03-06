﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Migrations.UPrinceCustomerContexMigrations
{
    [DbContext(typeof(UPrinceCustomerContex))]
    [Migration("20200928103435_tenantInforTableUpdated")]
    partial class tenantInforTableUpdated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("UprinceCustomer");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerContactPreference", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UPrinceCustomerJobRoleId")
                        .HasColumnType("int");

                    b.Property<int>("UprinceCustomerId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UprinceCustomerId")
                        .IsUnique();

                    b.ToTable("UprinceCustomerContactPreference");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerContractingUnit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CatelogConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UPrinceCustomerTenantsInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UPrinceCustomerContractingUnit");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerJobRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("UprinceCustomerJobRole");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerLegalAddress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UprinceCustomerProfileId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UprinceCustomerProfileId")
                        .IsUnique();

                    b.ToTable("UprinceCustomerLegalAddress");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerLocation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UprinceCustomerId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UprinceCustomerId");

                    b.ToTable("UprinceCustomerLocation");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerPrimaryContact", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UprinceCustomerProfileId")
                        .HasColumnType("int");

                    b.Property<string>("phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("UprinceCustomerProfileId")
                        .IsUnique();

                    b.ToTable("UprinceCustomerPrimaryContact");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerProfile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UprinceCustomerId")
                        .HasColumnType("int");

                    b.Property<string>("VerificationStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("UprinceCustomerId")
                        .IsUnique();

                    b.ToTable("UprinceCustomerProfile");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerTenantsInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AzureBlob")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AzureContainer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CatelogConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientSecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DatabaseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Host")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StorageConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenantId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UPrinceCustomerTenantsInfo");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerContactPreferenceHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UPrinceCustomerJobRoleId")
                        .HasColumnType("int");

                    b.Property<int>("UprinceCustomerId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("UprinceCustomerContactPreferenceHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerHistory", b =>
                {
                    b.Property<DateTime>("SysStartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("SysEndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SysStartTime");

                    b.ToTable("UprinceCustomerHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerJobRoleHistory", b =>
                {
                    b.Property<DateTime>("SysStartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysEndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SysStartTime");

                    b.ToTable("UprinceCustomerJobRoleHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerLegalAddressHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UprinceCustomerProfileId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("UprinceCustomerLegalAddressHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerLocationHistory", b =>
                {
                    b.Property<DateTime>("SysStartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysEndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UprinceCustomerId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SysStartTime");

                    b.ToTable("UprinceCustomerLocationHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerPrimaryContactHistory", b =>
                {
                    b.Property<DateTime>("SysStartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SysEndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UprinceCustomerProfileId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SysStartTime");

                    b.ToTable("UprinceCustomerPrimaryContactHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UprinceCustomerProfileHistory", b =>
                {
                    b.Property<DateTime>("SysStartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("SysEndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UprinceCustomerId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerificationStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SysStartTime");

                    b.ToTable("UprinceCustomerProfileHistory");
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerContactPreference", b =>
                {
                    b.HasOne("UPrinceV4.Web.Data.UPrinceCustomer", null)
                        .WithOne("UprinceCustomerContactPreference")
                        .HasForeignKey("UPrinceV4.Web.Data.UPrinceCustomerContactPreference", "UprinceCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerLegalAddress", b =>
                {
                    b.HasOne("UPrinceV4.Web.Data.UPrinceCustomerProfile", null)
                        .WithOne("UprinceCustomerLegalAddress")
                        .HasForeignKey("UPrinceV4.Web.Data.UPrinceCustomerLegalAddress", "UprinceCustomerProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerLocation", b =>
                {
                    b.HasOne("UPrinceV4.Web.Data.UPrinceCustomer", null)
                        .WithMany("UprinceCustomerLocations")
                        .HasForeignKey("UprinceCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerPrimaryContact", b =>
                {
                    b.HasOne("UPrinceV4.Web.Data.UPrinceCustomerProfile", null)
                        .WithOne("UprinceCustomerPrimaryContact")
                        .HasForeignKey("UPrinceV4.Web.Data.UPrinceCustomerPrimaryContact", "UprinceCustomerProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UPrinceV4.Web.Data.UPrinceCustomerProfile", b =>
                {
                    b.HasOne("UPrinceV4.Web.Data.UPrinceCustomer", null)
                        .WithOne("UprinceCustomerProfile")
                        .HasForeignKey("UPrinceV4.Web.Data.UPrinceCustomerProfile", "UprinceCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
