using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Experiment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DatingProfileSetId",
                table: "DatingProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserFeedback",
                table: "DatingProfileInteractions",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.CreateTable(
                name: "DatingProfileSets",
                columns: table => new
                {
                    DatingProfileSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowStatus = table.Column<int>(type: "integer", nullable: false),
                    RowCreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatingProfileSets", x => x.DatingProfileSetId);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    ExperimentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowStatus = table.Column<int>(type: "integer", nullable: false),
                    RowCreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.ExperimentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatingProfiles_DatingProfileSetId",
                table: "DatingProfiles",
                column: "DatingProfileSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatingProfiles_DatingProfileSets_DatingProfileSetId",
                table: "DatingProfiles",
                column: "DatingProfileSetId",
                principalTable: "DatingProfileSets",
                principalColumn: "DatingProfileSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatingProfiles_DatingProfileSets_DatingProfileSetId",
                table: "DatingProfiles");

            migrationBuilder.DropTable(
                name: "DatingProfileSets");

            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropIndex(
                name: "IX_DatingProfiles_DatingProfileSetId",
                table: "DatingProfiles");

            migrationBuilder.DropColumn(
                name: "DatingProfileSetId",
                table: "DatingProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserFeedback",
                table: "DatingProfileInteractions",
                type: "jsonb",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
