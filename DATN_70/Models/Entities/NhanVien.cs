namespace DATN_70.Models.Entities
{
    public class NhanVien
    {
        public string NhanVienID { get; set; }
        public string Ten { get; set; }
        public string SoDienThoai { get; set; }
        public int GioiTinh { get; set; } // 0: Nam, 1: Nữ
        public string DiaChi { get; set; }
        public DateTime NgayNhanViec { get; set; }     
        public string TaiKhoanID { get; set; }        
        public TaiKhoan TaiKhoan { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}
