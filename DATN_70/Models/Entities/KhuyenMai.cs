using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DATN_70.Models.Enums.Enums;
namespace DATN_70.Models.Entities
{
    public class KhuyenMai
    {
        [Key]
        [MaxLength(20)]
        public string KhuyenMaiID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal PhanTramChietKhau { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal GiaTriToiThieuApDung { get; set; }
        public DateTime NgayApDung { get; set; }
        public DateTime NgayKetThuc { get; set; }

        [MaxLength(500)]
        public string MoTa { get; set; }
        public TrangThaiHoatDong TrangThai { get; set; }      
        public ICollection<HoaDon> HoaDons { get; set; }        
        public ICollection<KhuyenMaiSanPham> KhuyenMaiSanPhams { get; set; }
    }
}
