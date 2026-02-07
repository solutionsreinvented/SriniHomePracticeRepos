using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DifficultyLevel = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    TotalQuestions = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "INTEGER", nullable: false),
                    ScorePercentage = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeSpentSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.QuizAttemptId);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_LanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "LanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizStatistics",
                columns: table => new
                {
                    QuizStatisticsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalQuizzesTaken = table.Column<int>(type: "INTEGER", nullable: false),
                    BestScore = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageScore = table.Column<double>(type: "REAL", nullable: false),
                    TotalTimeSpentSeconds = table.Column<long>(type: "INTEGER", nullable: false),
                    LastQuizDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CurrentQuizStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    BestQuizStreak = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizStatistics", x => x.QuizStatisticsId);
                    table.ForeignKey(
                        name: "FK_QuizStatistics_LanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "LanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    QuizQuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    VocabularyItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    VerbEntryId = table.Column<int>(type: "INTEGER", nullable: true),
                    QuestionType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    Options = table.Column<string>(type: "TEXT", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: true),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    TimeSpentSeconds = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.QuizQuestionId);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Verbs_VerbEntryId",
                        column: x => x.VerbEntryId,
                        principalTable: "VerbEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Vocabulary_VocabularyItemId",
                        column: x => x.VocabularyItemId,
                        principalTable: "Vocabulary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_LanguageProfileId",
                table: "QuizAttempts",
                column: "LanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizAttemptId",
                table: "QuizQuestions",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_VerbEntryId",
                table: "QuizQuestions",
                column: "VerbEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_VocabularyItemId",
                table: "QuizQuestions",
                column: "VocabularyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizStatistics_LanguageProfileId",
                table: "QuizStatistics",
                column: "LanguageProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "QuizStatistics");

            migrationBuilder.DropTable(
                name: "QuizAttempts");
        }
    }
}
