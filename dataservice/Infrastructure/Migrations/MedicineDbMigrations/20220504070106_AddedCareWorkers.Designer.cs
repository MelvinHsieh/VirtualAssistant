﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    [DbContext(typeof(MedicineDbContext))]
    [Migration("20220504070106_AddedCareWorkers")]
    partial class AddedCareWorkers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.MedicalData.CareWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Function")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Function");

                    b.ToTable("CareWorkers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Petra",
                            Function = "Verpleger",
                            LastName = "Janssen",
                            Status = "active"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Henny",
                            Function = "Verpleger",
                            LastName = "Heeren",
                            Status = "active"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "Peter",
                            Function = "Dokter",
                            LastName = "Peters",
                            Status = "active"
                        },
                        new
                        {
                            Id = 4,
                            FirstName = "Frida",
                            Function = "Dokter",
                            LastName = "Leuken",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.CareWorkerFunction", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Name");

                    b.ToTable("CareWorkerFunctions");

                    b.HasData(
                        new
                        {
                            Name = "Verpleger",
                            Status = "active"
                        },
                        new
                        {
                            Name = "Dokter",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.DoseUnit", b =>
                {
                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Unit");

                    b.ToTable("DoseUnits");

                    b.HasData(
                        new
                        {
                            Unit = "mg",
                            Status = "active"
                        },
                        new
                        {
                            Unit = "µg",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.IntakeRegistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("PatientIntakeId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TakenOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PatientIntakeId");

                    b.ToTable("IntakeRegistrations");
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Dose")
                        .HasColumnType("float");

                    b.Property<string>("DoseUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Indication")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Shape")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Color");

                    b.HasIndex("DoseUnit");

                    b.HasIndex("Shape");

                    b.HasIndex("Type");

                    b.ToTable("Medicine");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Color = "Wit",
                            Dose = 10.0,
                            DoseUnit = "mg",
                            Indication = "Hypercholesterolemie",
                            Name = "Rosuvastatine",
                            Shape = "Vierkant",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 2,
                            Color = "Rood",
                            Dose = 12.5,
                            DoseUnit = "mg",
                            Indication = "Hypertensei",
                            Name = "Hydrochloorthiazide",
                            Shape = "Rond",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 3,
                            Color = "Wit",
                            Dose = 500.0,
                            DoseUnit = "mg",
                            Indication = "Diabetes Mellitus type 2",
                            Name = "Metformine",
                            Shape = "Vierkant",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 4,
                            Color = "Blauw",
                            Dose = 80.0,
                            DoseUnit = "mg",
                            Indication = "Maagbeschermer",
                            Name = "Pantroprazol msr",
                            Shape = "Vierkant",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 5,
                            Color = "Wit",
                            Dose = 50.0,
                            DoseUnit = "mg",
                            Indication = "Antibiotica (Urineweginfectie)",
                            Name = "Nitrofurantione",
                            Shape = "Vierkant",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 6,
                            Color = "Groen",
                            Dose = 10.0,
                            DoseUnit = "mg",
                            Indication = "Somberheid",
                            Name = "Temazepam",
                            Shape = "Rond",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 7,
                            Color = "Wit",
                            Dose = 40.0,
                            DoseUnit = "mg",
                            Indication = "Hartfalen",
                            Name = "Furosemide",
                            Shape = "Hexagonaal",
                            Status = "active",
                            Type = "Tablet"
                        },
                        new
                        {
                            Id = 8,
                            Color = "Zwart",
                            Dose = 5.0,
                            DoseUnit = "mg",
                            Indication = "Nycturie",
                            Name = "Finasteride",
                            Shape = "Rond",
                            Status = "active",
                            Type = "Capsule"
                        },
                        new
                        {
                            Id = 9,
                            Color = "Wit",
                            Dose = 10.0,
                            DoseUnit = "mg",
                            Indication = "Slaapproblemen",
                            Name = "Oxazepam",
                            Shape = "Vierkant",
                            Status = "active",
                            Type = "Tablet"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.MedicineColor", b =>
                {
                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Color");

                    b.ToTable("Medicine_Colors");

                    b.HasData(
                        new
                        {
                            Color = "Rood",
                            Status = "active"
                        },
                        new
                        {
                            Color = "Wit",
                            Status = "active"
                        },
                        new
                        {
                            Color = "Blauw",
                            Status = "active"
                        },
                        new
                        {
                            Color = "Geel",
                            Status = "active"
                        },
                        new
                        {
                            Color = "Groen",
                            Status = "active"
                        },
                        new
                        {
                            Color = "Zwart",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.MedicineShape", b =>
                {
                    b.Property<string>("Shape")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Shape");

                    b.ToTable("Medicine_Shapes");

                    b.HasData(
                        new
                        {
                            Shape = "Vierkant",
                            Status = "active"
                        },
                        new
                        {
                            Shape = "Hexagonaal",
                            Status = "active"
                        },
                        new
                        {
                            Shape = "Rond",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.MedicineType", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Type");

                    b.ToTable("Medicine_Types");

                    b.HasData(
                        new
                        {
                            Type = "Pil",
                            Status = "active"
                        },
                        new
                        {
                            Type = "Tablet",
                            Status = "active"
                        },
                        new
                        {
                            Type = "Capsule",
                            Status = "active"
                        },
                        new
                        {
                            Type = "Spuit",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.PatientIntake", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("IntakeEnd")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("IntakeStart")
                        .HasColumnType("time");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MedicineId");

                    b.ToTable("PatientIntakes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 1,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 11, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 2,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 12, 0, 0, 0),
                            MedicineId = 2,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 3,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 4,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 5,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 7,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 6,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 8,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 7,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 9,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 8,
                            PatientId = 1,
                            Status = "active"
                        },
                        new
                        {
                            Id = 10,
                            Amount = 1,
                            IntakeEnd = new TimeSpan(0, 23, 59, 59, 0),
                            IntakeStart = new TimeSpan(0, 0, 0, 0, 0),
                            MedicineId = 9,
                            PatientId = 1,
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.CareWorker", b =>
                {
                    b.HasOne("Domain.Entities.MedicalData.CareWorkerFunction", null)
                        .WithMany()
                        .HasForeignKey("Function")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.IntakeRegistration", b =>
                {
                    b.HasOne("Domain.Entities.MedicalData.PatientIntake", "PatientIntake")
                        .WithMany()
                        .HasForeignKey("PatientIntakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PatientIntake");
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.Medicine", b =>
                {
                    b.HasOne("Domain.Entities.MedicalData.MedicineColor", null)
                        .WithMany()
                        .HasForeignKey("Color");

                    b.HasOne("Domain.Entities.MedicalData.DoseUnit", null)
                        .WithMany()
                        .HasForeignKey("DoseUnit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.MedicalData.MedicineShape", null)
                        .WithMany()
                        .HasForeignKey("Shape");

                    b.HasOne("Domain.Entities.MedicalData.MedicineType", null)
                        .WithMany()
                        .HasForeignKey("Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.MedicalData.PatientIntake", b =>
                {
                    b.HasOne("Domain.Entities.MedicalData.Medicine", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");
                });
#pragma warning restore 612, 618
        }
    }
}