using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeSpentAndReviewCountcolumnsOnReflection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileReviewCount",
                table: "DatingProfileReflections",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeSpentSeconds",
                table: "DatingProfileReflections",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileReviewCount",
                table: "DatingProfileReflections");

            migrationBuilder.DropColumn(
                name: "TimeSpentSeconds",
                table: "DatingProfileReflections");
        }
    }
}
