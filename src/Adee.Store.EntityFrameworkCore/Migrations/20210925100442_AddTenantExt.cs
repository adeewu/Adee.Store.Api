using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class AddTenantExt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderInfo_Order_OrderId",
                table: "OrderInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_PayOrderLog_PayOrder_OrderId",
                table: "PayOrderLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PayRefund_PayOrder_OrderId",
                table: "PayRefund");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCatalog_CatalogId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSaleInfo_ProductSale_ProductSaleId",
                table: "ProductSaleInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSaleInfo_ProductStock_ProductStockId",
                table: "ProductSaleInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStock_Product_ProductId",
                table: "ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStockLog_ProductStock_ProductStockId",
                table: "ProductStockLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStockOrder",
                table: "ProductStockOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStockLog",
                table: "ProductStockLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStock",
                table: "ProductStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSaleInfo",
                table: "ProductSaleInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSale",
                table: "ProductSale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCatalog",
                table: "ProductCatalog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayRefund",
                table: "PayRefund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayParameter",
                table: "PayParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayOrderLog",
                table: "PayOrderLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayOrder",
                table: "PayOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayNotify",
                table: "PayNotify");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderInfo",
                table: "OrderInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TargetDomain",
                table: "PayOrder");

            migrationBuilder.RenameTable(
                name: "ProductStockOrder",
                newName: "ProductStockOrders");

            migrationBuilder.RenameTable(
                name: "ProductStockLog",
                newName: "ProductStockLogs");

            migrationBuilder.RenameTable(
                name: "ProductStock",
                newName: "ProductStocks");

            migrationBuilder.RenameTable(
                name: "ProductSaleInfo",
                newName: "ProductSaleInfos");

            migrationBuilder.RenameTable(
                name: "ProductSale",
                newName: "ProductSales");

            migrationBuilder.RenameTable(
                name: "ProductCatalog",
                newName: "ProductCatalogs");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "PayRefund",
                newName: "PayRefunds");

            migrationBuilder.RenameTable(
                name: "PayParameter",
                newName: "PayParameters");

            migrationBuilder.RenameTable(
                name: "PayOrderLog",
                newName: "PayOrderLogs");

            migrationBuilder.RenameTable(
                name: "PayOrder",
                newName: "PayOrders");

            migrationBuilder.RenameTable(
                name: "PayNotify",
                newName: "PayNotifys");

            migrationBuilder.RenameTable(
                name: "OrderInfo",
                newName: "OrderInfos");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_ProductStockLog_ProductStockId",
                table: "ProductStockLogs",
                newName: "IX_ProductStockLogs_ProductStockId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductStock_ProductId",
                table: "ProductStocks",
                newName: "IX_ProductStocks_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSaleInfo_ProductStockId",
                table: "ProductSaleInfos",
                newName: "IX_ProductSaleInfos_ProductStockId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSaleInfo_ProductSaleId",
                table: "ProductSaleInfos",
                newName: "IX_ProductSaleInfos_ProductSaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CatalogId",
                table: "Products",
                newName: "IX_Products_CatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PayRefund_OrderId",
                table: "PayRefunds",
                newName: "IX_PayRefunds_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PayOrderLog_OrderId",
                table: "PayOrderLogs",
                newName: "IX_PayOrderLogs_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderInfo_OrderId",
                table: "OrderInfos",
                newName: "IX_OrderInfos_OrderId");

            migrationBuilder.AlterTable(
                name: "OrderInfos",
                comment: "订单详情")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "Orders",
                comment: "订单")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStockOrders",
                table: "ProductStockOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStockLogs",
                table: "ProductStockLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStocks",
                table: "ProductStocks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSaleInfos",
                table: "ProductSaleInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSales",
                table: "ProductSales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCatalogs",
                table: "ProductCatalogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayRefunds",
                table: "PayRefunds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayParameters",
                table: "PayParameters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayOrderLogs",
                table: "PayOrderLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayOrders",
                table: "PayOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayNotifys",
                table: "PayNotifys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderInfos",
                table: "OrderInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AbpTenantExts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SoftwareCode = table.Column<string>(type: "longtext", nullable: true, comment: "软件编号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaypameterVersion = table.Column<long>(type: "bigint", nullable: true, comment: "支付参数版本"),
                    PayOrganizationType = table.Column<int>(type: "int", nullable: true, comment: "支付收单机构"),
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
                    table.PrimaryKey("PK_AbpTenantExts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderInfos_Orders_OrderId",
                table: "OrderInfos",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayOrderLogs_PayOrders_OrderId",
                table: "PayOrderLogs",
                column: "OrderId",
                principalTable: "PayOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayRefunds_PayOrders_OrderId",
                table: "PayRefunds",
                column: "OrderId",
                principalTable: "PayOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCatalogs_CatalogId",
                table: "Products",
                column: "CatalogId",
                principalTable: "ProductCatalogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSaleInfos_ProductSales_ProductSaleId",
                table: "ProductSaleInfos",
                column: "ProductSaleId",
                principalTable: "ProductSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSaleInfos_ProductStocks_ProductStockId",
                table: "ProductSaleInfos",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStockLogs_ProductStocks_ProductStockId",
                table: "ProductStockLogs",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStocks_Products_ProductId",
                table: "ProductStocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderInfos_Orders_OrderId",
                table: "OrderInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_PayOrderLogs_PayOrders_OrderId",
                table: "PayOrderLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_PayRefunds_PayOrders_OrderId",
                table: "PayRefunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCatalogs_CatalogId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSaleInfos_ProductSales_ProductSaleId",
                table: "ProductSaleInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSaleInfos_ProductStocks_ProductStockId",
                table: "ProductSaleInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStockLogs_ProductStocks_ProductStockId",
                table: "ProductStockLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStocks_Products_ProductId",
                table: "ProductStocks");

            migrationBuilder.DropTable(
                name: "AbpTenantExts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStocks",
                table: "ProductStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStockOrders",
                table: "ProductStockOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStockLogs",
                table: "ProductStockLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSales",
                table: "ProductSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSaleInfos",
                table: "ProductSaleInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCatalogs",
                table: "ProductCatalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayRefunds",
                table: "PayRefunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayParameters",
                table: "PayParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayOrders",
                table: "PayOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayOrderLogs",
                table: "PayOrderLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayNotifys",
                table: "PayNotifys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderInfos",
                table: "OrderInfos");

            migrationBuilder.RenameTable(
                name: "ProductStocks",
                newName: "ProductStock");

            migrationBuilder.RenameTable(
                name: "ProductStockOrders",
                newName: "ProductStockOrder");

            migrationBuilder.RenameTable(
                name: "ProductStockLogs",
                newName: "ProductStockLog");

            migrationBuilder.RenameTable(
                name: "ProductSales",
                newName: "ProductSale");

            migrationBuilder.RenameTable(
                name: "ProductSaleInfos",
                newName: "ProductSaleInfo");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "ProductCatalogs",
                newName: "ProductCatalog");

            migrationBuilder.RenameTable(
                name: "PayRefunds",
                newName: "PayRefund");

            migrationBuilder.RenameTable(
                name: "PayParameters",
                newName: "PayParameter");

            migrationBuilder.RenameTable(
                name: "PayOrders",
                newName: "PayOrder");

            migrationBuilder.RenameTable(
                name: "PayOrderLogs",
                newName: "PayOrderLog");

            migrationBuilder.RenameTable(
                name: "PayNotifys",
                newName: "PayNotify");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderInfos",
                newName: "OrderInfo");

            migrationBuilder.RenameIndex(
                name: "IX_ProductStocks_ProductId",
                table: "ProductStock",
                newName: "IX_ProductStock_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductStockLogs_ProductStockId",
                table: "ProductStockLog",
                newName: "IX_ProductStockLog_ProductStockId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSaleInfos_ProductStockId",
                table: "ProductSaleInfo",
                newName: "IX_ProductSaleInfo_ProductStockId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSaleInfos_ProductSaleId",
                table: "ProductSaleInfo",
                newName: "IX_ProductSaleInfo_ProductSaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CatalogId",
                table: "Product",
                newName: "IX_Product_CatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_PayRefunds_OrderId",
                table: "PayRefund",
                newName: "IX_PayRefund_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PayOrderLogs_OrderId",
                table: "PayOrderLog",
                newName: "IX_PayOrderLog_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderInfos_OrderId",
                table: "OrderInfo",
                newName: "IX_OrderInfo_OrderId");

            migrationBuilder.AlterTable(
                name: "Order",
                oldComment: "订单")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "OrderInfo",
                oldComment: "订单详情")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TargetDomain",
                table: "PayOrder",
                type: "varchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                comment: "发起支付域名")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStock",
                table: "ProductStock",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStockOrder",
                table: "ProductStockOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStockLog",
                table: "ProductStockLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSale",
                table: "ProductSale",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSaleInfo",
                table: "ProductSaleInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCatalog",
                table: "ProductCatalog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayRefund",
                table: "PayRefund",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayParameter",
                table: "PayParameter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayOrder",
                table: "PayOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayOrderLog",
                table: "PayOrderLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayNotify",
                table: "PayNotify",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderInfo",
                table: "OrderInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderInfo_Order_OrderId",
                table: "OrderInfo",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayOrderLog_PayOrder_OrderId",
                table: "PayOrderLog",
                column: "OrderId",
                principalTable: "PayOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayRefund_PayOrder_OrderId",
                table: "PayRefund",
                column: "OrderId",
                principalTable: "PayOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCatalog_CatalogId",
                table: "Product",
                column: "CatalogId",
                principalTable: "ProductCatalog",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSaleInfo_ProductSale_ProductSaleId",
                table: "ProductSaleInfo",
                column: "ProductSaleId",
                principalTable: "ProductSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSaleInfo_ProductStock_ProductStockId",
                table: "ProductSaleInfo",
                column: "ProductStockId",
                principalTable: "ProductStock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStock_Product_ProductId",
                table: "ProductStock",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStockLog_ProductStock_ProductStockId",
                table: "ProductStockLog",
                column: "ProductStockId",
                principalTable: "ProductStock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
