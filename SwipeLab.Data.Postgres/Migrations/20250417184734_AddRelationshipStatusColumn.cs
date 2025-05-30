using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelationshipStatus",
                table: "Participants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelationshipStatus",
                table: "Participants");
        }
    }
}
