﻿// <auto-generated />
using System;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CleanMicroserviceSystem.Themis.Infrastructure.Migrations
{
    [DbContext(typeof(ThemisDbContext))]
    partial class ThemisDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities.GenericOption", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OptionName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("OptionValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerLevel")
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("ID");

                    b.HasIndex("OptionName", "Category", "OwnerLevel")
                        .IsUnique();

                    b.ToTable("GenericOptions");
                });

            modelBuilder.Entity("CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities.WebAPILog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("ElapsedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Exception")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAuthenticated")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("QueryString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestBody")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestURI")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceHost")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("StatusCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TraceIdentifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserAgent")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("RequestURI", "SourceHost", "IdentityName", "CreatedOn");

                    b.ToTable("WebAPILogs");
                });
#pragma warning restore 612, 618
        }
    }
}
