using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    public class PublicController : Controller
    {
        #region Khai báo các dịch vụ được sử dụng

        public readonly SignInManager<UserInfo> _signInManager;
        public readonly UserManager<UserInfo> _userManager;
        public readonly DB_Context _context;
        public readonly RoleManager<IdentityRole> _roleManager;

        public PublicController(SignInManager<UserInfo> signInManager, UserManager<UserInfo> userManager, DB_Context context, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        #endregion

        #region Hàm khởi tạo
        #endregion

        #region Kiểm tra xem có phải Admin không
        /// <summary>
        /// Kiểm tra xem user vừa đăng nhập có phải là quyền Admin không
        /// </summary>
        /// <returns>true nếu là Admin và false nếu không phải Admin</returns>
        public async Task<bool> IsAdmin()
        {
            Task<UserInfo> user = getUserET();

            // lấy ra quyền của user hiện tại
            var roles = await _userManager.GetRolesAsync(await user);
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i] == RoleType.Admin) return true;
            }
            return false;
        }
        #endregion

        #region Lấy ra thông tin user hiện tại đang đăng nhập
        /// <summary>
        /// Lấy thông tin user đăng nhập
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> getUserET() => await _userManager.GetUserAsync(User);
        #endregion

        #region Lấy ra danh sách user
        public List<UserInfo> GetListUser()
        {
            return _context.UserET?.ToList() ?? new List<UserInfo>();
        }
        #endregion

        #region Trả về một danh sách SelectList
        public SelectList Select_Facult()
        {
            return new SelectList(_context.FacultET, Default.ID, Default.FacultName);
        }
        public SelectList Select_Class()
        {
            return new SelectList(_context.ClassET, Default.ID, Default.ClassName);
        }
        public SelectList Select_School()
        {
            return new SelectList(_context.SchoolET, Default.ID, Default.SchoolName);
        }
        #endregion
    }
}
