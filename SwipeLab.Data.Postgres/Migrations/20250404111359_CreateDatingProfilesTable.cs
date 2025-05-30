using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatingProfilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatingProfiles",
                columns: table => new
                {
                    DatingProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Ethnicity = table.Column<int>(type: "integer", nullable: false),
                    LookingFor = table.Column<int>(type: "integer", nullable: false),
                    Drinking = table.Column<int>(type: "integer", nullable: false),
                    Smoking = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    KidsPreference = table.Column<int>(type: "integer", nullable: false),
                    Education = table.Column<int>(type: "integer", nullable: false),
                    Hobbies = table.Column<List<string>>(type: "text[]", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    RowStatus = table.Column<int>(type: "integer", nullable: false),
                    RowCreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatingProfiles", x => x.DatingProfileId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatingProfiles");
        }
    }
}
