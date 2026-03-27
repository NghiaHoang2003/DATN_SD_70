using System.ComponentModel.DataAnnotations;

namespace DATN_70.Models.Orders;

public sealed class PlaceOrderRequest
{
    [Required]
    [StringLength(100)]
    public string TenKhachHang { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string DiaChiGiaoHang { get; set; } = string.Empty;

    [MinLength(1)]
    public List<OrderItemRequest> Items { get; set; } = [];
}
