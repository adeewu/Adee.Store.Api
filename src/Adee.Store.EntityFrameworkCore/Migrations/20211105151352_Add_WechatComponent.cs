using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Add_WechatComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WechatComponentAuths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ComponentAppId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "第三方平台AppId")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthAppId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "授权AppId")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthorizationCode = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "授权码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthorizerRefreshToken = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "刷新令牌")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FuncInfo = table.Column<string>(type: "longtext", nullable: true, comment: "授权权限集")
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    table.PrimaryKey("PK_WechatComponentAuths", x => x.Id);
                },
                comment: "第三方平台授权")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WechatComponentAuths");
        }
    }
}
