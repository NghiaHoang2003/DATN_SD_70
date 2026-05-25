using DATN_70.Attributes;
using DATN_70.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Controllers.Admin;

[CustomAuthorize("R01", "R02")]
public class AdminPOSPageController : Controller
{
    private readonly AppDbContext _dbContext;

    public AdminPOSPageController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("/Admin/POS")]
    public async Task<IActionResult> Index()
    {
        // Lấy UserId từ session
        var userId = HttpContext.Session.GetString("UserId");
        var cashierName = "Thu ngân";

        if (!string.IsNullOrEmpty(userId))
        {
            // Tìm nhân viên theo TaiKhoanID
            var nhanVien = await _dbContext.NhanViens
                .AsNoTracking()
                .FirstOrDefaultAsync(nv => nv.TaiKhoanID == userId);

            if (nhanVien != null)
                cashierName = nhanVien.Ten;
            else
            {
                // Fallback về email nếu không có nhân viên
                cashierName = HttpContext.Session.GetString("UserEmail") ?? "Thu ngân";
            }
        }

        ViewBag.CashierName = cashierName;
        return View("~/Views/Admin/Pos.cshtml");
    }
}