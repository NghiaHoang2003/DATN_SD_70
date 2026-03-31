namespace DATN_70.Models.Entities
{
    public class VaiTro
    {
        public string VaiTroID { get; set; }
        public string Ten { get; set; }
        public ICollection<TaiKhoan> TaiKhoans { get; set; }
    }
}
