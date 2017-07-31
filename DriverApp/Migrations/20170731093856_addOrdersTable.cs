using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DriverApp.Migrations
{
    public partial class addOrdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Complete = table.Column<bool>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
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
                    StopStartTime = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<string>(nullable: true),
                    TimeWindowFrom = table.Column<string>(nullable: true),
                    TimeWindowTo = table.Column<string>(nullable: true),
                    TripId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
