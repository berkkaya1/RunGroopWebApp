using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ApplicationDbContext _context;
    
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    
    
    public IActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }
        
        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Race");
                }
            }
            TempData["Error"] = "Invalid login attempt";
            return View(loginViewModel);
        }
        TempData["Error"] = "Invalid login attempt";
        return View(loginViewModel);
    }
    public IActionResult Register()
    {
        var response = new RegisterViewModel();
        return View(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerViewModel);
        }
        var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

        if (user != null)
        {
            TempData["Error"] = "User email address already exists";
            return View(registerViewModel);
        }
        var newUser = new AppUser()
        {
            UserName = registerViewModel.EmailAddress,
            Email = registerViewModel.EmailAddress
        };
        var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
        if (newUserResponse.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

        }
       return RedirectToAction("Index", "Race");

    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Race");
    }
        
}