using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Modify_PayNotify_PayOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PayOrderId",
                table: "PayNotifys",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "支付订单Id",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "支付订单Id")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PayOrderId",
                table: "PayNotifys",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "支付订单Id",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "支付订单Id")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
