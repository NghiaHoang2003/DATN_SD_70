namespace DATN_70.Models.Entities
{
    public class ChiTietThanhToan
    {
        public string ChiTietThanhToanID { get; set; }
        public string Ten { get; set; }
        public decimal SoTien { get; set; }
        public string MaThamChieu { get; set; } // Mã giao dịch từ Ngân hàng/Ví điện tử
        public DateTime ThoiGianThanhToan { get; set; }
        public int TrangThai { get; set; } // 0: Thất bại, 1: Thành công        
        public string HoaDonID { get; set; }
        public HoaDon HoaDon { get; set; }       
        public string PhuongThucThanhToanID { get; set; }
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; }
    }
}
