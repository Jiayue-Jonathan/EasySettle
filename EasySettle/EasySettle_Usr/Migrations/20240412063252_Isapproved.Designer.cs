﻿// <auto-generated />
using System;
using EasySettle.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EasySettle.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240412063252_Isapproved")]
    partial class Isapproved
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EasySettle.Models.Client", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientID"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("telNo")
                        .HasColumnType("int");

                    b.HasKey("ClientID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("EasySettle.Models.Lease", b =>
                {
                    b.Property<int>("LeaseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeaseID"));

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<decimal>("DepositPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PropertyID")
                        .HasColumnType("int");

                    b.Property<DateTime>("RentFinish")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RentStart")
                        .HasColumnType("datetime2");

                    b.HasKey("LeaseID");

                    b.HasIndex("ClientID")
                        .IsUnique();

                    b.HasIndex("PropertyID");

                    b.ToTable("Leases");
                });

            modelBuilder.Entity("EasySettle.Models.Owner", b =>
                {
                    b.Property<int>("OwnerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OwnerID"));

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("telNo")
                        .HasColumnType("int");

                    b.HasKey("OwnerID");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("EasySettle.Models.Property", b =>
                {
                    b.Property<int>("PropertyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PropertyID"));

                    b.Property<decimal>("BathRooms")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAudited")
                        .HasColumnType("bit");

                    b.Property<int>("OwnerID")
                        .HasColumnType("int");

                    b.Property<bool>("Parking")
                        .HasColumnType("bit");

                    b.Property<bool>("Pets")
                        .HasColumnType("bit");

                    b.Property<int>("Rent")
                        .HasColumnType("int");

                    b.Property<bool>("Rented")
                        .HasColumnType("bit");

                    b.Property<decimal>("Rooms")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Street")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PropertyID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("EasySettle.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EasySettle.Models.UserProperty", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("PropertyID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("Email", "PropertyID");

                    b.HasIndex("PropertyID");

                    b.ToTable("UserProperties");
                });

            modelBuilder.Entity("EasySettle.Models.Lease", b =>
                {
                    b.HasOne("EasySettle.Models.Client", "Client")
                        .WithOne()
                        .HasForeignKey("EasySettle.Models.Lease", "ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasySettle.Models.Property", "Property")
                        .WithMany("Leases")
                        .HasForeignKey("PropertyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("EasySettle.Models.Property", b =>
                {
                    b.HasOne("EasySettle.Models.Owner", "Owner")
                        .WithMany("Properties")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("EasySettle.Models.UserProperty", b =>
                {
                    b.HasOne("EasySettle.Models.User", "User")
                        .WithMany("UserProperties")
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasySettle.Models.Property", "Property")
                        .WithMany("UserProperties")
                        .HasForeignKey("PropertyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EasySettle.Models.Owner", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("EasySettle.Models.Property", b =>
                {
                    b.Navigation("Leases");

                    b.Navigation("UserProperties");
                });

            modelBuilder.Entity("EasySettle.Models.User", b =>
                {
                    b.Navigation("UserProperties");
                });
#pragma warning restore 612, 618
        }
    }
}