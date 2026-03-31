namespace DATN_70.Models.Entities
{
    public class ThuongHieu
    {
        public string ThuongHieuID { get; set; }
        public string Ten { get; set; }
        public string LogoURL { get; set; }
        public string MoTa { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
