namespace DATN_70.Models.Entities
{
    public class GioHang
    {
        public string GioHangID { get; set; }
        public DateTime NgayTao { get; set; }
        public string TaiKhoanID { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
    }
}
