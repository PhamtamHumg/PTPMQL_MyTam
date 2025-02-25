using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using FirstWebMVC.Models;
namespace FirstWebMVC.Controllers
{
    public class BillController : Controller
    { 
        // GET: /Bill/
        public IActionResult Index()
        {
            return View();
        } 

        [HttpPost]
        public IActionResult Index(Bill bill){
            int result = bill.Quantity * bill.Price;
            ViewBag.Bill = result;
            return View();
        }
    }
}