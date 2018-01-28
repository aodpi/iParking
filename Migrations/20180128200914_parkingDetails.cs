using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace iParking.Migrations
{
    public partial class parkingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParkingName",
                table: "Parkings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSlots",
                table: "Parkings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "Parkings",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingName",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "ParkingSlots",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Parkings");
        }
    }
}
