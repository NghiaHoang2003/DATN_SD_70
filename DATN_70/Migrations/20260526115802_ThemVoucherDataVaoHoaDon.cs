using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_70.Migrations
{
    /// <inheritdoc />
    public partial class ThemVoucherDataVaoHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoucherData",
                table: "HoaDons",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoucherData",
                table: "HoaDons");
        }
    }
}
