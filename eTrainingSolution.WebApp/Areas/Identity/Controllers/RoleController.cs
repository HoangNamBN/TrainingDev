using eTrainingSolution.EntityFrameworkCore;
using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/Role/[action]")]
    public class RoleController : Controller
    {
        /// <summary>
        /// sử dụng RoleManger để quản lý và đọc các vai trò, các dịch vụ
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly eTrainingDbContext _eTrainingDbContext;
        private readonly UserManager<User> _userManager;
        
        /// <summary>
        /// inject các đối tượng vào trong hàm khởi tạo
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="eTrainingDbContext"></param>
        /// <param name="userManager"></param>
        public RoleController(RoleManager<IdentityRole> roleManager, eTrainingDbContext eTrainingDbContext, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _eTrainingDbContext = eTrainingDbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// HTTPGet đến trang quản lý Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var roles = new List<RoleModel>();
            //var r = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();

            //foreach(var _r in r)
            //{
            //    var roleModel = new RoleModel()
            //    {
            //        Name = _r.Name,
            //        Id = _r.Id,
            //    };
            //    roles.Add(roleModel);
            //}
            var data = _roleManager.Roles.ToList();
            ViewBag.Roles = data;
            return View();
        }
    }
}
