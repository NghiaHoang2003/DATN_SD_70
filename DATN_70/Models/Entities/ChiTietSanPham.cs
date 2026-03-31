namespace DATN_70.Models.Entities
{
    public class ChiTietSanPham
    {
        public string ChiTietSanPhamID { get; set; }
        public int SoLuongTonKho { get; set; }
        public string SKU { get; set; }
        public decimal GiaNiemYet { get; set; }
        public string KichCoID { get; set; }
        public string MauID { get; set; }
        public string SanPhamID { get; set; }
        public KichCo KichCo { get; set; }
        public Mau Mau { get; set; }
        public SanPham SanPham { get; set; }
        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
    }
}
