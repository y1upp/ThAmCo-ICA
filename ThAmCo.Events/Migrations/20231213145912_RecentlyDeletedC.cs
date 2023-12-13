using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThAmCo.Events.Migrations
{
    /// <inheritdoc />
    public partial class RecentlyDeletedC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RecentlyDeleted",
                table: "Guests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecentlyDeleted",
                table: "Guests");
        }
    }
}
