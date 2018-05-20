using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace iParking.Migrations
{
    public partial class latlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Parkings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Parkings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CarCategory",
                table: "ParkingReservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarNumber",
                table: "ParkingReservations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Parkings");

            migrationBuilder.DropColumn(
                name: "CarCategory",
                table: "ParkingReservations");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "ParkingReservations");
        }
    }
}
