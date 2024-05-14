using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portrait_forum.Migrations
{
    /// <inheritdoc />
    public partial class boardpostconvoowner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerID",
                table: "Posts",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_OwnerID",
                table: "Conversation",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_OwnerID",
                table: "Boards",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_OwnerID",
                table: "Boards",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversation_Users_OwnerID",
                table: "Conversation",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_OwnerID",
                table: "Posts",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_OwnerID",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversation_Users_OwnerID",
                table: "Conversation");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_OwnerID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_OwnerID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Conversation_OwnerID",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Boards_OwnerID",
                table: "Boards");
        }
    }
}
