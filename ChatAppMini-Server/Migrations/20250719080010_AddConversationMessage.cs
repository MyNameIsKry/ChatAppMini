using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppMini.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "Message",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationUser",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationUser", x => new { x.UserId, x.ConversationId });
                    table.ForeignKey(
                        name: "FK_ConversationUser_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_ConversationId",
                table: "Message",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUser_ConversationId",
                table: "ConversationUser",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Conversation_ConversationId",
                table: "Message",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Conversation_ConversationId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "ConversationUser");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ConversationId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Message");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");
        }
    }
}
