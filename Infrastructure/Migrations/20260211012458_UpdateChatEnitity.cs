using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatEnitity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_FirstUserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_SecondUserId",
                table: "ChatMessages");

            migrationBuilder.RenameColumn(
                name: "SecondUserId",
                table: "ChatMessages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "FirstUserId",
                table: "ChatMessages",
                newName: "ReciverId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_SecondUserId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_FirstUserId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ReciverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_ReciverId",
                table: "ChatMessages",
                column: "ReciverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_SenderId",
                table: "ChatMessages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_ReciverId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_SenderId",
                table: "ChatMessages");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "ChatMessages",
                newName: "SecondUserId");

            migrationBuilder.RenameColumn(
                name: "ReciverId",
                table: "ChatMessages",
                newName: "FirstUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_SecondUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ReciverId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_FirstUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_FirstUserId",
                table: "ChatMessages",
                column: "FirstUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_SecondUserId",
                table: "ChatMessages",
                column: "SecondUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
