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
        [HttpPost]
        public IActionResult Register(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {                
                if (_dbcontext.TaiKhoans.Any(t => t.Email == model.Email)) return BadRequest("Email đã tồn tại");
                _dbcontext.TaiKhoans.Add(model);
                _dbcontext.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _dbcontext.TaiKhoans.FirstOrDefault(u => u.Email == email && u.MatKhau == password);
            if (user != null)
            {
                // Lưu ID người dùng vào Session để đánh dấu đã đăng nhập
                HttpContext.Session.SetString("UserId", user.TaiKhoanID);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }
    }  
}
