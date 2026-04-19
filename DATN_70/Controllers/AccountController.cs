using DATN_70.Data;
using DATN_70.Models.Entities;
using DATN_70.Models.Enums;
using DATN_70.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _dbContext;

    public AccountController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(TaiKhoan model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _dbContext.TaiKhoans.AnyAsync(item => item.Email == model.Email))
        {
            ModelState.AddModelError(nameof(TaiKhoan.Email), "Email này đã được sử dụng.");
            return View(model);
        }

        model.TaiKhoanID = string.IsNullOrWhiteSpace(model.TaiKhoanID) ? Guid.NewGuid().ToString() : model.TaiKhoanID;
        model.TrangThai = string.IsNullOrWhiteSpace(model.TrangThai) ? "Hoạt động" : model.TrangThai;
        model.VaiTroID = string.IsNullOrWhiteSpace(model.VaiTroID) ? "R03" : model.VaiTroID;

        var customer = new KhachHang
        {
            KhachHangID = GenerateId("KH", 20),
            Ten = model.Email.Split('@')[0],
            Email = model.Email,
            SoDienThoai = "0000000000",
            GioiTinh = Enums.GioiTinh.Khac,
            DiaChi = string.Empty,
            TaiKhoanID = model.TaiKhoanID
        };

        _dbContext.TaiKhoans.Add(model);
        _dbContext.KhachHangs.Add(customer);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _dbContext.TaiKhoans
            .Include(item => item.KhachHang)
            .FirstOrDefaultAsync(item => item.Email == email && item.MatKhau == password);

        if (user is null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu.";
            return View();
        }

        HttpContext.Session.SetString("UserId", user.TaiKhoanID);
        HttpContext.Session.SetString("UserEmail", user.Email);

        if (user.KhachHang is null)
        {
            _dbContext.KhachHangs.Add(new KhachHang
            {
                KhachHangID = GenerateId("KH", 20),
                Ten = user.Email.Split('@')[0],
                Email = user.Email,
                SoDienThoai = "0000000000",
                GioiTinh = Enums.GioiTinh.Khac,
                DiaChi = string.Empty,
                TaiKhoanID = user.TaiKhoanID
            });
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var defaultAddress = await _dbContext.DiaChis
            .AsNoTracking()
            .Where(item => item.KhachHangID == user.KhachHang!.KhachHangID)
            .OrderByDescending(item => item.LaMacDinh)
            .Select(item => new
            {
                item.TinhThanh,
                item.QuanHuyen,
                item.PhuongXa
            })
            .FirstOrDefaultAsync();

        var model = new AccountProfileViewModel
        {
            TaiKhoanId = user.TaiKhoanID,
            KhachHangId = user.KhachHang!.KhachHangID,
            FullName = user.KhachHang.Ten,
            Email = user.Email,
            Phone = NormalizePhoneForDisplay(user.KhachHang.SoDienThoai),
            DefaultAddressText = defaultAddress is null
                ? "Chưa có địa chỉ mặc định."
                : string.Join(", ", new[]
                {
                    AddressSerializer.ExtractStreet(defaultAddress.PhuongXa),
                    AddressSerializer.ExtractWard(defaultAddress.PhuongXa),
                    defaultAddress.QuanHuyen,
                    defaultAddress.TinhThanh
                }.Where(part => !string.IsNullOrWhiteSpace(part)))
        };

        if (TempData["ProfileStatus"] is string statusMessage)
        {
            model.StatusMessage = statusMessage;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(AccountProfileViewModel model)
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (!ModelState.IsValid)
        {
            model.DefaultAddressText = await GetDefaultAddressTextAsync(user.KhachHang!.KhachHangID);
            return View(model);
        }

        user.Email = model.Email.Trim();
        user.KhachHang!.Ten = model.FullName.Trim();
        user.KhachHang.Email = model.Email.Trim();
        user.KhachHang.SoDienThoai = NormalizePhoneForStorage(model.Phone);

        await _dbContext.SaveChangesAsync();

        HttpContext.Session.SetString("UserEmail", user.Email);
        TempData["ProfileStatus"] = "Thông tin tài khoản đã được cập nhật.";
        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public async Task<IActionResult> Addresses(string? editId = null)
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var addresses = await BuildAddressItemsAsync(user.KhachHang!.KhachHangID);
        var editing = addresses.FirstOrDefault(item => item.Id == editId);

        var model = new AccountAddressPageViewModel
        {
            Addresses = addresses,
            Form = editing is null
                ? new AddressFormViewModel()
                : new AddressFormViewModel
                {
                    Id = editing.Id,
                    RecipientName = editing.RecipientName,
                    Phone = editing.Phone,
                    Street = editing.Street,
                    Province = editing.Province,
                    District = editing.District,
                    Ward = editing.Ward,
                    IsDefault = editing.IsDefault
                }
        };

        if (TempData["AddressStatus"] is string statusMessage)
        {
            model.StatusMessage = statusMessage;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAddress(AddressFormViewModel form)
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (!ModelState.IsValid)
        {
            var invalidModel = new AccountAddressPageViewModel
            {
                Form = form,
                Addresses = await BuildAddressItemsAsync(user.KhachHang!.KhachHangID)
            };
            return View("Addresses", invalidModel);
        }

        var customer = user.KhachHang!;
        var normalizedPhone = NormalizePhoneForStorage(form.Phone);
        var address = string.IsNullOrWhiteSpace(form.Id)
            ? new DiaChi
            {
                DiaChiID = Guid.NewGuid().ToString(),
                KhachHangID = customer.KhachHangID
            }
            : await _dbContext.DiaChis.FirstOrDefaultAsync(item => item.DiaChiID == form.Id && item.KhachHangID == customer.KhachHangID);

        if (address is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(form.Id))
        {
            _dbContext.DiaChis.Add(address);
        }

        address.TenNguoiNhan = form.RecipientName.Trim();
        address.SoDienThoaiNhan = normalizedPhone;
        address.TinhThanh = form.Province.Trim();
        address.QuanHuyen = form.District.Trim();
        address.PhuongXa = AddressSerializer.Pack(form.Ward.Trim(), form.Street.Trim());
        address.LaMacDinh = form.IsDefault;

        if (form.IsDefault)
        {
            await ClearDefaultAddressesAsync(customer.KhachHangID, address.DiaChiID);
        }

        customer.DiaChi = string.Join(", ", new[] { form.Street.Trim(), form.Ward.Trim(), form.District.Trim(), form.Province.Trim() });
        customer.SoDienThoai = normalizedPhone;
        if (!string.IsNullOrWhiteSpace(form.RecipientName))
        {
            customer.Ten = form.RecipientName.Trim();
        }

        await _dbContext.SaveChangesAsync();

        TempData["AddressStatus"] = string.IsNullOrWhiteSpace(form.Id)
            ? "Địa chỉ mới đã được thêm."
            : "Địa chỉ đã được cập nhật.";
        return RedirectToAction(nameof(Addresses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetDefaultAddress(string id)
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var address = await _dbContext.DiaChis.FirstOrDefaultAsync(item => item.DiaChiID == id && item.KhachHangID == user.KhachHang!.KhachHangID);
        if (address is null)
        {
            return NotFound();
        }

        await ClearDefaultAddressesAsync(user.KhachHang!.KhachHangID, address.DiaChiID);
        address.LaMacDinh = true;
        user.KhachHang.DiaChi = string.Join(", ", new[]
        {
            AddressSerializer.ExtractStreet(address.PhuongXa),
            AddressSerializer.ExtractWard(address.PhuongXa),
            address.QuanHuyen,
            address.TinhThanh
        }.Where(part => !string.IsNullOrWhiteSpace(part)));
        user.KhachHang.SoDienThoai = address.SoDienThoaiNhan;
        user.KhachHang.Ten = address.TenNguoiNhan;

        await _dbContext.SaveChangesAsync();
        TempData["AddressStatus"] = "Đã cập nhật địa chỉ mặc định.";
        return RedirectToAction(nameof(Addresses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(string id)
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var address = await _dbContext.DiaChis.FirstOrDefaultAsync(item => item.DiaChiID == id && item.KhachHangID == user.KhachHang!.KhachHangID);
        if (address is null)
        {
            return NotFound();
        }

        _dbContext.DiaChis.Remove(address);
        await _dbContext.SaveChangesAsync();

        TempData["AddressStatus"] = "Địa chỉ đã được xóa.";
        return RedirectToAction(nameof(Addresses));
    }

    [HttpGet]
    public IActionResult Password()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction(nameof(Login));
        }

        var model = new ChangePasswordViewModel();
        if (TempData["PasswordStatus"] is string statusMessage)
        {
            model.StatusMessage = statusMessage;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Password(ChangePasswordViewModel model)
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!string.Equals(user.MatKhau, model.CurrentPassword, StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(ChangePasswordViewModel.CurrentPassword), "Mật khẩu hiện tại không đúng.");
            return View(model);
        }

        user.MatKhau = model.NewPassword;
        await _dbContext.SaveChangesAsync();

        TempData["PasswordStatus"] = "Mật khẩu đã được cập nhật.";
        return RedirectToAction(nameof(Password));
    }

    [HttpGet]
    public async Task<IActionResult> Orders(string status = "all")
    {
        var user = await GetCurrentAccountAsync();
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var query = _dbContext.HoaDons
            .AsNoTracking()
            .Include(item => item.DiaChi)
            .Include(item => item.HoaDonChiTiets)
                .ThenInclude(item => item.ChiTietSanPham)
                    .ThenInclude(item => item.SanPham)
            .Where(item => item.KhachHangID == user.KhachHang!.KhachHangID);

        if (TryMapOrderFilter(status, out var orderStatus))
        {
            query = query.Where(item => item.TrangThai == orderStatus);
        }

        var orderEntities = await query
            .OrderByDescending(item => item.NgayTao)
            .ToListAsync();

        var orders = orderEntities.Select(item => new AccountOrderViewModel
        {
            Id = item.HoaDonID,
            StatusKey = GetStatusKey(item.TrangThai),
            StatusLabel = GetStatusLabel(item.TrangThai),
            CreatedAt = item.NgayTao,
            TotalAmount = (decimal)item.ThanhTien,
            RecipientName = item.DiaChi != null ? item.DiaChi.TenNguoiNhan : user.KhachHang!.Ten,
            Phone = item.DiaChi != null ? item.DiaChi.SoDienThoaiNhan : user.KhachHang!.SoDienThoai,
            ShippingAddress = item.DiaChi != null
                ? string.Join(", ", new[]
                {
                    AddressSerializer.ExtractStreet(item.DiaChi.PhuongXa),
                    AddressSerializer.ExtractWard(item.DiaChi.PhuongXa),
                    item.DiaChi.QuanHuyen,
                    item.DiaChi.TinhThanh
                }.Where(part => !string.IsNullOrWhiteSpace(part)))
                : user.KhachHang!.DiaChi,
            Items = item.HoaDonChiTiets.Select(detail => new AccountOrderLineViewModel
            {
                ProductName = detail.ChiTietSanPham.SanPham.Ten,
                Variant = $"{detail.ChiTietSanPham.KichCoID} / {detail.ChiTietSanPham.MauID}",
                Quantity = detail.SoLuong,
                UnitPrice = detail.DonGia
            }).ToList()
        }).ToList();

        return View(new AccountOrdersPageViewModel
        {
            CurrentFilter = status,
            Orders = orders
        });
    }

    private async Task<TaiKhoan?> GetCurrentAccountAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        var user = await _dbContext.TaiKhoans
            .Include(item => item.KhachHang)
            .FirstOrDefaultAsync(item => item.TaiKhoanID == userId);

        if (user?.KhachHang is not null)
        {
            return user;
        }

        if (user is null)
        {
            return null;
        }

        var customer = new KhachHang
        {
            KhachHangID = GenerateId("KH", 20),
            Ten = user.Email.Split('@')[0],
            Email = user.Email,
            SoDienThoai = "0000000000",
            GioiTinh = Enums.GioiTinh.Khac,
            DiaChi = string.Empty,
            TaiKhoanID = user.TaiKhoanID
        };

        _dbContext.KhachHangs.Add(customer);
        await _dbContext.SaveChangesAsync();
        user.KhachHang = customer;
        return user;
    }

    private async Task<List<AccountAddressItemViewModel>> BuildAddressItemsAsync(string customerId)
    {
        var entities = await _dbContext.DiaChis
            .AsNoTracking()
            .Where(item => item.KhachHangID == customerId)
            .OrderByDescending(item => item.LaMacDinh)
            .ThenBy(item => item.TenNguoiNhan)
            .ToListAsync();

        return entities.Select(item => new AccountAddressItemViewModel
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
    }

    private async Task<string> GetDefaultAddressTextAsync(string customerId)
    {
        var address = await _dbContext.DiaChis
            .AsNoTracking()
            .Where(item => item.KhachHangID == customerId)
            .OrderByDescending(item => item.LaMacDinh)
            .Select(item => new
            {
                item.TinhThanh,
                item.QuanHuyen,
                item.PhuongXa
            })
            .FirstOrDefaultAsync();

        if (address is null)
        {
            return "Chưa có địa chỉ mặc định.";
        }

        return string.Join(", ", new[]
        {
            AddressSerializer.ExtractStreet(address.PhuongXa),
            AddressSerializer.ExtractWard(address.PhuongXa),
            address.QuanHuyen,
            address.TinhThanh
        }.Where(part => !string.IsNullOrWhiteSpace(part)));
    }

    private async Task ClearDefaultAddressesAsync(string customerId, string keepAddressId)
    {
        var addresses = await _dbContext.DiaChis
            .Where(item => item.KhachHangID == customerId && item.DiaChiID != keepAddressId && item.LaMacDinh)
            .ToListAsync();

        foreach (var address in addresses)
        {
            address.LaMacDinh = false;
        }
    }

    private static string GenerateId(string prefix, int totalLength)
    {
        var suffixLength = totalLength - prefix.Length;
        return prefix + Guid.NewGuid().ToString("N")[..suffixLength];
    }

    private static string NormalizePhoneForStorage(string phone)
    {
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        return string.IsNullOrWhiteSpace(digits) ? "0000000000" : digits;
    }

    private static string NormalizePhoneForDisplay(string phone)
    {
        return string.IsNullOrWhiteSpace(phone) || phone == "0000000000" ? string.Empty : phone;
    }

    private static bool TryMapOrderFilter(string status, out Enums.TrangThaiHoaDon orderStatus)
    {
        switch (status?.Trim().ToLowerInvariant())
        {
            case "confirmed":
                orderStatus = Enums.TrangThaiHoaDon.DaXacNhan;
                return true;
            case "preparing":
                orderStatus = Enums.TrangThaiHoaDon.DangChuanBi;
                return true;
            case "shipping":
                orderStatus = Enums.TrangThaiHoaDon.DangGiao;
                return true;
            case "success":
                orderStatus = Enums.TrangThaiHoaDon.HoanThanh;
                return true;
            case "cancelled":
                orderStatus = Enums.TrangThaiHoaDon.DaHuy;
                return true;
            default:
                orderStatus = Enums.TrangThaiHoaDon.ChoDuyet;
                return false;
        }
    }

    private static string GetStatusKey(Enums.TrangThaiHoaDon status)
    {
        return status switch
        {
            Enums.TrangThaiHoaDon.ChoDuyet => "pending",
            Enums.TrangThaiHoaDon.DaXacNhan => "confirmed",
            Enums.TrangThaiHoaDon.DangChuanBi => "preparing",
            Enums.TrangThaiHoaDon.DangGiao => "shipping",
            Enums.TrangThaiHoaDon.HoanThanh => "success",
            Enums.TrangThaiHoaDon.DaHuy => "cancelled",
            _ => "pending"
        };
    }

    private static string GetStatusLabel(Enums.TrangThaiHoaDon status)
    {
        return status switch
        {
            Enums.TrangThaiHoaDon.ChoDuyet => "Chờ xác nhận",
            Enums.TrangThaiHoaDon.DaXacNhan => "Đã xác nhận",
            Enums.TrangThaiHoaDon.DangChuanBi => "Đang chuẩn bị",
            Enums.TrangThaiHoaDon.DangGiao => "Đang giao",
            Enums.TrangThaiHoaDon.HoanThanh => "Thành công",
            Enums.TrangThaiHoaDon.DaHuy => "Đã hủy",
            _ => "Chờ xác nhận"
        };
    }
}
