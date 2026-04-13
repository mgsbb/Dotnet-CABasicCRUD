using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CoverImageUrl", table: "UserProfiles");

            migrationBuilder.DropColumn(name: "ProfileImageUrl", table: "UserProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                table: "UserProfiles",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageId",
                table: "UserProfiles",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageProvider = table.Column<string>(type: "text", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    MediaType = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MediaItems");

            migrationBuilder.DropColumn(name: "CoverImageId", table: "UserProfiles");

            migrationBuilder.DropColumn(name: "ProfileImageId", table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "UserProfiles",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "UserProfiles",
                type: "text",
                nullable: true
            );
        }
    }
}
