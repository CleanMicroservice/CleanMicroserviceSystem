﻿// <auto-generated />
using System;
using CleanMicroserviceSystem.Themis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CleanMicroserviceSystem.Themis.Infrastructure.Migrations.ConfigurationDb
{
    [DbContext(typeof(ConfigurationDbContext))]
    [Migration("20230218091347_MigrateConfigurationDbContext")]
    partial class MigrateConfigurationDbContext
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("CleanMicroserviceSystem.Themis.Domain.Entities.Configuration.ApiResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ApiResources");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = 1,
                            CreatedOn = new DateTime(2023, 2, 18, 9, 13, 47, 65, DateTimeKind.Utc).AddTicks(3500),
                            Description = "ThemisAPI",
                            Enabled = true,
                            Name = "ThemisAPI"
                        });
                });

            modelBuilder.Entity("CleanMicroserviceSystem.Themis.Domain.Entities.Configuration.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("Secret")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = 1,
                            CreatedOn = new DateTime(2023, 2, 18, 9, 13, 47, 65, DateTimeKind.Utc).AddTicks(3585),
                            Description = "Tethys",
                            Enabled = true,
                            Name = "Tethys",
                            Secret = "dZ4LIKrWTu4W+XlkYYEamdddV4MrXnxZpjPUQClKn+8="
                        });
                });

            modelBuilder.Entity("CleanMicroserviceSystem.Themis.Domain.Entities.Configuration.ClientClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClientId", "ClaimType")
                        .IsUnique();

                    b.ToTable("ClientClaims");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "ThemisAPI.Read",
                            ClaimValue = "ThemisAPI.Read",
                            ClientId = 1
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "ThemisAPI.Write",
                            ClaimValue = "ThemisAPI.Write",
                            ClientId = 1
                        });
                });

            modelBuilder.Entity("CleanMicroserviceSystem.Themis.Domain.Entities.Configuration.ClientClaim", b =>
                {
                    b.HasOne("CleanMicroserviceSystem.Themis.Domain.Entities.Configuration.Client", "Client")
                        .WithMany("Claims")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("CleanMicroserviceSystem.Themis.Domain.Entities.Configuration.Client", b =>
                {
                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}
