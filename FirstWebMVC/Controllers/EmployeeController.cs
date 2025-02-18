using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using FirstWebMVC.Models;
namespace FirstWebMVC.Controllers
{
    public class EmployeeController : Controller
    { 
        // GET: /Employee/
        public IActionResult Index()
        {
            return View();
        } 
    
        public string some(){
            return "hog co chi dou";
        }

        [HttpPost]
       public IActionResult Index(Employee employee){
         ViewBag.id = employee.id;
         ViewBag.name = employee.Name;
         ViewBag.gender = employee.Gender;
         ViewBag.email = employee.email;
         ViewBag.age = employee.Age;
         return View();
       }
    }
}