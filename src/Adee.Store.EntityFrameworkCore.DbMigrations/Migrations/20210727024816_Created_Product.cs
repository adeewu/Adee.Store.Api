using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adee.Store.Migrations
{
    public partial class Created_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    RunningId = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "订单流水号"),
                    MerchantOrderId = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "支付业务号"),
                    Title = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: false, comment: "订单标题"),
                    BusinessType = table.Column<int>(type: "int", nullable: false, comment: "业务类型"),
                    TerminalType = table.Column<int>(type: "int", nullable: false, comment: "终端类型"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "销售单价，单一商品有效"),
                    Quantity = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "销售数量"),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "销售金额"),
                    Payment = table.Column<string>(type: "varchar(20) CHARACTER SET utf8mb4", maxLength: 20, nullable: true, comment: "收款方式"),
                    PayTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "收款时间"),
                    PayStatus = table.Column<int>(type: "int", nullable: false, comment: "收款状态"),
                    OrderTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "订单时间"),
                    OrderStatus = table.Column<int>(type: "int", nullable: false, comment: "订单状态"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPayNotify",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Method = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: false, comment: "请求方式"),
                    Url = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false, comment: "通知地址"),
                    Body = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "请求正文"),
                    Query = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "请求参数"),
                    HashCode = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: false, comment: "Method、Url、Body、Query经过MD5计算的值"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "通知执行状态"),
                    StatusMessage = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "通知状态状态描述"),
                    MerchantOrderId = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "支付结果查询Id"),
                    PayOrderId = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "支付订单Id"),
                    ResultStatus = table.Column<int>(type: "int", nullable: false, comment: "通知内容状态"),
                    ResultStatusMessage = table.Column<string>(type: "varchar(500) CHARACTER SET utf8mb4", maxLength: 500, nullable: true, comment: "通知内容状态描述"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPayNotify", x => x.Id);
                },
                comment: "支付回调通知");

            migrationBuilder.CreateTable(
                name: "AppPayOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    MerchantOrderId = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "支付结果查询Id"),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "收款金额"),
                    PayOrderId = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "支付Id"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "支付状态"),
                    ParameterVersion = table.Column<int>(type: "int", nullable: false, comment: "支付参数版本"),
                    PaymentType = table.Column<int>(type: "int", nullable: false, comment: "付款方式"),
                    PayOrganizationType = table.Column<int>(type: "int", nullable: false, comment: "收单机构"),
                    StatusMessage = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "支付状态描述"),
                    OrderTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "订单时间"),
                    QueryStatus = table.Column<int>(type: "int", nullable: false, comment: "查询状态"),
                    NotifyStatus = table.Column<int>(type: "int", nullable: false, comment: "通知状态"),
                    QueryStatusMessage = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "查询状态描述"),
                    NotifyStatusMessage = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "通知状态描述"),
                    PaymethodType = table.Column<int>(type: "int", nullable: false, comment: "支付方式"),
                    OrderData = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "订单数据"),
                    RefundStatus = table.Column<int>(type: "int", nullable: true, comment: "退款状态"),
                    RefundStatusMessage = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "退款状态描述"),
                    RefundCount = table.Column<int>(type: "int", nullable: true, comment: "成功退款次数"),
                    NotifyUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false, comment: "通知地址"),
                    TargetDomain = table.Column<string>(type: "varchar(1024) CHARACTER SET utf8mb4", maxLength: 1024, nullable: false, comment: "发起支付域名"),
                    BusinessType = table.Column<int>(type: "int", nullable: false, comment: "业务模块类型"),
                    Subject = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: false, comment: "收款标题"),
                    PayOrganizationOrderId = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: true, comment: "收单机构订单号"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPayOrder", x => x.Id);
                },
                comment: "支付订单");

            migrationBuilder.CreateTable(
                name: "AppPayParameter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false, comment: "版本"),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false, comment: "支付参数值"),
                    PaymentType = table.Column<int>(type: "int", nullable: true, comment: "付款方式"),
                    PayOrganizationType = table.Column<int>(type: "int", nullable: false, comment: "收单机构"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPayParameter", x => x.Id);
                },
                comment: "支付参数");

            migrationBuilder.CreateTable(
                name: "AppPaySecrect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    SignKey = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "密钥"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPaySecrect", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProductCatalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Name = table.Column<string>(type: "varchar(20) CHARACTER SET utf8mb4", maxLength: 20, nullable: true, comment: "分类名称"),
                    CatalogPath = table.Column<string>(type: "varchar(1000) CHARACTER SET utf8mb4", maxLength: 1000, nullable: true, comment: "分类路径，计算值"),
                    ParentCatalogId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "父分类Id"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductCatalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProductSale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Title = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: false, comment: "售卖标题"),
                    Recommend = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "推荐售卖"),
                    MarketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "市场价"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "售卖单价"),
                    SaleVolume = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "售卖量"),
                    TotalSaleVolume = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "总售卖量"),
                    AllowOversell = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "允许超售"),
                    Discount = table.Column<int>(type: "int", nullable: true, comment: "折扣，百分比"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "状态，1：正常，-1：下架"),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "商品描述"),
                    Photo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "图片"),
                    ProductSaleType = table.Column<int>(type: "int", nullable: false, comment: "售卖类型"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductSale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProductStockOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    BatchNo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false, comment: "库存批次号"),
                    Supplier = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false, comment: "供应商"),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "订单金额"),
                    ActualMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "实付金额"),
                    Payment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false, comment: "付款方式"),
                    Remark = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "备注"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductStockOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppOrderInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "订单Id"),
                    OrderType = table.Column<int>(type: "int", nullable: false, comment: "订单类型"),
                    DataId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "数据Id"),
                    DataName = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "数据名称"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "成交单价"),
                    Quantity = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "成交数量"),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "成交总价"),
                    Desc = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "成交描述"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrderInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOrderInfo_AppOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "AppOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPayOrderLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    OrderId = table.Column<Guid>(type: "char(50)", maxLength: 50, nullable: false, comment: "订单Id"),
                    Message = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "描述"),
                    ExceptionMessage = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "异常描述"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "记录状态"),
                    LogType = table.Column<int>(type: "int", nullable: false, comment: "记录类型"),
                    LogData = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "记录数据"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPayOrderLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPayOrderLog_AppPayOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "AppPayOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "支付订单记录");

            migrationBuilder.CreateTable(
                name: "AppPayRefund",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    OrderId = table.Column<Guid>(type: "char(50)", maxLength: 50, nullable: false, comment: "订单Id"),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "退款金额"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "退款状态"),
                    StatusMessage = table.Column<string>(type: "varchar(500) CHARACTER SET utf8mb4", maxLength: 500, nullable: true, comment: "退款状态描述"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPayRefund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPayRefund_AppPayOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "AppPayOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "支付订单退款记录");

            migrationBuilder.CreateTable(
                name: "AppProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CatalogId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品分类"),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "名称"),
                    BarCode = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "条码"),
                    Photo = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true, comment: "图片"),
                    PricingType = table.Column<int>(type: "int", nullable: false, comment: "计价方式，1：计件，2：计重"),
                    QuickCode = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "助记码"),
                    ProductBrand = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "品牌"),
                    UnitName = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "单位名称"),
                    Specs = table.Column<string>(type: "varchar(2000) CHARACTER SET utf8mb4", maxLength: 2000, nullable: true, comment: "商品规格，单规格商品留空"),
                    Stock = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "库存，冗余值"),
                    SaleVolume = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "销量，冗余值"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProduct_AppProductCatalog_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "AppProductCatalog",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppProductStock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "商品Id"),
                    Spec = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true, comment: "商品规格"),
                    Quantity = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "库存"),
                    Warranty = table.Column<decimal>(type: "decimal(9,2)", nullable: true, comment: "质保期"),
                    WarrantyUnit = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true, comment: "质保期单位"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProductStock_AppProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "AppProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProductSaleInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProductStockId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "库存Id"),
                    ProductSaleId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "商品售卖Id"),
                    Quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false, comment: "售卖量")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductSaleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProductSaleInfo_AppProductSale_ProductSaleId",
                        column: x => x.ProductSaleId,
                        principalTable: "AppProductSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProductSaleInfo_AppProductStock_ProductStockId",
                        column: x => x.ProductStockId,
                        principalTable: "AppProductStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProductStockLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProductStockId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "库存Id"),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "进货价"),
                    Quantity = table.Column<decimal>(type: "decimal(9,2)", nullable: false, comment: "进货量"),
                    BatchNo = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false, comment: "库存批次号"),
                    OriginPlace = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true, comment: "原产地"),
                    Source = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true, comment: "库存来源"),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductStockLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProductStockLog_AppProductStock_ProductStockId",
                        column: x => x.ProductStockId,
                        principalTable: "AppProductStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrderInfo_OrderId",
                table: "AppOrderInfo",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPayOrderLog_OrderId",
                table: "AppPayOrderLog",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPayRefund_OrderId",
                table: "AppPayRefund",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProduct_CatalogId",
                table: "AppProduct",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductSaleInfo_ProductSaleId",
                table: "AppProductSaleInfo",
                column: "ProductSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductSaleInfo_ProductStockId",
                table: "AppProductSaleInfo",
                column: "ProductStockId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductStock_ProductId",
                table: "AppProductStock",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductStockLog_ProductStockId",
                table: "AppProductStockLog",
                column: "ProductStockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppOrderInfo");

            migrationBuilder.DropTable(
                name: "AppPayNotify");

            migrationBuilder.DropTable(
                name: "AppPayOrderLog");

            migrationBuilder.DropTable(
                name: "AppPayParameter");

            migrationBuilder.DropTable(
                name: "AppPayRefund");

            migrationBuilder.DropTable(
                name: "AppPaySecrect");

            migrationBuilder.DropTable(
                name: "AppProductSaleInfo");

            migrationBuilder.DropTable(
                name: "AppProductStockLog");

            migrationBuilder.DropTable(
                name: "AppProductStockOrder");

            migrationBuilder.DropTable(
                name: "AppOrder");

            migrationBuilder.DropTable(
                name: "AppPayOrder");

            migrationBuilder.DropTable(
                name: "AppProductSale");

            migrationBuilder.DropTable(
                name: "AppProductStock");

            migrationBuilder.DropTable(
                name: "AppProduct");

            migrationBuilder.DropTable(
                name: "AppProductCatalog");
        }
    }
}
