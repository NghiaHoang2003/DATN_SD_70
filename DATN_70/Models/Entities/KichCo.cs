namespace DATN_70.Models.Entities
{
    public class KichCo
    {
        public string KichCoID { get; set; }
        public string Ten { get; set; } 
        public string MoTa { get; set; }
        public ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
    }
}
