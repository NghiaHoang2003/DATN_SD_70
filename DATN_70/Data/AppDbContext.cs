using DATN_70.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // Khai báo các bảng sẽ xuất hiện trong SQL Server
        // Nhóm Người dùng & Phân quyền
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<DiaChi> DiaChis { get; set; }

        // Nhóm Sản phẩm
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<ThuongHieu> ThuongHieus { get; set; }
        public DbSet<Mau> Maus { get; set; }
        public DbSet<KichCo> KichCos { get; set; }

        // Nhóm Bán hàng & Giỏ hàng
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }

        // Nhóm Khuyến mãi & Thanh toán
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<KhuyenMaiSanPham> KhuyenMaiSanPhams { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<ChiTietThanhToan> ChiTietThanhToans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Legacy schema chỉ có các bảng sản phẩm/đơn hàng tối thiểu.
            // Những entity chưa có bảng/cột tương ứng trong DB legacy sẽ bị bỏ qua.
            modelBuilder.Ignore<TaiKhoan>();
            modelBuilder.Ignore<VaiTro>();
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
                entity.Ignore(x => x.MoTa);
            });

            modelBuilder.Entity<Mau>(entity =>
            {
                entity.ToTable("Mau");
                entity.HasKey(x => x.MauID);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPham");
                entity.HasKey(x => x.SanPhamID);

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

                entity.Property(x => x.SoLuongTonKho)
                    .HasColumnName("SoLuongTon");

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

            // --- Cấu hình kiểu dữ liệu Decimal (Chuyên cho tiền tệ) ---
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
