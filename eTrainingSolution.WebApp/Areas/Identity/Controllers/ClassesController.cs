using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Classes/[action]")]
    public class ClassesController : Controller
    {
        private readonly eTrainingDbContext _eTrainingDbContext;

        public ClassesController(eTrainingDbContext eTrainingDbContext) { 
            _eTrainingDbContext = eTrainingDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var classesDbContext = _eTrainingDbContext.Classrooms.Include(f => f.Facultys);
            return View(await classesDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["FacultyID"] = new SelectList(_eTrainingDbContext.Facultys, "ID", "FacultyName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID, ClassName, ClassCapacity, CreateDate, CreatedBy, FacultyID")] Classroom classes)
        {
            if(ModelState.IsValid)
            {
                classes.ID= Guid.NewGuid();
                _eTrainingDbContext.Add(classes);
                await _eTrainingDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyID"] = new SelectList(_eTrainingDbContext.Facultys, "ID", "FacultyName", classes.FacultyID);
            return View(classes);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid? id)
        {
            // Kiểm tra nếu id null hoặc không tồn tại khoa thì return NotFound()
            if(id == null || _eTrainingDbContext.Classrooms == null)
            {
                return NotFound();
            }
            // lấy thông tin của class kèm theo thông tin Facultys
            var classDb = await _eTrainingDbContext.Classrooms.Include(g=> g.Facultys).FirstOrDefaultAsync(m => m.ID == id);
            if(classDb == null)
            {
                return NotFound();
            }
            return View(classDb);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if(id == null || _eTrainingDbContext.Classrooms == null)
            {
                return NotFound();
            }
            var classDbContext = await _eTrainingDbContext.Classrooms.FindAsync(id);
            if(classDbContext == null)
            {
                return NotFound();
            }
            ViewData["FacultyID"] = new SelectList(_eTrainingDbContext.Facultys, "ID", "FacultyName", classDbContext.FacultyID);
            return View(classDbContext);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid? id, 
            [Bind("ID, ClassName, ClassCapacity, CreateDate, CreatedBy, FacultyID")] Classroom classes)
        {
            if (id != classes.ID)
            {
                // trả về kết quả không tìm thấy
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var classDb = _eTrainingDbContext.Classrooms.Find(id);
                    _eTrainingDbContext.Entry(classDb).CurrentValues.SetValues(classes);
                    await _eTrainingDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyID"] = new SelectList(_eTrainingDbContext.Facultys, "ID", "FacultyName", classes.FacultyID);
            return View(classes);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if(id == null || _eTrainingDbContext.Classrooms == null)
            {
                return NotFound();
            }
            var classDbContext = await _eTrainingDbContext.Classrooms.Include(m => m.Facultys).FirstOrDefaultAsync(g => g.ID == id);
            if(classDbContext == null)
            {
                return NotFound();
            }
            return View(classDbContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (_eTrainingDbContext.Classrooms == null)
            {
                return Problem("Entity set null!");
            }
            var classDbContext = await _eTrainingDbContext.Classrooms.FindAsync(id);
            if(classDbContext == null)
            {
                return NotFound();
            }
            // Xóa theo ID
            _eTrainingDbContext.Classrooms.Remove(classDbContext);
            await _eTrainingDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
