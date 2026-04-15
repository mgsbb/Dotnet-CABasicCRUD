using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "UserProfiles",
                newName: "ProfileImageId"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                table: "UserProfiles",
                type: "TEXT",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "GroupTitle",
                table: "Conversations",
                type: "TEXT",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StorageProvider = table.Column<string>(type: "TEXT", nullable: false),
                    StorageKey = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    MediaType = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "PostsMedia",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MediaId = table.Column<Guid>(type: "TEXT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsMedia", x => new { x.PostId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_PostsMedia_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MediaItems");

            migrationBuilder.DropTable(name: "PostsMedia");

            migrationBuilder.DropColumn(name: "CoverImageId", table: "UserProfiles");

            migrationBuilder.DropColumn(name: "GroupTitle", table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "ProfileImageId",
                table: "UserProfiles",
                newName: "ProfileImageUrl"
            );
        }
    }
}
