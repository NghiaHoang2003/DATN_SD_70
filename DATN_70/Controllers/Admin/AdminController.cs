using DATN_70.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DATN_70.Controllers.Admin;

public class AdminController : Controller
{

    // Bật giao diện Quản lý hóa đơn
    [CustomAuthorize("R01", "R02")]
    public IActionResult Orders()
    {
        return View();
    }

    // Bật giao diện Quản lý tài khoản
    [CustomAuthorize("R01")]
    public IActionResult Accounts()
    {
        return View();
    }

    // Bật giao diện Quản lý khuyến mãi
    [CustomAuthorize("R01")]
    public IActionResult Promotions()
    {
        return View();
    }

    // ĐIỀU HƯỚNG TRANG SẢN PHẨM
    [CustomAuthorize("R01")]
    public IActionResult Products()
    {
        return View();
    }
    // Thêm vào cuối file AdminController.cs của bạn
    [CustomAuthorize("R01")]
    public IActionResult Attributes()
    {
        return View();
    }
    [HttpGet]
    [CustomAuthorize("R01", "R02")]
    public IActionResult Dashboard()
    {
        return View();
    }
}