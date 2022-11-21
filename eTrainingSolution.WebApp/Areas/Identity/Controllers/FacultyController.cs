using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Faculty/[action]")]
    public class FacultyController : Controller
    {
        private readonly eTrainingDbContext _eTrainingDbContext;

        public FacultyController(eTrainingDbContext eTrainingDbContext)
        {
            _eTrainingDbContext = eTrainingDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(_eTrainingDbContext.Schools == null)
            {
                return Problem("null");
            }
            // thực hiện query để trả về list danh sách các phần tử
            var eTrainDbContext = _eTrainingDbContext.Facultys.Include(f => f.Schools);
            return View(await eTrainDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName");
            return View();
        }

        /// <summary>
        /// Bind("ID, FacultyName, CapacityOfFaculty, CreateDate, CreatedBy, SchoolID"): tạo ra 1 list item add vào BindAtrribute
        /// </summary>
        /// <param name="faculty"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID, FacultyName, CapacityOfFaculty, CreateDate, CreatedBy, SchoolID")] Faculty faculty)
        {
            if(ModelState.IsValid)
            {
                faculty.ID = Guid.NewGuid();
                _eTrainingDbContext.Add(faculty);
                await _eTrainingDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", faculty.SchoolID);
            return View(faculty);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid? id)
        {
            if(id == null || _eTrainingDbContext.Facultys == null)
            {
                return NotFound();
            }
            // câu lệnh query để truy vấn giữa ID của bảng Facultys với id của bảng Schools
            var faculitiesDbContext = await _eTrainingDbContext.Facultys.Include(g => g.Schools).FirstOrDefaultAsync(m => m.ID == id);
            if(faculitiesDbContext == null)
            {
                return NotFound();
            }
            return View(faculitiesDbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _eTrainingDbContext.Facultys == null)
            {
                return NotFound();
            }
            // tìm Khoa theo ID
            var faculitiesDbContext = await _eTrainingDbContext.Facultys.FindAsync(id);
            if(faculitiesDbContext == null)
            {
                return NotFound();
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", faculitiesDbContext.SchoolID);
            return View(faculitiesDbContext);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid? id, [Bind("ID, FacultyName, CapacityOfFaculty, CreateDate, CreatedBy, SchoolID")] Faculty faculty)
        {
            if (id == null || id != faculty.ID)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    var facultyesDb = _eTrainingDbContext.Facultys.Find(id);
                    _eTrainingDbContext.Entry(facultyesDb).CurrentValues.SetValues(faculty);
                    await _eTrainingDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", faculty.SchoolID);
            return View(faculty);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var faculitiesDbContext = await _eTrainingDbContext.Facultys.Include(m => m.Schools).FirstOrDefaultAsync(_ => _.ID == id);
            if(faculitiesDbContext == null)
            {
                return NotFound();
            }
            return View(faculitiesDbContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(_eTrainingDbContext.Facultys == null)
            {
                return Problem("Entity set null");
            }
            // Lấy thông tin Khoa theo ID
            var faculytiesDbContext = await _eTrainingDbContext.Facultys.FindAsync(id);
            if(faculytiesDbContext!= null)
            {
                // Tìm ra danh sách các lớp trong Khoa được chọn
                var classDbContext = await _eTrainingDbContext.Classrooms.Where(m=> m.FacultyID == id).ToListAsync();
                if(classDbContext!= null)
                {
                    _eTrainingDbContext.Classrooms.RemoveRange(classDbContext);
                }
                _eTrainingDbContext.Facultys.Remove(faculytiesDbContext);
            }
            await _eTrainingDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
