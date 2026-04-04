namespace DATN_70.Models.Entities
{
    public class HoaDonChiTiet
    {
        public string HoaDonChiTietID { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal MucVAT { get; set; }
        public decimal TienVAT { get; set; }

        public string HoaDonID { get; set; }
        public string ChiTietSanPhamID { get; set; }

        public HoaDon HoaDon { get; set; }
        public ChiTietSanPham ChiTietSanPham { get; set; }
    }
}
