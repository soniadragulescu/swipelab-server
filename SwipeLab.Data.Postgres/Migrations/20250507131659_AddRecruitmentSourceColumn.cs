using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentSourceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecruitmentSource",
                table: "Participants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecruitmentSource",
                table: "Participants");
        }
    }
}
