using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class PayNotify_Add_PayOrganizationOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayOrganizationOrderId",
                table: "PayNotifys",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                comment: "收单机构订单号")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayOrganizationOrderId",
                table: "PayNotifys");
        }
    }
}
