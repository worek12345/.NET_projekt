using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AirTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserAdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserAdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var list = new List<(IdentityUser User, IList<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add((user, roles));
            }

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (User.Identity?.Name == user.UserName)
                {
                    TempData["Error"] = "Nie możesz usunąć własnego konta.";
                    return RedirectToAction("Index");
                }

                await _userManager.DeleteAsync(user);
                TempData["Success"] = $"Użytkownik {user.UserName} został usunięty.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string id, string newRole)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || string.IsNullOrWhiteSpace(newRole)) return RedirectToAction("Index");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, newRole);

            TempData["Success"] = $"Zmieniono rolę użytkownika {user.Email ?? user.UserName} na: {newRole}";
            return RedirectToAction("Index");
        }
    }
}
