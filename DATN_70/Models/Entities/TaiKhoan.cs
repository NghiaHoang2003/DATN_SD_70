namespace DATN_70.Models.Entities
{
    public class TaiKhoan
    {
        public string TaiKhoanID { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string TrangThai { get; set; }
        public string VaiTroID { get; set; }
        public VaiTro VaiTro { get; set; }
        public GioHang GioHang { get; set; }

        // Quan hệ 1-1 với Khách Hàng
        public KhachHang KhachHang { get; set; }
        // Quan hệ 1-1 với Nhân Viên
        public NhanVien NhanVien { get; set; }
    }
}
