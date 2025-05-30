using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceBioWithPersonalityPrompts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "DatingProfiles");

            migrationBuilder.DropColumn(
                name: "Prompt",
                table: "DatingProfiles");

            migrationBuilder.AddColumn<string>(
                name: "PersonalityPrompts",
                table: "DatingProfiles",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalityPrompts",
                table: "DatingProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "DatingProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prompt",
                table: "DatingProfiles",
                type: "text",
                nullable: true);
        }
    }
}
