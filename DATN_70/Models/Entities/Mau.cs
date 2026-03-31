namespace DATN_70.Models.Entities
{
    public class Mau
    {
        public string MauID { get; set; }
        public string Ten { get; set; }
        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
    }
}
