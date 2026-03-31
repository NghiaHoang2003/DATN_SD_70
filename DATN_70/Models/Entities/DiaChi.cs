namespace DATN_70.Models.Entities
{
    public class DiaChi
    {
        public string DiaChiID { get; set; }
        public string TenNguoiNhan { get; set; }
        public string SoDienThoaiNhan { get; set; }
        public string TinhThanh { get; set; }
        public string QuanHuyen { get; set; }
        public string PhuongXa { get; set; }
        public bool LaMacDinh { get; set; }        
        public string KhachHangID { get; set; }
        public KhachHang KhachHang { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}
