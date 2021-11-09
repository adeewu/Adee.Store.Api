using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class CallbackRequest_Remove_Query : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Query",
                table: "CallbackRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Query",
                table: "CallbackRequests",
                type: "longtext",
                nullable: true,
                comment: "请求参数")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
