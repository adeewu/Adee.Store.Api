using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class CallbackRequest_Modify_EnumDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CallbackType",
                table: "CallbackRequests",
                type: "int",
                nullable: false,
                comment: "回调类型；WechatComponentAuthNoity：1[微信第三方平台授权事件]；WechatComponentNotify：2[微信第三方平台授权事件]；PayNotify：3[支付回调通知]",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "回调类型；WechatComponentAuthNoity：WechatComponentAuthNoity,微信第三方平台授权事件；WechatComponentNotify：WechatComponentNotify,微信第三方平台授权事件；PayNotify：PayNotify,支付回调通知");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CallbackType",
                table: "CallbackRequests",
                type: "int",
                nullable: false,
                comment: "回调类型；WechatComponentAuthNoity：WechatComponentAuthNoity,微信第三方平台授权事件；WechatComponentNotify：WechatComponentNotify,微信第三方平台授权事件；PayNotify：PayNotify,支付回调通知",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "回调类型；WechatComponentAuthNoity：1[微信第三方平台授权事件]；WechatComponentNotify：2[微信第三方平台授权事件]；PayNotify：3[支付回调通知]");
        }
    }
}
