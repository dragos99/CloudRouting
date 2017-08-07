using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DriverApp.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Managers_ManagerDriverId",
                table: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ManagerDriverId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "ManagerDriverId",
                table: "Drivers");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerKey",
                table: "Managers",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerKey",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "CustomerKey");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CustomerKey",
                table: "Drivers",
                column: "CustomerKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Managers_CustomerKey",
                table: "Drivers",
                column: "CustomerKey",
                principalTable: "Managers",
                principalColumn: "CustomerKey",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Managers_CustomerKey",
                table: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CustomerKey",
                table: "Drivers");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerKey",
                table: "Managers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4);

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Managers",
                maxLength: 4,
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerKey",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerDriverId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ManagerDriverId",
                table: "Drivers",
                column: "ManagerDriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Managers_ManagerDriverId",
                table: "Drivers",
                column: "ManagerDriverId",
                principalTable: "Managers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
