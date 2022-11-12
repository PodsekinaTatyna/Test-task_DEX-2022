using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelsDb.Migrations
{
    public partial class ChangedTheNameInTheTableFromAdToAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ads_users_user_id",
                table: "ads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ads",
                table: "ads");

            migrationBuilder.RenameTable(
                name: "ads",
                newName: "announcements");

            migrationBuilder.RenameIndex(
                name: "IX_ads_user_id",
                table: "announcements",
                newName: "IX_announcements_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_announcements",
                table: "announcements",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_announcements_users_user_id",
                table: "announcements",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_announcements_users_user_id",
                table: "announcements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_announcements",
                table: "announcements");

            migrationBuilder.RenameTable(
                name: "announcements",
                newName: "ads");

            migrationBuilder.RenameIndex(
                name: "IX_announcements_user_id",
                table: "ads",
                newName: "IX_ads_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ads",
                table: "ads",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ads_users_user_id",
                table: "ads",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
