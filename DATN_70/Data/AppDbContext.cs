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
            // --- 1. Cấu hình Khóa chính phức hợp cho bảng trung gian ---
            modelBuilder.Entity<KhuyenMaiSanPham>()
                .HasKey(ks => new { ks.KhuyenMaiID, ks.SanPhamID });

            // --- 2. Cấu hình Quan hệ 1-1 (One-to-One) ---

            // TaiKhoan <-> KhachHang
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.KhachHang)
                .WithOne(kh => kh.TaiKhoan)
                .HasForeignKey<KhachHang>(kh => kh.TaiKhoanID);

            // TaiKhoan <-> NhanVien
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.NhanVien)
                .WithOne(nv => nv.TaiKhoan)
                .HasForeignKey<NhanVien>(nv => nv.TaiKhoanID);

            // TaiKhoan <-> GioHang
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.GioHang)
                .WithOne(gh => gh.TaiKhoan)
                .HasForeignKey<GioHang>(gh => gh.TaiKhoanID);

            // --- 3. Cấu hình Quan hệ 1-N (One-to-Many) & Hạn chế xóa (Restrict) ---

            // VaiTro -> TaiKhoan
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.VaiTro)
                .WithMany(v => v.TaiKhoans)
                .HasForeignKey(tk => tk.VaiTroID);

            // DanhMuc -> SanPham
            modelBuilder.Entity<SanPham>()
                .HasOne(s => s.DanhMuc)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.DanhMucID);

            // ThuongHieu -> SanPham
            modelBuilder.Entity<SanPham>()
                .HasOne(s => s.ThuongHieu)
                .WithMany(t => t.SanPhams)
                .HasForeignKey(s => s.ThuongHieuID);

            // SanPham -> ChiTietSanPham
            modelBuilder.Entity<ChiTietSanPham>()
                .HasOne(ct => ct.SanPham)
                .WithMany(s => s.ChiTietSanPhams)
                .HasForeignKey(ct => ct.SanPhamID);

            // KichCo/Mau -> ChiTietSanPham
            modelBuilder.Entity<ChiTietSanPham>()
                .HasOne(ct => ct.KichCo)
                .WithMany(k => k.ChiTietSanPhams)
                .HasForeignKey(ct => ct.KichCoID);

            modelBuilder.Entity<ChiTietSanPham>()
                .HasOne(ct => ct.Mau)
                .WithMany(k => k.ChiTietSanPhams)
                .HasForeignKey(ct => ct.MauID);

            // --- 4. Cấu hình Đơn hàng (Hội tụ nhiều quan hệ) ---

            // KhachHang -> HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.KhachHang)
                .WithMany(kh => kh.HoaDons)
                .HasForeignKey(h => h.KhachHangID)
                .OnDelete(DeleteBehavior.Restrict);

            // NhanVien -> HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.NhanVien)
                .WithMany(nv => nv.HoaDons)
                .HasForeignKey(h => h.NhanVienID)
                .OnDelete(DeleteBehavior.Restrict);

            // DiaChi -> HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.DiaChi)
                .WithMany(dc => dc.HoaDons)
                .HasForeignKey(h => h.DiaChiID)
                .OnDelete(DeleteBehavior.Restrict);

            // KhuyenMai -> HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.KhuyenMai)
                .WithMany(km => km.HoaDons)
                .HasForeignKey(h => h.KhuyenMaiID)
                .OnDelete(DeleteBehavior.SetNull); // Nếu xóa KM, hóa đơn vẫn giữ lại

            // --- 5. Cấu hình Chi tiết (Giỏ hàng & Hóa đơn) ---

            modelBuilder.Entity<ChiTietGioHang>()
                .HasOne(ct => ct.GioHang)
                .WithMany(g => g.ChiTietGioHangs)
                .HasForeignKey(ct => ct.GioHangID);

            modelBuilder.Entity<HoaDonChiTiet>()
                .HasOne(ct => ct.HoaDon)
                .WithMany(h => h.HoaDonChiTiets)
                .HasForeignKey(ct => ct.HoaDonID);

            // --- 6. Cấu hình Thanh toán ---

            modelBuilder.Entity<ChiTietThanhToan>()
                .HasOne(ct => ct.PhuongThucThanhToan)
                .WithMany(p => p.ChiTietThanhToans)
                .HasForeignKey(ct => ct.PhuongThucThanhToanID);

            // --- 7. Cấu hình kiểu dữ liệu Decimal (Chuyên cho tiền tệ) ---
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
