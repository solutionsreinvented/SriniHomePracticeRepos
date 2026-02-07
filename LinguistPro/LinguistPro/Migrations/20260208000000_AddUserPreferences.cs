using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    PreferencesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Theme = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    FontSize = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    DefaultLanguageCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    ItemsPerPage = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableEmailNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnablePushNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableStreakReminders = table.Column<bool>(type: "INTEGER", nullable: false),
                    DailyReminderHour = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableSoundNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSaveInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    ShowPronunciationGuide = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowUsageExamples = table.Column<bool>(type: "INTEGER", nullable: false),
                    UILanguage = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.PreferencesId);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreferences");
        }
    }
}
