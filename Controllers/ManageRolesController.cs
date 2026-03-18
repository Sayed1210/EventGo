using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EventGo.Models;
using System.Threading.Tasks;

namespace EventGo.Controllers
{
    public class ManageRolesController : Controller

    {
        private readonly RoleManager<MyRole> roleManager;
        public ManageRolesController(RoleManager<MyRole> rol)
        {
            roleManager = rol;
        }
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }
        // Action to Create Role
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(string RoleName, string Description) // ROleViewModel 
        {
            var role = new MyRole() { Name = RoleName, desc = Description };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            ModelState.AddModelError("", "Invalid Creation");
            return View();
        }





    }
}
