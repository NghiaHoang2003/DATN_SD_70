using AspNetCoreGeneratedDocument;

namespace DATN_70.Models.Entities
{
    public class SanPham
    {
        public string SanPhamID { get; set; }
        public string Ten { get; set; }
        public double MucVAT { get; set; }
        public string ChatLieu { get; set; }
        public string MoTa { get; set; }
        public string ThuongHieuID { get; set; }
        public string DanhMucID { get; set; }


        public ThuongHieu ThuongHieu { get; set; }
        public DanhMuc DanhMuc { get; set; }

        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
        public ICollection<KhuyenMaiSanPham> KhuyenMaiSanPhams { get; set; }
    }
}
