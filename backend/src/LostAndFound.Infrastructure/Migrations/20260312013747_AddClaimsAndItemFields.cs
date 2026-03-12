using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostAndFound.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimsAndItemFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Items",
                newName: "LocationFound");

            migrationBuilder.RenameColumn(
                name: "DateReported",
                table: "Items",
                newName: "DateFound");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FeatureDescription = table.Column<string>(type: "text", nullable: false),
                    LocationLost = table.Column<string>(type: "text", nullable: false),
                    TimeLost = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateResolved = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ItemId",
                table: "Claims",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UserId",
                table: "Claims",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "LocationFound",
                table: "Items",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "DateFound",
                table: "Items",
                newName: "DateReported");
        }
    }
}
