using eTrainingSolution.EntityFrameworkCore.Entities;
using eTrainingSolution.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eTrainingSolution.Shared;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    public class BaseController : Controller
    {
        public readonly SignInManager<User> _signInManager;
        public readonly UserManager<User> _userManager;
        public readonly eTrainingDbContext _eTrainingDbContext;
        public readonly RoleManager<IdentityRole> _roleManager;

        public BaseController(SignInManager<User> signInManager, UserManager<User> userManager, eTrainingDbContext eTrainingDbContext, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eTrainingDbContext = eTrainingDbContext;
            _roleManager = roleManager;
        }

        public async Task<bool> IsAdmin()
        {
            Task<User> user = getUserEntities();

            // lấy ra quyền của user hiện tại
            var roles = await _userManager.GetRolesAsync(await user);
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i] == RoleType.Admin)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<User> getUserEntities() => await _userManager.GetUserAsync(User);
    }
}
