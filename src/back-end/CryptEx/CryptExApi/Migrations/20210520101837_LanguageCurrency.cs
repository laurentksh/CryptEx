using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptExApi.Migrations
{
    public partial class LanguageCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Deposits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PreferedLanguage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_AspNetUsers_UserId",
                table: "Deposits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_AspNetUsers_UserId",
                table: "Deposits");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "PreferedLanguage",
                table: "AspNetUsers");
        }
    }
}
