using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OauthServer.Migrations
{
    public partial class AddProductRequestVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductRequestVote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ProductRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Upvote = table.Column<bool>(type: "boolean", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequestVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRequestVote_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRequestVote_ProductRequest_ProductRequestId",
                        column: x => x.ProductRequestId,
                        principalTable: "ProductRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequestVote_ProductRequestId",
                table: "ProductRequestVote",
                column: "ProductRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequestVote_UserId",
                table: "ProductRequestVote",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRequestVote");
        }
    }
}
