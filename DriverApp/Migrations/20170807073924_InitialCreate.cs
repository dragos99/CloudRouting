using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DriverApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    DriverId = table.Column<int>(maxLength: 4, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    ArrivalDateTime = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Complete = table.Column<bool>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    DepartureDateTime = table.Column<string>(nullable: true),
                    Distance = table.Column<float>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    FixedDurationInSec = table.Column<int>(nullable: false),
                    GeoX = table.Column<float>(nullable: false),
                    GeoY = table.Column<float>(nullable: false),
                    GivenX = table.Column<float>(nullable: false),
                    GivenY = table.Column<float>(nullable: false),
                    OrderNumber = table.Column<string>(nullable: true),
                    OrderType = table.Column<string>(nullable: true),
                    StopDurationInSec = table.Column<int>(nullable: false),
                    StopFinishTime = table.Column<string>(nullable: true),
                    StopSequence = table.Column<int>(nullable: false),
                    StopStartTime = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<string>(nullable: true),
                    TimeWindowFrom = table.Column<string>(nullable: true),
                    TimeWindowTill = table.Column<string>(nullable: true),
                    TripId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AvailableFromTime = table.Column<DateTime>(nullable: false),
                    AvailableTillTime = table.Column<DateTime>(nullable: false),
                    DriverId = table.Column<string>(nullable: true),
                    FinishTime = table.Column<DateTime>(nullable: false),
                    NOfStops = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TotalDistanceInKm = table.Column<float>(nullable: false),
                    TotalDurationInSec = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<string>(nullable: false),
                    CustomerKey = table.Column<string>(nullable: true),
                    ManagerDriverId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                    table.ForeignKey(
                        name: "FK_Drivers_Managers_ManagerDriverId",
                        column: x => x.ManagerDriverId,
                        principalTable: "Managers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ManagerDriverId",
                table: "Drivers",
                column: "ManagerDriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
