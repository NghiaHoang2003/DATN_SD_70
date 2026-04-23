namespace DATN_70.Models.Products;

public sealed class ProductListItemResponse
{
    public string SanPhamID { get; set; } = string.Empty;

    public string Ten { get; set; } = string.Empty;

    public string MoTa { get; set; } = string.Empty;

    public decimal GiaThapNhat { get; set; }

    public decimal GiaGoc { get; set; }

    public decimal PhanTramGiam { get; set; }

    public int TongSoLuongTon { get; set; }
}
