using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class AddPronunciationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PronunciationData",
                columns: table => new
                {
                    PronunciationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Word = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IPA = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AudioUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DifficultySyllables = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SyllableBreakdown = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PronunciationNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LanguageCode = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PronunciationData", x => x.PronunciationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PronunciationData_Word_LanguageCode",
                table: "PronunciationData",
                columns: new[] { "Word", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PronunciationData_LanguageCode",
                table: "PronunciationData",
                column: "LanguageCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PronunciationData");
        }
    }
}
