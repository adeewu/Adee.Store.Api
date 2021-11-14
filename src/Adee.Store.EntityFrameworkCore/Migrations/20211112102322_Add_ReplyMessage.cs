using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Adee.Store.Migrations
{
    public partial class Add_ReplyMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WechatOffiAccountReplyMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "租户Id", collation: "ascii_general_ci"),
                    Keyword = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "关键字")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatchType = table.Column<int>(type: "int", nullable: false, comment: "匹配类型；Default：1[租户默认]；Full：2[全匹配]；StartLike：3[开头匹配]；EndLike：4[结尾匹配]；Like：5[模糊匹配]"),
                    MessageType = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, comment: "消息类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageContent = table.Column<string>(type: "longtext", nullable: true, comment: "消息内容")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "禁用"),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WechatOffiAccountReplyMessages", x => x.Id);
                },
                comment: "公众平台被动回复消息")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WechatOffiAccountReplyMessages");
        }
    }
}
