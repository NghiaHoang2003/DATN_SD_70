namespace DATN_70.Models.Entities
{
    public class DanhMuc
    {
        public string DanhMucID { get; set; }
        public string Ten { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
