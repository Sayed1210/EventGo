using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventGo.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventGo.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<MyUser> userManager;
        private readonly RoleManager<MyRole> roleManager;
        private readonly SignInManager<MyUser> signinManager;
        private readonly ILogger _logger;
        public AccountsController(UserManager<MyUser>um,RoleManager<MyRole>rm,SignInManager<MyUser>sm)
        {
            userManager = um;
            roleManager = rm;
            signinManager = sm;

            
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // var user=new MyUser() { FullName = model.FullName, Email = model.Email,UserName=model.Username };
            var user = new MyUser() { UserName = model.Email, Email = model.Email, FullName = model.FullName };
            var result=   await userManager.CreateAsync(user,model.Password);
            var check = await userManager.FindByEmailAsync(model.Email);
            //if (check != null)
            //{
            //    ModelState.AddModelError("Email", "Email is already taken.");
            //    return View(model);
            //}
       
            if (result.Succeeded)
            {
                // Login
                await signinManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home1");
            }
            ModelState.AddModelError("", "Invalid Register");
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
      
            return View(model);

        


    }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel1 model, string returnUrl)
        {
            //1)------------ Find User in Database & match password
            var result = await signinManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    Console.WriteLine("Redirecting to returnUrl: " + returnUrl);
                    return LocalRedirect(returnUrl);

                }
                else
                {
                    Console.WriteLine("Redirecting to Home1");
                    return RedirectToAction("Index", "Home1 ");
                }
            }

            ModelState.AddModelError("", "Invalid Attemped to Login");
            return View(model);

        }




        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Login", "Accounts");
        }
       // [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(userManager.Users.ToList());
        }
        //---------------Assign Role to User
        [HttpGet]
       
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(string id)
        {
            var user = await userManager.FindByIdAsync(id); // Find User in Database
            UserRoleViewModel userrole = new UserRoleViewModel() { UserID = user.Id, UserName = user.Email };
            ViewData["RoleId"] = new SelectList(roleManager.Roles.ToList(), "Id", "Name");
            return View(userrole);
        }
        [HttpPost]
        public async Task<IActionResult> Assign(UserRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserID);
            var role = await roleManager.FindByIdAsync(model.RoleId);
            var result = await userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ViewData["RoleId"] = new SelectList(roleManager.Roles.ToList(), "Id", "Name");
            return View(model);

        }
    }
}
