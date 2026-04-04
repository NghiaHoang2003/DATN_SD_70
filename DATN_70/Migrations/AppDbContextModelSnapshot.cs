using System;
using DATN_70.Data;
using DATN_70.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DATN_70.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DATN_70.Data.StorefrontOrder", b =>
                {
                    b.Property<string>("HoaDonID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChiGiaoHang")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("NgayTao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("SYSUTCDATETIME()");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TenKhachHang")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("TongTien")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TrangThai")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("HoaDonID");

                    b.ToTable("HoaDon");
                });

            modelBuilder.Entity("DATN_70.Data.StorefrontOrderItem", b =>
                {
                    b.Property<string>("HoaDonChiTietID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChiTietSanPhamID")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("DonGia")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("HoaDonID")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhTien")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("HoaDonChiTietID");

                    b.HasIndex("ChiTietSanPhamID");

                    b.HasIndex("HoaDonID");

                    b.ToTable("HoaDonChiTiet");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.ChiTietSanPham", b =>
                {
                    b.Property<string>("ChiTietSanPhamID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("GiaNiemYet")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("KichCoID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MauID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("SanPhamID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuongTonKho")
                        .HasColumnType("int")
                        .HasColumnName("SoLuongTon");

                    b.HasKey("ChiTietSanPhamID");

                    b.HasIndex("KichCoID");

                    b.HasIndex("MauID");

                    b.HasIndex("SanPhamID");

                    b.ToTable("ChiTietSanPham");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.KichCo", b =>
                {
                    b.Property<string>("KichCoID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KichCoID");

                    b.ToTable("KichCo");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.Mau", b =>
                {
                    b.Property<string>("MauID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MauID");

                    b.ToTable("Mau");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.SanPham", b =>
                {
                    b.Property<string>("SanPhamID")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SanPhamID");

                    b.ToTable("SanPham");
                });

            modelBuilder.Entity("DATN_70.Data.StorefrontOrderItem", b =>
                {
                    b.HasOne("DATN_70.Models.Entities.ChiTietSanPham", null)
                        .WithMany()
                        .HasForeignKey("ChiTietSanPhamID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DATN_70.Data.StorefrontOrder", "HoaDon")
                        .WithMany("ChiTietHoaDon")
                        .HasForeignKey("HoaDonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDon");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.ChiTietSanPham", b =>
                {
                    b.HasOne("DATN_70.Models.Entities.KichCo", "KichCo")
                        .WithMany("ChiTietSanPhams")
                        .HasForeignKey("KichCoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DATN_70.Models.Entities.Mau", "Mau")
                        .WithMany("ChiTietSanPhams")
                        .HasForeignKey("MauID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DATN_70.Models.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietSanPhams")
                        .HasForeignKey("SanPhamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KichCo");

                    b.Navigation("Mau");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.KichCo", b =>
                {
                    b.Navigation("ChiTietSanPhams");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.Mau", b =>
                {
                    b.Navigation("ChiTietSanPhams");
                });

            modelBuilder.Entity("DATN_70.Models.Entities.SanPham", b =>
                {
                    b.Navigation("ChiTietSanPhams");
                });
#pragma warning restore 612, 618
        }
    }
}
