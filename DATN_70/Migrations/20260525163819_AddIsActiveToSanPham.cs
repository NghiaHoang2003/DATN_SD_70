using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_70.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToSanPham : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SanPhams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SanPhams");
        }
    }
}
