namespace DATN_70.Models.Products;

public sealed class ProductVariantResponse
{
    public string ChiTietSanPhamID { get; set; } = string.Empty;

    public string KichCoID { get; set; } = string.Empty;

    public string TenKichCo { get; set; } = string.Empty;

    public string MauID { get; set; } = string.Empty;

    public string TenMau { get; set; } = string.Empty;

    public decimal GiaNiemYet { get; set; }

    public int SoLuongTon { get; set; }
}
