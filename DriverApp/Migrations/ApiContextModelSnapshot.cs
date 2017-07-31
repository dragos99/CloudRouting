using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DriverApp.Models;

namespace DriverApp.Migrations
{
    [DbContext(typeof(ApiContext))]
    partial class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DriverApp.Models.Driver", b =>
                {
                    b.Property<string>("DriverId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Done");

                    b.Property<string>("ManagerCustomerKey");

                    b.HasKey("DriverId");

                    b.HasIndex("ManagerCustomerKey");

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

                    b.Property<string>("CityName");

                    b.Property<string>("Comment");

                    b.Property<bool>("Complete");

                    b.Property<string>("CountryCode");

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

                    b.Property<string>("StopStartTime");

                    b.Property<string>("StreetName");

                    b.Property<string>("StreetNumber");

                    b.Property<string>("TimeWindowFrom");

                    b.Property<string>("TimeWindowTo");

                    b.Property<int>("TripId");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DriverApp.Models.Driver", b =>
                {
                    b.HasOne("DriverApp.Models.Manager", "Manager")
                        .WithMany("Drivers")
                        .HasForeignKey("ManagerCustomerKey");
                });
        }
    }
}
