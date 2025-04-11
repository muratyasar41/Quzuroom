using Microsoft.AspNetCore.Mvc;
using CompanyManagementSystem.Web.Models;

namespace CompanyManagementSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private static List<User> _users = new List<User>();

        public IActionResult Index()
        {
            return View(_users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = _users.Count + 1;
                _users.Add(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Details(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
} 