namespace DATN_70.Models.Entities
{
    public class ChiTietGioHang
    {
        public string ChiTietGioHangID { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }

        public string GioHangID { get; set; }
        public string ChiTietSanPhamID { get; set; }

        public GioHang GioHang { get; set; }
        public ChiTietSanPham ChiTietSanPham { get; set; }
    }
}
