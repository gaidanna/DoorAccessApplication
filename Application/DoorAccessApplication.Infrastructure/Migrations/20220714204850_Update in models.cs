using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoorAccessApplication.Infrastructure.Migrations
{
    public partial class Updateinmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Locks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "Locks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LockHistoryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LockId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockHistoryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LockHistoryEntries_Locks_LockId",
                        column: x => x.LockId,
                        principalTable: "Locks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LockHistoryEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LockHistoryEntries_LockId",
                table: "LockHistoryEntries",
                column: "LockId");

            migrationBuilder.CreateIndex(
                name: "IX_LockHistoryEntries_UserId",
                table: "LockHistoryEntries",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LockHistoryEntries");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Locks");

            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "Locks");
        }
    }
}
