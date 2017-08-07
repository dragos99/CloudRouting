using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DriverApp.Models;

namespace DriverApp.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20170807091753_2")]
    partial class _2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DriverApp.Models.Driver", b =>
                {
                    b.Property<string>("DriverId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerKey");

                    b.HasKey("DriverId");

                    b.HasIndex("CustomerKey");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("DriverApp.Models.Manager", b =>
                {
                    b.Property<string>("CustomerKey")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4);

                    b.HasKey("CustomerKey");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("DriverApp.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<string>("ArrivalDateTime");

                    b.Property<string>("CityName");

                    b.Property<string>("Comment");

                    b.Property<bool>("Complete");

                    b.Property<string>("CountryCode");

                    b.Property<string>("DepartureDateTime");

                    b.Property<float>("Distance");

                    b.Property<int>("DriverId");

                    b.Property<int>("FixedDurationInSec");

                    b.Property<float>("GeoX");

                    b.Property<float>("GeoY");

                    b.Property<float>("GivenX");

                    b.Property<float>("GivenY");

                    b.Property<string>("OrderNumber");

                    b.Property<string>("OrderType");

                    b.Property<int>("StopDurationInSec");

                    b.Property<string>("StopFinishTime");

                    b.Property<int>("StopSequence");

                    b.Property<string>("StopStartTime");

                    b.Property<string>("StreetName");

                    b.Property<string>("StreetNumber");

                    b.Property<string>("TimeWindowFrom");

                    b.Property<string>("TimeWindowTill");

                    b.Property<int>("TripId");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DriverApp.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<DateTime>("AvailableFromTime");

                    b.Property<DateTime>("AvailableTillTime");

                    b.Property<string>("DriverId");

                    b.Property<DateTime>("FinishTime");

                    b.Property<int>("NOfStops");

                    b.Property<DateTime>("StartTime");

                    b.Property<float>("TotalDistanceInKm");

                    b.Property<int>("TotalDurationInSec");

                    b.HasKey("Id");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("DriverApp.Models.Driver", b =>
                {
                    b.HasOne("DriverApp.Models.Manager", "Manager")
                        .WithMany("Drivers")
                        .HasForeignKey("CustomerKey");
                });
        }
    }
}
