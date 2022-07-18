using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoorAccessApplication.Infrastructure.Migrations
{
    public partial class updateinusermodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Locks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Locks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Locks");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Locks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
