using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_70.Migrations
{
    [DbContext(typeof(DATN_70.Data.AppDbContext))]
    [Migration("20260405000000_StorefrontSchema")]
    public partial class StorefrontSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KichCo",
                columns: table => new
                {
                    KichCoID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichCo", x => x.KichCoID);
                });

            migrationBuilder.CreateTable(
                name: "Mau",
                columns: table => new
                {
                    MauID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mau", x => x.MauID);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    HoaDonID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.HoaDonID);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    SanPhamID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.SanPhamID);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietSanPham",
                columns: table => new
                {
                    ChiTietSanPhamID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    GiaNiemYet = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    KichCoID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MauID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SanPhamID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietSanPham", x => x.ChiTietSanPhamID);
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_KichCo_KichCoID",
                        column: x => x.KichCoID,
                        principalTable: "KichCo",
                        principalColumn: "KichCoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_Mau_MauID",
                        column: x => x.MauID,
                        principalTable: "Mau",
                        principalColumn: "MauID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_SanPham_SanPhamID",
                        column: x => x.SanPhamID,
                        principalTable: "SanPham",
                        principalColumn: "SanPhamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiet",
                columns: table => new
                {
                    HoaDonChiTietID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    HoaDonID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ChiTietSanPhamID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonChiTiet", x => x.HoaDonChiTietID);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_ChiTietSanPham_ChiTietSanPhamID",
                        column: x => x.ChiTietSanPhamID,
                        principalTable: "ChiTietSanPham",
                        principalColumn: "ChiTietSanPhamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_HoaDon_HoaDonID",
                        column: x => x.HoaDonID,
                        principalTable: "HoaDon",
                        principalColumn: "HoaDonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_KichCoID",
                table: "ChiTietSanPham",
                column: "KichCoID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_MauID",
                table: "ChiTietSanPham",
                column: "MauID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_SanPhamID",
                table: "ChiTietSanPham",
                column: "SanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_ChiTietSanPhamID",
                table: "HoaDonChiTiet",
                column: "ChiTietSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_HoaDonID",
                table: "HoaDonChiTiet",
                column: "HoaDonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoaDonChiTiet");

            migrationBuilder.DropTable(
                name: "ChiTietSanPham");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "KichCo");

            migrationBuilder.DropTable(
                name: "Mau");

            migrationBuilder.DropTable(
                name: "SanPham");
        }
    }
}
