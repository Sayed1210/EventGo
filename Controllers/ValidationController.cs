using Microsoft.AspNetCore.Mvc;
using System;

namespace EventGo.Controllers
{
    public class ValidationController : Controller
    {
        public IActionResult StartDate(DateTime startDate)
        {
            if (startDate < DateTime.Now) return Json(false);
            return Json(true);
        }

        public IActionResult EndDate(DateTime endDate, DateTime startDate)
        {
            if (endDate <= startDate) return Json(false);
            return Json(true);
        }
    }
}
