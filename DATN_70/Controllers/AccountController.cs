using DATN_70.Data;
using DATN_70.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _dbcontext;
        public AccountController(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
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
        public IActionResult Register(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                if (_dbcontext.TaiKhoans.Any(t => t.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                _dbcontext.TaiKhoans.Add(model);
                _dbcontext.SaveChanges();                
                return RedirectToAction("Login");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _dbcontext.TaiKhoans.FirstOrDefault(u => u.Email == email && u.MatKhau == password);
            if (user != null)
            {
                // Đăng nhập thành công: Lưu ID và Lưu luôn cả Email để hiển thị lên Layout
                HttpContext.Session.SetString("UserId", user.TaiKhoanID);
                HttpContext.Session.SetString("UserEmail", user.Email);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }
    }  
}
