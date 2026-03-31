namespace DATN_70.Models.Entities
{
    public class HoaDon
    {
        public string HoaDonID { get; set; }
        public double TongTienVAT { get; set; }
        public double TongTienGiamGia { get; set; }
        public double ThanhTien { get; set; }
        public int LoaiGiaoDich { get; set; }
        public DateTime NgayTao { get; set; } 
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string KhachHangID { get; set; }
        public string NhanVienID { get; set; }
        public string DiaChiID { get; set; }
        public string? KhuyenMaiID { get; set; }        
        public KhachHang KhachHang { get; set; }
        public NhanVien NhanVien { get; set; }
        public DiaChi DiaChi { get; set; }
        public KhuyenMai KhuyenMai { get; set; }
        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}
