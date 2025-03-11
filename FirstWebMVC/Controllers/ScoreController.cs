using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using  FirstWebMVC.Models;
namespace FirstWebMVC.Controllers
{
    public class ScoreController : Controller
    { 
        // GET: /Score/
       
        public IActionResult Index() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Score score){
            if (score.A > 0 && score.B > 0 && score.C > 0){
                double totalScore = (score.A * 0.6) + (score.B * 0.3) + (score.C * 0.1);
                string grade;

                if (totalScore >= 9.0)
                    grade = "Xuất sắc";
                else if (totalScore >= 8.0)
                    grade = "Giỏi";
                else if (totalScore >= 6.5)
                    grade = "Khá";
                else if (totalScore >= 5.0)
                    grade = "Trung bình";
                else if (totalScore >= 3.5)
                    grade = "Yếu";
                else
                    grade = "Kém";

                ViewBag.TotalScore = totalScore.ToString("0.00");
                ViewBag.Grade = grade;
            }
            else {
                ViewBag.Error = "Vui lòng nhập điểm hợp lệ!";
            }
            return View();
        }
    }

}