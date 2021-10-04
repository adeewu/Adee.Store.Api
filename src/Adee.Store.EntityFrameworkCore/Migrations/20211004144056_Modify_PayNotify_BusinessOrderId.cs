using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Modify_PayNotify_BusinessOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchantOrderId",
                table: "PayNotifys");

            migrationBuilder.DropColumn(
                name: "PayOrganizationType",
                table: "AbpTenantExts");

            migrationBuilder.AddColumn<string>(
                name: "BusinessOrderId",
                table: "PayNotifys",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "业务订单号")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessOrderId",
                table: "PayNotifys");

            migrationBuilder.AddColumn<string>(
                name: "MerchantOrderId",
                table: "PayNotifys",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "支付结果查询Id")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PayOrganizationType",
                table: "AbpTenantExts",
                type: "int",
                nullable: true,
                comment: "支付收单机构");
        }
    }
}
