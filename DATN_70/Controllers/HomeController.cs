using DATN_70.Data;
using DATN_70.Models.Entities;
using DATN_70.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Products()
    {
        return View();
    }

    public IActionResult Details(string? id)
    {
        ViewData["ProductId"] = id ?? string.Empty;
        return View("~/Views/ChiTietSanPham/Index.cshtml");
    }

    public IActionResult Cart()
    {
        return View("~/Views/GioHang/Index.cshtml");
    }

    public async Task<IActionResult> Checkout()
    {
        var model = new CheckoutPageViewModel
        {
            Customer = await BuildCheckoutCustomerAsync()
        };

        return View("~/Views/PhuongThucThanhToan/Index.cshtml", model);
    }

    private async Task<CheckoutCustomerBootstrapViewModel> BuildCheckoutCustomerAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new CheckoutCustomerBootstrapViewModel();
        }

        var account = await _dbContext.TaiKhoans
            .AsNoTracking()
            .Include(item => item.KhachHang)
            .FirstOrDefaultAsync(item => item.TaiKhoanID == userId);

        if (account is null)
        {
            return new CheckoutCustomerBootstrapViewModel();
        }

        var customer = account.KhachHang;
        if (customer is null)
        {
            return new CheckoutCustomerBootstrapViewModel
            {
                IsAuthenticated = true,
                Email = account.Email
            };
        }

        var addressEntities = await _dbContext.DiaChis
            .AsNoTracking()
            .Where(item => item.KhachHangID == customer.KhachHangID)
            .OrderByDescending(item => item.LaMacDinh)
            .ThenBy(item => item.TenNguoiNhan)
            .ToListAsync();

        var addresses = addressEntities.Select(item => new SavedAddressViewModel
        {
            Id = item.DiaChiID,
            RecipientName = item.TenNguoiNhan,
            Phone = item.SoDienThoaiNhan,
            Street = AddressSerializer.ExtractStreet(item.PhuongXa),
            Ward = AddressSerializer.ExtractWard(item.PhuongXa),
            District = item.QuanHuyen,
            Province = item.TinhThanh,
            IsDefault = item.LaMacDinh
        }).ToList();

        var defaultAddress = addresses.FirstOrDefault(item => item.IsDefault) ?? addresses.FirstOrDefault();

        return new CheckoutCustomerBootstrapViewModel
        {
            IsAuthenticated = true,
            FullName = customer.Ten ?? string.Empty,
            Email = account.Email ?? customer.Email ?? string.Empty,
            Phone = customer.SoDienThoai ?? string.Empty,
            Street = defaultAddress?.Street ?? ExtractStreetFromCustomer(customer.DiaChi),
            Province = defaultAddress?.Province ?? string.Empty,
            District = defaultAddress?.District ?? string.Empty,
            Ward = defaultAddress?.Ward ?? string.Empty,
            SelectedAddressId = defaultAddress?.Id ?? string.Empty,
            SavedAddresses = addresses
        };
    }

    private static string ExtractStreetFromCustomer(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return string.Empty;
        }

        var parts = address.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.FirstOrDefault() ?? address;
    }
}

internal static class AddressSerializer
{
    private const string Separator = "||";

    public static string Pack(string ward, string street)
    {
        return $"{ward}{Separator}{street}";
    }

    public static string ExtractWard(string? packedWard)
    {
        if (string.IsNullOrWhiteSpace(packedWard))
        {
            return string.Empty;
        }

        var parts = packedWard.Split(Separator, StringSplitOptions.None);
        return parts[0];
    }

    public static string ExtractStreet(string? packedWard)
    {
        if (string.IsNullOrWhiteSpace(packedWard))
        {
            return string.Empty;
        }

        var parts = packedWard.Split(Separator, StringSplitOptions.None);
        return parts.Length > 1 ? parts[1] : string.Empty;
    }
}
