﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.PatientDbMigrations
{
    [DbContext(typeof(PatientDbContext))]
    [Migration("20220606131938_Emergency_location")]
    partial class Emergency_location
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.PatientData.EmergencyNotice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Sent")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("EmergencyNotices");
                });

            modelBuilder.Entity("Domain.Entities.PatientData.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CareWorkerId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Patients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthDate = new DateTime(1993, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CareWorkerId = 0,
                            Email = "testtester@test.com",
                            FirstName = "Test",
                            HomeNumber = "215",
                            LastName = "Tester",
                            LocationId = 1,
                            PhoneNumber = "0687654321",
                            PostalCode = "5223 DE",
                            Status = "active"
                        },
                        new
                        {
                            Id = 2,
                            BirthDate = new DateTime(1998, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CareWorkerId = 0,
                            Email = "testtesteranderson@test.com",
                            FirstName = "Teste",
                            HomeNumber = "123",
                            LastName = "Anderson",
                            LocationId = 2,
                            PhoneNumber = "0612346789",
                            PostalCode = "1234 DE",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.PatientData.PatientLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PatientLocations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = "TestLocatie",
                            RoomId = "1",
                            Status = "active"
                        },
                        new
                        {
                            Id = 2,
                            Department = "TestLocatie",
                            RoomId = "2",
                            Status = "active"
                        });
                });

            modelBuilder.Entity("Domain.Entities.PatientData.EmergencyNotice", b =>
                {
                    b.HasOne("Domain.Entities.PatientData.Patient", "Patient")
                        .WithMany("EmergencyNotices")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Domain.Entities.PatientData.Patient", b =>
                {
                    b.HasOne("Domain.Entities.PatientData.PatientLocation", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Domain.Entities.PatientData.Patient", b =>
                {
                    b.Navigation("EmergencyNotices");
                });
#pragma warning restore 612, 618
        }
    }
}
