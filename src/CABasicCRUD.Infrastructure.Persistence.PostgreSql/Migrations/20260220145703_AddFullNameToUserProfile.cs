using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameToUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserProfiles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FullName", table: "UserProfiles");
        }
    }
}
