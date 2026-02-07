using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class AddLearningStreakAndDailyLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyLearningLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    LearningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VocabularyItemsLearned = table.Column<int>(type: "INTEGER", nullable: false),
                    VerbsLearned = table.Column<int>(type: "INTEGER", nullable: false),
                    LanguageItemsLearned = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalTimeSpentSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemsMastered = table.Column<int>(type: "INTEGER", nullable: false),
                    MasteryLevelAtEndOfDay = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyLearningLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_DailyLearningLogs_LanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "LanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningStreaks",
                columns: table => new
                {
                    StreakId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    LongestStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    StreakStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLearningDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalLearningDays = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningStreaks", x => x.StreakId);
                    table.ForeignKey(
                        name: "FK_LearningStreaks_LanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "LanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyLearningLogs_LanguageProfileId_LearningDate",
                table: "DailyLearningLogs",
                columns: new[] { "LanguageProfileId", "LearningDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningStreaks_LanguageProfileId",
                table: "LearningStreaks",
                column: "LanguageProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyLearningLogs");

            migrationBuilder.DropTable(
                name: "LearningStreaks");
        }
    }
}
