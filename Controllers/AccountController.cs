using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EventGo.Models;
using EventGo.ViewModels;
using System;
using System.Threading.Tasks;

namespace EventGo.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly MovieContext db;
        #region Constructor Injection
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, MovieContext db)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
        }
        #endregion
        #region LogIn
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                
                var user = await userManager.FindByNameAsync(loginVM.UserLogin);
                user = (user != null) ? user : await userManager.FindByEmailAsync(loginVM.UserLogin);
                if (user != null)
                {
                    var passwordCheck = await userManager.CheckPasswordAsync(user, loginVM.Password);
                    if (passwordCheck)
                    {
                        var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                        if (result.Succeeded)

						{
							if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
							{
								Console.WriteLine("Redirecting to returnUrl: " + returnUrl);
								return LocalRedirect(returnUrl);

							}
							else
							{

								HttpContext.Session.SetString("id", user.Id);


								var checkIfAdmin = await userManager.GetRolesAsync(user);
								if (checkIfAdmin.Contains("Admin"))

									return RedirectToAction("Admin", "Main");

								return RedirectToAction("Index", "Main");
							}



                        }
                    }
                }
            }
            TempData["Error"] = "Wrong Login. Please, try again!";
            return View(loginVM);
        }


        #endregion 

        #region SignUp
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpVM)
        {
            // Step 1: Check ViewModel annotations first ([Required], [EmailAddress], etc.)
            if (!ModelState.IsValid)
            {
                return View(signUpVM);
            }

            // Step 2: Check duplicate username
            var userByUserName = await userManager.FindByNameAsync(signUpVM.UserName);
            if (userByUserName != null)
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View(signUpVM);
            }

            // Step 3: Check duplicate email
            var userByEmail = await userManager.FindByEmailAsync(signUpVM.Email);
            if (userByEmail != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(signUpVM);
            }

            // Step 4: Try to create user
            var newUser = new User()
            {
                UserName = signUpVM.UserName,
                Email = signUpVM.Email,
                FullName = signUpVM.FullName,
            };

            var response = await userManager.CreateAsync(newUser, signUpVM.Password);

            // Step 5: If creation failed, show Identity errors (password complexity, etc.)
            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(signUpVM);
            }

            // Step 6: Assign role
            var roleResult = await userManager.AddToRoleAsync(newUser, UserRoles.User);
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(signUpVM);
            }

            // Step 7: Only redirect if EVERYTHING succeeded
            return RedirectToAction("SignUpCompleted");
        }

        public IActionResult SignUpCompleted()
        {
            return View();
        }

        #endregion
        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Remove("id");
            return RedirectToAction("Index", "Main");
        }
        #endregion
    }
}
