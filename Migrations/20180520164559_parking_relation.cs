using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace iParking.Migrations
{
    public partial class parking_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingId",
                table: "ParkingReservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingReservations_ParkingId",
                table: "ParkingReservations",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingReservations_Parkings_ParkingId",
                table: "ParkingReservations",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingReservations_Parkings_ParkingId",
                table: "ParkingReservations");

            migrationBuilder.DropIndex(
                name: "IX_ParkingReservations_ParkingId",
                table: "ParkingReservations");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "ParkingReservations");
        }
    }
}
