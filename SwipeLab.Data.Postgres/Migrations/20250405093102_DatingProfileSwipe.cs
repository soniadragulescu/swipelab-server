using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeLab.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class DatingProfileSwipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatingProfileInteractions");

            migrationBuilder.CreateTable(
                name: "DatingProfileSwipes",
                columns: table => new
                {
                    DatingProfileSwipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DatingProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    SwipeState = table.Column<int>(type: "integer", nullable: false),
                    TimeSpentSeconds = table.Column<int>(type: "integer", nullable: false),
                    RowStatus = table.Column<int>(type: "integer", nullable: false),
                    RowCreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowLastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatingProfileSwipes", x => x.DatingProfileSwipeId);
                    table.ForeignKey(
                        name: "FK_DatingProfileSwipes_DatingProfiles_DatingProfileId",
                        column: x => x.DatingProfileId,
                        principalTable: "DatingProfiles",
                        principalColumn: "DatingProfileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatingProfileSwipes_DatingProfileId",
                table: "DatingProfileSwipes",
                column: "DatingProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatingProfileSwipes");

            migrationBuilder.CreateTable(
                name: "DatingProfileInteractions",
                columns: table => new
                {
                    DatingProfileInteractionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DatingProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    HasChangedItsMind = table.Column<bool>(type: "boolean", nullable: false),
                    RowCreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowLastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowStatus = table.Column<int>(type: "integer", nullable: false),
                    SwipeState = table.Column<int>(type: "integer", nullable: false),
                    TimeSpentSeconds = table.Column<int>(type: "integer", nullable: false),
                    UserFeedback = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatingProfileInteractions", x => x.DatingProfileInteractionId);
                    table.ForeignKey(
                        name: "FK_DatingProfileInteractions_DatingProfiles_DatingProfileId",
                        column: x => x.DatingProfileId,
                        principalTable: "DatingProfiles",
                        principalColumn: "DatingProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatingProfileInteractions_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatingProfileInteractions_DatingProfileId",
                table: "DatingProfileInteractions",
                column: "DatingProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DatingProfileInteractions_ParticipantId",
                table: "DatingProfileInteractions",
                column: "ParticipantId");
        }
    }
}
