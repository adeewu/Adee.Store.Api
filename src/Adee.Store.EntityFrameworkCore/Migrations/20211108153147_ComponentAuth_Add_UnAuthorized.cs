using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class ComponentAuth_Add_UnAuthorized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UnAuthorized",
                table: "WechatComponentAuths",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "取消授权");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnAuthorized",
                table: "WechatComponentAuths");
        }
    }
}
