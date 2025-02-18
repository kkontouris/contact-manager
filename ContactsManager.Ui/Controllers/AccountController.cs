using _16CrudExample.Controllers;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Ui.Controllers
{
	//atribute routing
	[Route("[controller]/[action]")]
	//[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
			SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_roleManager = roleManager;
        }

        [HttpGet]
		[Authorize("NotAuthorized")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
        [Authorize("NotAuthorized")]
		//[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			//check for errors
			if(ModelState.IsValid==false)
			{
				ViewBag.Errors=ModelState.Values.SelectMany(temp=>temp.Errors).Select(temp=>temp.ErrorMessage);
				return View(registerDTO);
			}
            //if no errors
            //Storing the users registration details into identity database
            ApplicationUser user = new ApplicationUser()
			{
				Email= registerDTO.Email,
				PhoneNumber=registerDTO.Phone,
				ApplicationUserName=registerDTO.PersonName,
				UserName=registerDTO.Email
			};
			IdentityResult result=await _userManager.CreateAsync(user, registerDTO.Password);
			if (result.Succeeded)
			{
				//if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
				//{

				//	ApplicationRole applicationRole = new ApplicationRole()
				//	{
				//		Name = UserTypeOptions.Admin.ToString()
				//	};
				//	await _roleManager.CreateAsync(applicationRole);
				//}

				//Check if the "USER" role exists, if not, create it
				var userRole = await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString());
				if (userRole == null)
				{
					userRole = new ApplicationRole()
					{
						Name = UserTypeOptions.User.ToString()
					};
					await _roleManager.CreateAsync(userRole);
				}


				//add the new user into admin role
				await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());

                //To Do: Sign In
                await _signInManager.SignInAsync(user, isPersistent: false);
				//ActionName, ControllerName
				return RedirectToAction(nameof(PersonsController.Index), "Persons");
			}
        
			else
			{
				foreach(IdentityError error in  result.Errors)
				{
					ModelState.AddModelError("Register",error.Description);
				}
				return View(registerDTO);
			};	
	}
		[HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
		{
			if (ModelState.IsValid==false)
			{
				ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
				return View(loginDTO);

			}

			var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, 
				isPersistent: false, lockoutOnFailure: false);
			
			if(result.Succeeded)
            { //Admin
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
					else
					{
						return RedirectToAction("Index", "Persons");
					}
                }
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
           

            ModelState.AddModelError("Login", "Invalid username or password");

            return View(loginDTO);
		}
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(PersonsController.Index), "Persons");
		}

		[AllowAnonymous]
		public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
		{
			ApplicationUser user=await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return Json(true); //valid(not already registered)
			}
			else
			{
				return Json(false);   //invalid(already registered)
			}
		}
	}
}
