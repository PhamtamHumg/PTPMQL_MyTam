using Microsoft.AspNetCore.Mvc;
using FirstWebMVC.Models;

namespace FirstWebMVC.Controllers
{
    public class BMIController : Controller
    { 
        // GET: /BMI/
        public IActionResult Index()
        {
            return View();
        } 

        [HttpPost]
        public IActionResult Index(BMI bmi)
        {
            if (bmi.weight > 0 && bmi.height > 0)
            {
                double result = bmi.weight / (bmi.height * bmi.height);
                string category = result < 18.5 ? "Gầy" :
                                  result < 24.9 ? "Bình Thường" :
                                  result < 29.9 ? "Béo phì" :
                                  "Béo phì nặng"; 
                
                ViewBag.BMI = result.ToString("0.00");
                ViewBag.Category = category;
            }
            else
            {
                ViewBag.Error = "Vui lòng nhập chiều cao cân nặng hợp lệ!";
            }
            return View();
        }
    }
}
