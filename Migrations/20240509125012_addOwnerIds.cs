using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portrait_forum.Migrations
{
    /// <inheritdoc />
    public partial class addOwnerIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OwnerID",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OwnerID",
                table: "Conversation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OwnerID",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Boards");
        }
    }
}
