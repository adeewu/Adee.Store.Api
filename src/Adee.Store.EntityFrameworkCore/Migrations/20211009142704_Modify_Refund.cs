using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Modify_Refund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotifyStatus",
                table: "PayRefunds",
                type: "int",
                nullable: true,
                comment: "退款通知状态");

            migrationBuilder.AddColumn<string>(
                name: "NotifyStatusMessage",
                table: "PayRefunds",
                type: "longtext",
                nullable: true,
                comment: "退款通知状态描述")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "QueryStatus",
                table: "PayRefunds",
                type: "int",
                nullable: true,
                comment: "查询退款状态");

            migrationBuilder.AddColumn<string>(
                name: "QueryStatusMessage",
                table: "PayRefunds",
                type: "longtext",
                nullable: true,
                comment: "查询退款状态描述")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyStatus",
                table: "PayRefunds");

            migrationBuilder.DropColumn(
                name: "NotifyStatusMessage",
                table: "PayRefunds");

            migrationBuilder.DropColumn(
                name: "QueryStatus",
                table: "PayRefunds");

            migrationBuilder.DropColumn(
                name: "QueryStatusMessage",
                table: "PayRefunds");
        }
    }
}
