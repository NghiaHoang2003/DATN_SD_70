namespace DATN_70.Models.Entities
{
    public class PhuongThucThanhToan
    {
        public string PhuongThucThanhToanID { get; set; }
        public string Ten { get; set; }
        public string KieuThanhToan { get; set; } // Online/Offline
        public string HinhURL { get; set; }
        public int TrangThai { get; set; } // 1: Đang hoạt động, 0: Không hoạt động        
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
}
