using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Experiments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DatingProfileReflections",
                columns: table => new
                {
                    DatingProfileReflectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DatingProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedOpinion = table.Column<bool>(type: "boolean", nullable: false),
                    PromptAnswers = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    RowStatus = table.Column<int>(type: "integer", nullable: false),
                    RowCreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowLastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatingProfileReflections", x => x.DatingProfileReflectionId);
                    table.ForeignKey(
                        name: "FK_DatingProfileReflections_DatingProfiles_DatingProfileId",
                        column: x => x.DatingProfileId,
                        principalTable: "DatingProfiles",
                        principalColumn: "DatingProfileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatingProfileReflections_DatingProfileId",
                table: "DatingProfileReflections",
                column: "DatingProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatingProfileReflections");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Experiments");
        }
    }
}
