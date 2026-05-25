using DATN_70.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DATN_70.Controllers.Admin;

public class ReportsController : Controller
{
    [Route("/Admin/Reports/Advanced")]
    [CustomAuthorize("R01")]   // chỉ R01

    public IActionResult Advanced()
    {
        return View("~/Views/Admin/Reports/Advanced.cshtml");
    }
}