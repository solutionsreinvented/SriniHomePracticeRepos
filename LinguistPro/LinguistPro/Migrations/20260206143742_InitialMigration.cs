using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Verbs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Infinitive = table.Column<string>(type: "TEXT", nullable: false),
                    Meaning = table.Column<string>(type: "TEXT", nullable: false),
                    S1 = table.Column<string>(type: "TEXT", nullable: false),
                    S2 = table.Column<string>(type: "TEXT", nullable: false),
                    S2F = table.Column<string>(type: "TEXT", nullable: false),
                    S3 = table.Column<string>(type: "TEXT", nullable: false),
                    P1 = table.Column<string>(type: "TEXT", nullable: false),
                    P2 = table.Column<string>(type: "TEXT", nullable: false),
                    P2F = table.Column<string>(type: "TEXT", nullable: false),
                    P3 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vocabulary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceWord = table.Column<string>(type: "TEXT", nullable: false),
                    TargetMeaning = table.Column<string>(type: "TEXT", nullable: false),
                    Definition = table.Column<string>(type: "TEXT", nullable: false),
                    UsageExample = table.Column<string>(type: "TEXT", nullable: false),
                    MasteryPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    LastInteraction = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocabulary", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Verbs");

            migrationBuilder.DropTable(
                name: "Vocabulary");
        }
    }
}
