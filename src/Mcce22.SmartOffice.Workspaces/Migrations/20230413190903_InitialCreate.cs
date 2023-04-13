using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mcce22.SmartOffice.Workspaces.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkspaceConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeskHeight = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    WorkspaceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<int>(type: "int", nullable: false),
                    Noise = table.Column<int>(type: "int", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    Co2 = table.Column<int>(type: "int", nullable: false),
                    Luminosity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workspaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkspaceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Top = table.Column<int>(type: "int", nullable: false),
                    Left = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceConfigurations_UserId_WorkspaceId",
                table: "WorkspaceConfigurations",
                columns: new[] { "UserId", "WorkspaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_WorkspaceNumber",
                table: "Workspaces",
                column: "WorkspaceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkspaceConfigurations");

            migrationBuilder.DropTable(
                name: "WorkspaceData");

            migrationBuilder.DropTable(
                name: "Workspaces");
        }
    }
}
