namespace DATN_70.Models.Entities
{
    public class KhuyenMaiSanPham
    {
        public string KhuyenMaiID { get; set; }
        public KhuyenMai KhuyenMai { get; set; }

        public string SanPhamID { get; set; }
        public SanPham SanPham { get; set; }
    }
}
