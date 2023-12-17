﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApi.Helpers;

#nullable disable

namespace flightMVC.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231227112424_updated")]
    partial class updated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("flightMVC.Models.Aircraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AircraftModelId")
                        .HasColumnType("integer");

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AircraftModelId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Aircrafts");
                });

            modelBuilder.Entity("flightMVC.Models.AircraftModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AircraftModels");
                });

            modelBuilder.Entity("flightMVC.Models.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("flightMVC.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("flightMVC.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AircraftId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("RouteId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("flightMVC.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ArrivalAirportId")
                        .HasColumnType("integer");

                    b.Property<int>("DepartureAirportId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalAirportId");

                    b.HasIndex("DepartureAirportId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("flightMVC.Models.Aircraft", b =>
                {
                    b.HasOne("flightMVC.Models.AircraftModel", "AircraftModel")
                        .WithMany("Aircrafts")
                        .HasForeignKey("AircraftModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("flightMVC.Models.Company", "Company")
                        .WithMany("Aircrafts")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AircraftModel");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("flightMVC.Models.Flight", b =>
                {
                    b.HasOne("flightMVC.Models.Aircraft", "Aircraft")
                        .WithMany()
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("flightMVC.Models.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aircraft");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("flightMVC.Models.Route", b =>
                {
                    b.HasOne("flightMVC.Models.Airport", "ArrivalAirport")
                        .WithMany()
                        .HasForeignKey("ArrivalAirportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("flightMVC.Models.Airport", "DepartureAirport")
                        .WithMany()
                        .HasForeignKey("DepartureAirportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArrivalAirport");

                    b.Navigation("DepartureAirport");
                });

            modelBuilder.Entity("flightMVC.Models.AircraftModel", b =>
                {
                    b.Navigation("Aircrafts");
                });

            modelBuilder.Entity("flightMVC.Models.Company", b =>
                {
                    b.Navigation("Aircrafts");
                });
#pragma warning restore 612, 618
        }
    }
}
