using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DriverApp.Migrations
{
    public partial class addTripsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    AvailableFromTime = table.Column<string>(nullable: true),
                    AvailableTillTime = table.Column<string>(nullable: true),
                    DriverId = table.Column<int>(nullable: false),
                    FinishTime = table.Column<string>(nullable: true),
                    StartTime = table.Column<string>(nullable: true),
                    TotalDistanceInKm = table.Column<int>(nullable: false),
                    TotalDurationInSec = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
