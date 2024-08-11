using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.Migrations
{
    /// <inheritdoc />
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointments",
                newName: "TimeEntry");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExitTime",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExitTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "TimeEntry",
                table: "Appointments",
                newName: "Date");

            migrationBuilder.AddColumn<double>(
                name: "Hours",
                table: "Appointments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
