using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Download_Game_GameId",
                table: "Download");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Download",
                table: "Download");

            migrationBuilder.RenameTable(
                name: "Download",
                newName: "Downloads");

            migrationBuilder.RenameIndex(
                name: "IX_Download_GameId",
                table: "Downloads",
                newName: "IX_Downloads_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Downloads",
                table: "Downloads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Downloads_Game_GameId",
                table: "Downloads",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Downloads_Game_GameId",
                table: "Downloads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Downloads",
                table: "Downloads");

            migrationBuilder.RenameTable(
                name: "Downloads",
                newName: "Download");

            migrationBuilder.RenameIndex(
                name: "IX_Downloads_GameId",
                table: "Download",
                newName: "IX_Download_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Download",
                table: "Download",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Download_Game_GameId",
                table: "Download",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
