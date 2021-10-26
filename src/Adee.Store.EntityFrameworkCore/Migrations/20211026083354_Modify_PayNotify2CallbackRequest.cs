using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Modify_PayNotify2CallbackRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "PayNotifys");

            migrationBuilder.DropColumn(
                name: "HashCode",
                table: "PayNotifys");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "PayNotifys");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "PayNotifys");

            migrationBuilder.DropColumn(
                name: "Query",
                table: "PayNotifys");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "PayNotifys");

            migrationBuilder.AddColumn<Guid>(
                name: "CallbackRequestId",
                table: "PayNotifys",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "回调Id",
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "CallbackRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CallbackType = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "请求方式")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false, comment: "通知地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Query = table.Column<string>(type: "longtext", nullable: true, comment: "请求参数")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "longtext", nullable: true, comment: "请求正文")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Header = table.Column<string>(type: "longtext", nullable: true, comment: "请求头部")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashCode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Method、Url、Body、Query、Header经过MD5计算的值")
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
                    table.PrimaryKey("PK_CallbackRequests", x => x.Id);
                },
                comment: "回调请求")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallbackRequests");

            migrationBuilder.DropColumn(
                name: "CallbackRequestId",
                table: "PayNotifys");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "PayNotifys",
                type: "longtext",
                nullable: true,
                comment: "请求正文")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HashCode",
                table: "PayNotifys",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Method、Url、Body、Query经过MD5计算的值")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "PayNotifys",
                type: "longtext",
                nullable: true,
                comment: "请求头部")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "PayNotifys",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "请求方式")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Query",
                table: "PayNotifys",
                type: "longtext",
                nullable: true,
                comment: "请求参数")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "PayNotifys",
                type: "longtext",
                nullable: false,
                comment: "通知地址")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
