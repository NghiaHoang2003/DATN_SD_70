using DATN_70.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<SanPham> SanPhams => Set<SanPham>();
        public DbSet<ChiTietSanPham> ChiTietSanPhams => Set<ChiTietSanPham>();
        public DbSet<Mau> Maus => Set<Mau>();
        public DbSet<KichCo> KichCos => Set<KichCo>();
        public DbSet<StorefrontOrder> StorefrontOrders => Set<StorefrontOrder>();
        public DbSet<StorefrontOrderItem> StorefrontOrderItems => Set<StorefrontOrderItem>();
        public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
        public DbSet<VaiTro> VaiTros => Set<VaiTro>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Ignore<TaiKhoan>();
            //modelBuilder.Ignore<VaiTro>();
            modelBuilder.Ignore<KhachHang>();
            modelBuilder.Ignore<NhanVien>();
            modelBuilder.Ignore<DiaChi>();
            modelBuilder.Ignore<DanhMuc>();
            modelBuilder.Ignore<ThuongHieu>();
            modelBuilder.Ignore<GioHang>();
            modelBuilder.Ignore<ChiTietGioHang>();
            modelBuilder.Ignore<KhuyenMai>();
            modelBuilder.Ignore<KhuyenMaiSanPham>();
            modelBuilder.Ignore<PhuongThucThanhToan>();
            modelBuilder.Ignore<ChiTietThanhToan>();
            modelBuilder.Ignore<HoaDon>();
            modelBuilder.Ignore<HoaDonChiTiet>();

            modelBuilder.Entity<KichCo>(entity =>
            {
                entity.ToTable("KichCo");
                entity.HasKey(x => x.KichCoID);

                entity.Property(x => x.KichCoID).HasMaxLength(450);
                entity.Property(x => x.Ten).IsRequired();
                entity.Property(x => x.MoTa).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<Mau>(entity =>
            {
                entity.ToTable("Mau");
                entity.HasKey(x => x.MauID);

                entity.Property(x => x.MauID).HasMaxLength(450);
                entity.Property(x => x.Ten).IsRequired();
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPham");
                entity.HasKey(x => x.SanPhamID);

                entity.Property(x => x.SanPhamID).HasMaxLength(450);
                entity.Property(x => x.Ten).IsRequired();
                entity.Property(x => x.MoTa).HasDefaultValue(string.Empty);

                entity.Ignore(x => x.MucVAT);
                entity.Ignore(x => x.ChatLieu);
                entity.Ignore(x => x.ThuongHieuID);
                entity.Ignore(x => x.DanhMucID);
                entity.Ignore(x => x.ThuongHieu);
                entity.Ignore(x => x.DanhMuc);
                entity.Ignore(x => x.KhuyenMaiSanPhams);
            });

            modelBuilder.Entity<ChiTietSanPham>(entity =>
            {
                entity.ToTable("ChiTietSanPham");
                entity.HasKey(x => x.ChiTietSanPhamID);

                entity.Property(x => x.ChiTietSanPhamID).HasMaxLength(450);
                entity.Property(x => x.SoLuongTonKho).HasColumnName("SoLuongTon");
                entity.Property(x => x.SKU).HasDefaultValue(string.Empty);
                entity.Property(x => x.GiaNiemYet).HasPrecision(18, 2);

                entity.HasOne(x => x.SanPham)
                    .WithMany(x => x.ChiTietSanPhams)
                    .HasForeignKey(x => x.SanPhamID);

                entity.HasOne(x => x.KichCo)
                    .WithMany(x => x.ChiTietSanPhams)
                    .HasForeignKey(x => x.KichCoID);

                entity.HasOne(x => x.Mau)
                    .WithMany(x => x.ChiTietSanPhams)
                    .HasForeignKey(x => x.MauID);

                entity.Ignore(x => x.HoaDonChiTiets);
                entity.Ignore(x => x.ChiTietGioHangs);
            });

            modelBuilder.Entity<StorefrontOrder>(entity =>
            {
                entity.ToTable("HoaDon");
                entity.HasKey(x => x.HoaDonID);

                entity.Property(x => x.HoaDonID).HasMaxLength(450);
                entity.Property(x => x.TenKhachHang).HasMaxLength(100);
                entity.Property(x => x.SoDienThoai).HasMaxLength(20);
                entity.Property(x => x.DiaChiGiaoHang).HasMaxLength(255);
                entity.Property(x => x.NgayTao).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(x => x.TongTien).HasPrecision(18, 2);
                entity.Property(x => x.TrangThai).HasDefaultValue(0);

                entity.HasMany(x => x.ChiTietHoaDon)
                    .WithOne(x => x.HoaDon)
                    .HasForeignKey(x => x.HoaDonID);
            });

            modelBuilder.Entity<StorefrontOrderItem>(entity =>
            {
                entity.ToTable("HoaDonChiTiet");
                entity.HasKey(x => x.HoaDonChiTietID);

                entity.Property(x => x.HoaDonChiTietID).HasMaxLength(450);
                entity.Property(x => x.HoaDonID).HasMaxLength(450);
                entity.Property(x => x.ChiTietSanPhamID).HasMaxLength(450);
                entity.Property(x => x.DonGia).HasPrecision(18, 2);
                entity.Property(x => x.ThanhTien).HasPrecision(18, 2);

                entity.HasIndex(x => x.HoaDonID);
                entity.HasIndex(x => x.ChiTietSanPhamID);

                entity.HasOne<ChiTietSanPham>()
                    .WithMany()
                    .HasForeignKey(x => x.ChiTietSanPhamID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
