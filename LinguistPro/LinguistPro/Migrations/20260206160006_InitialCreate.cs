using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguistPro.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetMeaning",
                table: "Vocabulary",
                newName: "Term");

            migrationBuilder.RenameColumn(
                name: "SourceWord",
                table: "Vocabulary",
                newName: "Meaning");

            migrationBuilder.RenameColumn(
                name: "MasteryPoints",
                table: "Vocabulary",
                newName: "Mastery");

            migrationBuilder.RenameColumn(
                name: "LastInteraction",
                table: "Vocabulary",
                newName: "LastReviewed");

            migrationBuilder.RenameColumn(
                name: "S2F",
                table: "Verbs",
                newName: "Tense");

            migrationBuilder.RenameColumn(
                name: "S2",
                table: "Verbs",
                newName: "S2Inf");

            migrationBuilder.RenameColumn(
                name: "P2F",
                table: "Verbs",
                newName: "S2Form");

            migrationBuilder.RenameColumn(
                name: "P2",
                table: "Verbs",
                newName: "P2Inf");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Vocabulary",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Verbs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "P2Form",
                table: "Verbs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Vocabulary");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Verbs");

            migrationBuilder.DropColumn(
                name: "P2Form",
                table: "Verbs");

            migrationBuilder.RenameColumn(
                name: "Term",
                table: "Vocabulary",
                newName: "TargetMeaning");

            migrationBuilder.RenameColumn(
                name: "Meaning",
                table: "Vocabulary",
                newName: "SourceWord");

            migrationBuilder.RenameColumn(
                name: "Mastery",
                table: "Vocabulary",
                newName: "MasteryPoints");

            migrationBuilder.RenameColumn(
                name: "LastReviewed",
                table: "Vocabulary",
                newName: "LastInteraction");

            migrationBuilder.RenameColumn(
                name: "Tense",
                table: "Verbs",
                newName: "S2F");

            migrationBuilder.RenameColumn(
                name: "S2Inf",
                table: "Verbs",
                newName: "S2");

            migrationBuilder.RenameColumn(
                name: "S2Form",
                table: "Verbs",
                newName: "P2F");

            migrationBuilder.RenameColumn(
                name: "P2Inf",
                table: "Verbs",
                newName: "P2");
        }
    }
}
