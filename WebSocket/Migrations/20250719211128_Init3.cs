using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSocket.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserSenderId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserRooms_UserRoomId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserSenderId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "UserRoomId",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserRoomId",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RoomId",
                table: "Messages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RoomId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "UserRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                newName: "IX_Messages_UserRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserSenderId",
                table: "Likes",
                column: "UserSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserSenderId",
                table: "Likes",
                column: "UserSenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserRooms_UserRoomId",
                table: "Messages",
                column: "UserRoomId",
                principalTable: "UserRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
