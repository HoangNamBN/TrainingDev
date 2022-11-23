using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Classes/[action]")]
    public class ClassesController : Controller
    {
        #region Khai báo dbContext sử dụng

        private readonly eTrainingDbContext _eTrainingDbContext;

        public ClassesController(eTrainingDbContext eTrainingDbContext) { 
            _eTrainingDbContext = eTrainingDbContext;
        }

        #endregion

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // lấy thông tin của class kèm theo thông tin của Facultys
            var classesDbContext = _eTrainingDbContext.Classrooms.Include(f => f.Facultys).Include(f => f.Schools);
            return View(await classesDbContext.ToListAsync());
        }

        #endregion

        #region Create Class

        /// <summary>
        /// Nghiệp vụ: 
        ///     - Chuyển hướng sang form Detail là Tạo mới
        ///     - Nhập các thông tin cần thiết trong đó có lựa chọn Khoa cho lớp học. Thông tin này sẽ được truyền từ Controller sang View để hứng kết quả
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles =RoleType.Admin)]
        public async Task<IActionResult> Create()
        {
            // lấy dữ liệu từ Controller chuyển sang cho View
            // ViewData có kiểu là Dictionary nên sẽ chứa cặp key-value
            // ViewData có giá trị sẽ bị xóa khi mà chuyển hướng
            ViewData["FacultyID"] = new SelectList(_eTrainingDbContext.Facultys, "ID", "FacultyName");
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName");
            return View();
        }

        /// <summary>
        /// Nghiệp vụ:
        ///     - khi nhấn vào button đăng ký => Controller nhận được request
        ///     - Các thông tin được nhập sẽ được truyền đến
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind("ID, ClassName, ClassCapacity, CreateDate, CreatedBy, FacultyID, SchoolID")] Classroom classes)
        {
            // check kiểm tra xem giá trị của các trường thông tin có là hợp lệ hay không
            if(ModelState.IsValid)
            {
                // tạo ID cho classes
                classes.ID= Guid.NewGuid();
                _eTrainingDbContext.Add(classes);
                await _eTrainingDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyID"] = new SelectList(_eTrainingDbContext.Facultys, "ID", "FacultyName", classes.FacultyID);
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", classes.SchoolID);
            return View(classes);
        }

        #endregion

        #region View thông tin

        [HttpGet]
        public async Task<IActionResult> View(Guid? id)
        {
            // Kiểm tra nếu id null hoặc không tồn tại khoa thì return NotFound()
            if(id == null || _eTrainingDbContext.Classrooms == null)
            {
                return NotFound();
            }
            // lấy thông tin của class kèm theo thông tin Facultys
            var classDb = await _eTrainingDbContext.Classrooms.Include(g=> g.Facultys).Include(g => g.Schools).FirstOrDefaultAsync(m => m.ID == id);
            if(classDb == null)
            {
                return NotFound();
            }
            return View(classDb);
        }

        #endregion

        #region Sửa thông tin Class

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
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
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", classDbContext.SchoolID);
            return View(classDbContext);
        }

        /// <summary>
        /// Bind: chỉ định các thuộc tính nào của lớp Model Classroom muốn liên kết
        /// Nếu muốn bỏ bớt các thuộc tính thì thêm Exclude
        /// </summary>
        /// <param name="id"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Guid? id, 
            [Bind("ID, ClassName, ClassCapacity, CreateDate, CreatedBy, FacultyID, SchoolID")] Classroom classes)
        {
            if (id != classes.ID || id == null)
            {
                // trả về kết quả không tìm thấy
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var classDb = _eTrainingDbContext.Classrooms.Find(id);
                    // sử dụng Entry của DbContext, SetValues để cập nhật các trường thông tin 
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
            ViewData["SchoolID"] = new SelectList(_eTrainingDbContext.Schools, "Id", "SchoolName", classes.SchoolID);
            return View(classes);
        }

        #endregion

        #region Xóa thông tin Class

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if(id == null || _eTrainingDbContext.Classrooms == null)
            {
                return NotFound();
            }
            // lấy thông tin của class thông qua ID và kèm theo thông tin của Facultys
            var classDbContext = await _eTrainingDbContext.Classrooms.Include(m => m.Facultys).Include(m => m.Schools).FirstOrDefaultAsync(g => g.ID == id);
            if(classDbContext == null)
            {
                return NotFound();
            }
            return View(classDbContext);
        }

        /// <summary>
        /// Do cùng truyền vào tham số là id nên đối với Delete của HttpPost thì đổi tên thành DeleteConfirmed và thêm ActionName
        /// </summary>
        /// <param name="id">id của lớp muốn xóa</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RoleType.Admin)]
        [AutoValidateAntiforgeryToken]
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
            // lấy thông tin class theo ID tìm được
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

        #endregion
    }
}
