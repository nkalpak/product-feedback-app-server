using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OauthServer.Migrations
{
    public partial class UseBaseEntityProductRequestVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "ProductRequestVote",
                newName: "DateUpdated");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ProductRequestVote",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ProductRequestVote");

            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "ProductRequestVote",
                newName: "Timestamp");
        }
    }
}
