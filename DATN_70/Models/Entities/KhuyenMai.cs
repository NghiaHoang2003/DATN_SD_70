namespace DATN_70.Models.Entities
{
    public class KhuyenMai
    {
        public string KhuyenMaiID { get; set; }
        public string Ten { get; set; }
        public decimal PhanTramChietKhau { get; set; }
        public decimal GiaTriToiThieuApDung { get; set; }
        public DateTime NgayApDung { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }      
        public ICollection<HoaDon> HoaDons { get; set; }        
        public ICollection<KhuyenMaiSanPham> KhuyenMaiSanPhams { get; set; }
    }
}
