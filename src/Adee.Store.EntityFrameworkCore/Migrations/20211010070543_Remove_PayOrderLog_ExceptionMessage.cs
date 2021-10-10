using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Remove_PayOrderLog_ExceptionMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "PayOrderLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "PayOrderLogs",
                type: "longtext",
                nullable: true,
                comment: "异常描述")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
