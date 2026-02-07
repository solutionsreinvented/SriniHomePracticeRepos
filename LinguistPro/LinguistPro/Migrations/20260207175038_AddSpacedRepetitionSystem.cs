using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class AddSpacedRepetitionSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VocabularyItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    LanguageItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    LeitnerBox = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IncorrectCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastReviewedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NextReviewDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewSchedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_ReviewSchedules_LanguageItems_LanguageItemId",
                        column: x => x.LanguageItemId,
                        principalTable: "LanguageItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewSchedules_Vocabulary_VocabularyItemId",
                        column: x => x.VocabularyItemId,
                        principalTable: "Vocabulary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSchedules_LanguageItemId",
                table: "ReviewSchedules",
                column: "LanguageItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSchedules_LeitnerBox",
                table: "ReviewSchedules",
                column: "LeitnerBox");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSchedules_NextReviewDate",
                table: "ReviewSchedules",
                column: "NextReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSchedules_VocabularyItemId",
                table: "ReviewSchedules",
                column: "VocabularyItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewSchedules");
        }
    }
}
