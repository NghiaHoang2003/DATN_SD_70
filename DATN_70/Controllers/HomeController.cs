using Microsoft.AspNetCore.Mvc;

namespace DATN_70.Controllers
{
    public class HomeController : Controller
    {
        // Bắt đường dẫn mặc định "/"
        public IActionResult Index()
        {
            // Tự động tìm và trả về file Views/Home/Index.cshtml
            return View();
        }
        public IActionResult Details()
        {
            // Chỉ định đích danh file Index trong thư mục ChiTietSanPham
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
}
