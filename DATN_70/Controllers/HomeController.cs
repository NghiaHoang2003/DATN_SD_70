using Microsoft.AspNetCore.Mvc;

namespace DATN_70.Controllers;

public class HomeController : Controller
{
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

    public IActionResult Checkout()
    {
        return View("~/Views/PhuongThucThanhToan/Index.cshtml");
    }
}
