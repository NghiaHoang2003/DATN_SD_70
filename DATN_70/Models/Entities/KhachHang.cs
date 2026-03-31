namespace DATN_70.Models.Entities
{
    public class KhachHang
    {
        public string KhachHangID { get; set; }
        public string Ten { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public int DiemTichLuy { get; set; }

        // Khóa ngoại trỏ về TaiKhoan
        public string TaiKhoanID { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}
