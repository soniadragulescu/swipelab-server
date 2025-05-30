using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddJobColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "DatingProfiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Job",
                table: "DatingProfiles");
        }
    }
}
