﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehiclesApi.Data;

#nullable disable

namespace VehiclesApi.Data.Migrations
{
    [DbContext(typeof(VehiclesDbContext))]
    [Migration("20240107140631_SeedBrandsAndEquipment")]
    partial class SeedBrandsAndEquipment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VehiclesApi.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Toyota"
                        },
                        new
                        {
                            Id = 2,
                            Name = "General Motors"
                        },
                        new
                        {
                            Id = 3,
                            Name = "BMW"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Tesla"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Hyundai"
                        });
                });

            modelBuilder.Entity("VehiclesApi.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Equipment");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Parking sensor"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Rear view camera"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Leather seats"
                        });
                });

            modelBuilder.Entity("VehiclesApi.Models.Vehicle", b =>
                {
                    b.Property<string>("Vin")
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("LicensePlateNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Vin");

                    b.HasIndex("BrandId");

                    b.HasIndex("LicensePlateNumber")
                        .IsUnique();

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("VehiclesApi.Models.VehicleEquipment", b =>
                {
                    b.Property<string>("Vin")
                        .HasColumnType("nvarchar(17)");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.HasKey("Vin", "EquipmentId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("VehicleEquipment");
                });

            modelBuilder.Entity("VehiclesApi.Models.Vehicle", b =>
                {
                    b.HasOne("VehiclesApi.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("VehiclesApi.Models.VehicleEquipment", b =>
                {
                    b.HasOne("VehiclesApi.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VehiclesApi.Models.Vehicle", "Vehicle")
                        .WithMany("VehicleEquipment")
                        .HasForeignKey("Vin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("VehiclesApi.Models.Vehicle", b =>
                {
                    b.Navigation("VehicleEquipment");
                });
#pragma warning restore 612, 618
        }
    }
}
