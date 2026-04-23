using DATN_70.Data;
using DATN_70.Models.Cart;
using DATN_70.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CartController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public CartController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetCart(CancellationToken cancellationToken)
    {
        var account = await GetCurrentAccountAsync(cancellationToken);
        if (account is null)
        {
            return Unauthorized(new { message = "Vui lòng đăng nhập." });
        }

        var cart = await EnsureCartAsync(account.TaiKhoanID, cancellationToken);
        var response = await BuildCartResponseAsync(cart.GioHangID, cancellationToken);
        return Ok(response);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartResponse>> AddItem(
        [FromBody] AddCartItemRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var account = await GetCurrentAccountAsync(cancellationToken);
        if (account is null)
        {
            return Unauthorized(new { message = "Vui lòng đăng nhập." });
        }

        var cart = await EnsureCartAsync(account.TaiKhoanID, cancellationToken);
        var variant = await _dbContext.ChiTietSanPhams
            .Include(item => item.SanPham)
            .Include(item => item.Mau)
            .Include(item => item.KichCo)
            .FirstOrDefaultAsync(item => item.ChiTietSanPhamID == request.ChiTietSanPhamID, cancellationToken);

        if (variant is null)
        {
            return NotFound(new { message = "Không tìm thấy biến thể sản phẩm." });
        }

        var line = await _dbContext.ChiTietGioHangs
            .FirstOrDefaultAsync(item => item.GioHangID == cart.GioHangID && item.ChiTietSanPhamID == request.ChiTietSanPhamID, cancellationToken);

        var nextQuantity = (line?.SoLuong ?? 0) + request.SoLuong;
        if (nextQuantity > Math.Min(20, variant.SoLuongTonKho))
        {
            return BadRequest(new { message = $"Số lượng tối đa cho biến thể này là {Math.Min(20, variant.SoLuongTonKho)}." });
        }

        if (line is null)
        {
            var currentUnitPrice = await GetCurrentUnitPriceAsync(variant, cancellationToken);
            _dbContext.ChiTietGioHangs.Add(new ChiTietGioHang
            {
                ChiTietGioHangID = Guid.NewGuid().ToString(),
                GioHangID = cart.GioHangID,
                ChiTietSanPhamID = variant.ChiTietSanPhamID,
                SoLuong = request.SoLuong,
                TongTien = currentUnitPrice * request.SoLuong
            });
        }
        else
        {
            var currentUnitPrice = await GetCurrentUnitPriceAsync(variant, cancellationToken);
            line.SoLuong = nextQuantity;
            line.TongTien = currentUnitPrice * nextQuantity;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(await BuildCartResponseAsync(cart.GioHangID, cancellationToken));
    }

    [HttpPut("items/{productDetailId}")]
    public async Task<ActionResult<CartResponse>> UpdateItem(
        string productDetailId,
        [FromBody] UpdateCartItemRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var account = await GetCurrentAccountAsync(cancellationToken);
        if (account is null)
        {
            return Unauthorized(new { message = "Vui lòng đăng nhập." });
        }

        var cart = await EnsureCartAsync(account.TaiKhoanID, cancellationToken);
        var line = await _dbContext.ChiTietGioHangs
            .FirstOrDefaultAsync(item => item.GioHangID == cart.GioHangID && item.ChiTietSanPhamID == productDetailId, cancellationToken);

        if (line is null)
        {
            return NotFound(new { message = "Sản phẩm không có trong giỏ hàng." });
        }

        if (request.SoLuong <= 0)
        {
            _dbContext.ChiTietGioHangs.Remove(line);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Ok(await BuildCartResponseAsync(cart.GioHangID, cancellationToken));
        }

        var variant = await _dbContext.ChiTietSanPhams.FirstOrDefaultAsync(item => item.ChiTietSanPhamID == productDetailId, cancellationToken);
        if (variant is null)
        {
            return NotFound(new { message = "Không tìm thấy biến thể sản phẩm." });
        }

        if (request.SoLuong > Math.Min(20, variant.SoLuongTonKho))
        {
            return BadRequest(new { message = $"Số lượng tối đa cho biến thể này là {Math.Min(20, variant.SoLuongTonKho)}." });
        }

        line.SoLuong = request.SoLuong;
        line.TongTien = await GetCurrentUnitPriceAsync(variant, cancellationToken) * request.SoLuong;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Ok(await BuildCartResponseAsync(cart.GioHangID, cancellationToken));
    }

    [HttpDelete("items/{productDetailId}")]
    public async Task<ActionResult<CartResponse>> RemoveItem(
        string productDetailId,
        CancellationToken cancellationToken)
    {
        var account = await GetCurrentAccountAsync(cancellationToken);
        if (account is null)
        {
            return Unauthorized(new { message = "Vui lòng đăng nhập." });
        }

        var cart = await EnsureCartAsync(account.TaiKhoanID, cancellationToken);
        var line = await _dbContext.ChiTietGioHangs
            .FirstOrDefaultAsync(item => item.GioHangID == cart.GioHangID && item.ChiTietSanPhamID == productDetailId, cancellationToken);

        if (line is not null)
        {
            _dbContext.ChiTietGioHangs.Remove(line);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Ok(await BuildCartResponseAsync(cart.GioHangID, cancellationToken));
    }

    [HttpDelete]
    public async Task<ActionResult<CartResponse>> ClearCart(CancellationToken cancellationToken)
    {
        var account = await GetCurrentAccountAsync(cancellationToken);
        if (account is null)
        {
            return Unauthorized(new { message = "Vui lÃ²ng Ä‘Äƒng nháº­p." });
        }

        var cart = await EnsureCartAsync(account.TaiKhoanID, cancellationToken);
        var lines = await _dbContext.ChiTietGioHangs
            .Where(item => item.GioHangID == cart.GioHangID)
            .ToListAsync(cancellationToken);

        if (lines.Count > 0)
        {
            _dbContext.ChiTietGioHangs.RemoveRange(lines);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Ok(await BuildCartResponseAsync(cart.GioHangID, cancellationToken));
    }

    private async Task<TaiKhoan?> GetCurrentAccountAsync(CancellationToken cancellationToken)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return await _dbContext.TaiKhoans.FirstOrDefaultAsync(item => item.TaiKhoanID == userId, cancellationToken);
    }

    private async Task<GioHang> EnsureCartAsync(string accountId, CancellationToken cancellationToken)
    {
        var cart = await _dbContext.GioHangs.FirstOrDefaultAsync(item => item.TaiKhoanID == accountId, cancellationToken);
        if (cart is not null)
        {
            return cart;
        }

        cart = new GioHang
        {
            GioHangID = Guid.NewGuid().ToString(),
            TaiKhoanID = accountId,
            NgayTao = DateTime.Now
        };

        _dbContext.GioHangs.Add(cart);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return cart;
    }

    private async Task<CartResponse> BuildCartResponseAsync(string cartId, CancellationToken cancellationToken)
    {
        var items = await _dbContext.ChiTietGioHangs
            .AsNoTracking()
            .Include(item => item.ChiTietSanPham)
                .ThenInclude(item => item.SanPham)
            .Include(item => item.ChiTietSanPham)
                .ThenInclude(item => item.Mau)
            .Include(item => item.ChiTietSanPham)
                .ThenInclude(item => item.KichCo)
            .Where(item => item.GioHangID == cartId)
            .OrderBy(item => item.ChiTietSanPham.SanPham.Ten)
            .ThenBy(item => item.ChiTietSanPham.Mau.Ten)
            .ThenBy(item => item.ChiTietSanPham.KichCo.Ten)
            .ToListAsync(cancellationToken);

        var discounts = await GetActiveDiscountsAsync(
            items.Select(item => item.ChiTietSanPham.SanPhamID).Distinct().ToList(),
            cancellationToken);

        return new CartResponse
        {
            Items = items.Select(item => new CartItemResponse
            {
                SanPhamID = item.ChiTietSanPham.SanPhamID,
                ChiTietSanPhamID = item.ChiTietSanPhamID,
                TenSanPham = item.ChiTietSanPham.SanPham.Ten,
                PhanLoai = $"{item.ChiTietSanPham.Mau.Ten} / {item.ChiTietSanPham.KichCo.Ten.Replace("Size ", string.Empty)}",
                SoLuong = item.SoLuong,
                DonGia = ApplyDiscount(item.ChiTietSanPham.GiaNiemYet, discounts.GetValueOrDefault(item.ChiTietSanPham.SanPhamID)),
                TonKho = item.ChiTietSanPham.SoLuongTonKho
            }).ToList()
        };
    }

    private async Task<decimal> GetCurrentUnitPriceAsync(ChiTietSanPham variant, CancellationToken cancellationToken)
    {
        var discounts = await GetActiveDiscountsAsync([variant.SanPhamID], cancellationToken);
        return ApplyDiscount(variant.GiaNiemYet, discounts.GetValueOrDefault(variant.SanPhamID));
    }

    private async Task<Dictionary<string, decimal>> GetActiveDiscountsAsync(
        IReadOnlyCollection<string> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<string, decimal>();
        }

        var now = DateTime.Now;
        return await _dbContext.KhuyenMaiSanPhams
            .AsNoTracking()
            .Where(item => productIds.Contains(item.SanPhamID)
                && item.KhuyenMai.TrangThai == Models.Enums.Enums.TrangThaiHoatDong.HoatDong
                && item.KhuyenMai.NgayApDung <= now
                && item.KhuyenMai.NgayKetThuc >= now)
            .GroupBy(item => item.SanPhamID)
            .Select(group => new
            {
                SanPhamID = group.Key,
                PhanTramGiam = group.Max(item => item.KhuyenMai.PhanTramChietKhau)
            })
            .ToDictionaryAsync(item => item.SanPhamID, item => item.PhanTramGiam, cancellationToken);
    }

    private static decimal ApplyDiscount(decimal basePrice, decimal discountPercent)
    {
        if (discountPercent <= 0)
        {
            return basePrice;
        }

        return Math.Round(basePrice * (100 - discountPercent) / 100m, 0, MidpointRounding.AwayFromZero);
    }
}
