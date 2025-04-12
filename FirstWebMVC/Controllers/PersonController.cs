using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstWebMVC.Models;
using FirstWebMVC.Data;
using FirstWebMVC.Models.Process;
using System.IO;
using System.Data;
using OfficeOpenXml;
using X.PagedList;


namespace FirstWebMVC.Controllers
{
    public class PersonController : Controller
    { 
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
{
    int pageNumber = page ?? 1;
    int pageSize = 5;

    var persons = _context.Person.AsQueryable(); // Giữ IQueryable để phân trang hiệu quả
    var model = await persons.ToPagedListAsync(pageNumber, pageSize);

    return View(model);
}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,email")] Person person)
        {
            if (PersonExists(person.Id))
            {
                ModelState.AddModelError("Id", "Id này đã tồn tại!");
                return View(person);
            }

            if (!IsValidEmail(person.email))
            {
                ModelState.AddModelError("email", "Email không hợp lệ!");
                return View(person);
            }

            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

         public async Task<IActionResult> Download()
        {
            var fileName = Guid.NewGuid().ToString() + ".xlsx";
            using ExcelPackage excelPackage = new();
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            excelWorksheet.Cells["A1"].Value = "PersonID";
            excelWorksheet.Cells["B1"].Value = "FullName";
            excelWorksheet.Cells["C1"].Value = "Address";
            //get all person from database
            var personList = await _context.Person.ToListAsync();
            //fill data to worksheet
            excelWorksheet.Cells["A2"].LoadFromCollection(personList, true);

            var stream = new MemoryStream();
            excelPackage.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Gender,email")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Person == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Person' is null.");
            }
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return (_context.Person?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    //rename file when upload to server
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels" + fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using var stream = new FileStream(filePath, FileMode.Create);
                    //save file to server
                    await file.CopyToAsync(stream);
                    var dt = _excelProcess.ExcelToDataTable(fileLocation);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var ps = new Person();
                        ps.Id = dt.Rows[i][0].ToString()!;
                        ps.Name = dt.Rows[i][1].ToString()!;
                        ps.Gender = dt.Rows[i][2].ToString();
                        ps.email = dt.Rows[i][3].ToString();
                        await _context.AddAsync(ps);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(nameof(Create));
        }

    }
}