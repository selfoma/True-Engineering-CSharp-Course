using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EveryoneToTheHackathon.Domain.Migrations._2024
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => new { x.EmployeeId, x.Role });
                });

            migrationBuilder.CreateTable(
                name: "Hackathons",
                columns: table => new
                {
                    HackathonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HarmonicMean = table.Column<decimal>(type: "decimal(16,12)", precision: 16, scale: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hackathons", x => x.HackathonId);
                });

            migrationBuilder.CreateTable(
                name: "HackathonDreamTeams",
                columns: table => new
                {
                    JuniorId = table.Column<int>(type: "int", nullable: false),
                    TeamLeadId = table.Column<int>(type: "int", nullable: false),
                    HackathonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JuniorRole = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamLeadRole = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HackathonDreamTeams", x => new { x.HackathonId, x.TeamLeadId, x.JuniorId });
                    table.ForeignKey(
                        name: "FK_HackathonDreamTeams_Employees_JuniorId_JuniorRole",
                        columns: x => new { x.JuniorId, x.JuniorRole },
                        principalTable: "Employees",
                        principalColumns: new[] { "EmployeeId", "Role" });
                    table.ForeignKey(
                        name: "FK_HackathonDreamTeams_Employees_TeamLeadId_TeamLeadRole",
                        columns: x => new { x.TeamLeadId, x.TeamLeadRole },
                        principalTable: "Employees",
                        principalColumns: new[] { "EmployeeId", "Role" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HackathonDreamTeams_Hackathons_HackathonId",
                        column: x => x.HackathonId,
                        principalTable: "Hackathons",
                        principalColumn: "HackathonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HackathonEmployeeWishListMappings",
                columns: table => new
                {
                    MappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HackathonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeRole = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HackathonEmployeeWishListMappings", x => x.MappingId);
                    table.ForeignKey(
                        name: "FK_HackathonEmployeeWishListMappings_Employees_EmployeeId_EmployeeRole",
                        columns: x => new { x.EmployeeId, x.EmployeeRole },
                        principalTable: "Employees",
                        principalColumns: new[] { "EmployeeId", "Role" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HackathonEmployeeWishListMappings_Hackathons_HackathonId",
                        column: x => x.HackathonId,
                        principalTable: "Hackathons",
                        principalColumn: "HackathonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishLists",
                columns: table => new
                {
                    WishListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferenceValue = table.Column<int>(type: "int", nullable: false),
                    PreferredEmployeeId = table.Column<int>(type: "int", nullable: false),
                    PreferredEmployeeRole = table.Column<int>(type: "int", nullable: false),
                    MappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishLists", x => x.WishListId);
                    table.ForeignKey(
                        name: "FK_WishLists_HackathonEmployeeWishListMappings_MappingId",
                        column: x => x.MappingId,
                        principalTable: "HackathonEmployeeWishListMappings",
                        principalColumn: "MappingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HackathonDreamTeams_JuniorId_JuniorRole",
                table: "HackathonDreamTeams",
                columns: new[] { "JuniorId", "JuniorRole" });

            migrationBuilder.CreateIndex(
                name: "IX_HackathonDreamTeams_TeamLeadId_TeamLeadRole",
                table: "HackathonDreamTeams",
                columns: new[] { "TeamLeadId", "TeamLeadRole" });

            migrationBuilder.CreateIndex(
                name: "IX_HackathonEmployeeWishListMappings_EmployeeId_EmployeeRole_HackathonId",
                table: "HackathonEmployeeWishListMappings",
                columns: new[] { "EmployeeId", "EmployeeRole", "HackathonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HackathonEmployeeWishListMappings_HackathonId",
                table: "HackathonEmployeeWishListMappings",
                column: "HackathonId");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_MappingId",
                table: "WishLists",
                column: "MappingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HackathonDreamTeams");

            migrationBuilder.DropTable(
                name: "WishLists");

            migrationBuilder.DropTable(
                name: "HackathonEmployeeWishListMappings");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Hackathons");
        }
    }
}
