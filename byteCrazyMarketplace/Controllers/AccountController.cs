using byteCrazy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;



namespace byteCrazy.Controllers
{
   
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        
        public AccountController()
        {
            _context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // The Authorize action is the endpoint that is called when you access any protected Web API. 
        // If the user is not logged in, they will be redirected to the Login page. 
        //After a successful login, you can call the Web API.

        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Invalid login attempt。");
            return View(model);
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.HometownList = new List<SelectListItem>
    {
        new SelectListItem { Text = "Newcastle", Value = "Newcastle" },
        new SelectListItem { Text = "Hamilton", Value = "Hamilton" },
        new SelectListItem { Text = "Cooks Hill", Value = "Cooks Hill" },
        new SelectListItem { Text = "Merewether", Value = "Merewether" },
        new SelectListItem { Text = "Islington", Value = "Islington" },
        new SelectListItem { Text = "Broadmeadow", Value = "Broadmeadow" },
        new SelectListItem { Text = "Adamstown", Value = "Adamstown" },
        new SelectListItem { Text = "Stockton", Value = "Stockton" },
        new SelectListItem { Text = "Waratah", Value = "Waratah" },
        new SelectListItem { Text = "Charlestown", Value = "Charlestown" },
        new SelectListItem { Text = "Others", Value = "Others" }

    };
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = model.StudentNumber + "@uon.edu.au";
                var existingUser = await UserManager.FindByEmailAsync(email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "This account already exists, please do not register again.");
                    ViewBag.HometownList = GetHometownList();
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Hometown = model.Hometown,
                    PhoneNumber = model.PhoneNumber,
                    Id = model.StudentNumber  // use StudentNumber as fo Id
                };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            ViewBag.HometownList = GetHometownList();
            return View(model);
        }

        // Helper methods to get HometownList
        private List<SelectListItem> GetHometownList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Text = "Newcastle", Value = "Newcastle" },
        new SelectListItem { Text = "Hamilton", Value = "Hamilton" },
        new SelectListItem { Text = "Cooks Hill", Value = "Cooks Hill" },
        new SelectListItem { Text = "Merewether", Value = "Merewether" },
        new SelectListItem { Text = "Islington", Value = "Islington" },
        new SelectListItem { Text = "Broadmeadow", Value = "Broadmeadow" },
        new SelectListItem { Text = "Adamstown", Value = "Adamstown" },
        new SelectListItem { Text = "Stockton", Value = "Stockton" },
        new SelectListItem { Text = "Waratah", Value = "Waratah" },
        new SelectListItem { Text = "Charlestown", Value = "Charlestown" },
        new SelectListItem { Text = "Others", Value = "Others" }
    };
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> UserCenter()
        {
            var userId = User.Identity.GetUserId();

            ViewBag.IsAdmin = await _context.AdminUserList
             .AnyAsync(a => a.userID == userId && a.status == "Active");


            var publishedProductsOnSale = await _context.Product
                .Where(p => p.SellerID == userId && p.Status == "Active")
                .ToListAsync();

            var publishedProductsSold = await _context.Product
                .Where(p => p.SellerID == userId && p.Status == "sold")
                .ToListAsync();

            var purchasedProducts = await _context.Product
                .Where(p => p.BuyerID == userId)
                .ToListAsync();

            var savedProducts = await _context.SavedProducts
                .Where(sp => sp.UserID == userId)
                .Select(sp => sp.Product)
                .ToListAsync();

            var model = new UserCenterViewModel
            {
                PublishedProductsOnSale = publishedProductsOnSale,
                PublishedProductsSold = publishedProductsSold,
                PurchasedProducts = purchasedProducts,
                SavedProducts = savedProducts
            };

            return View(model);
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        //// POST: /Account/ForgotPassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = model.StudentNumber + "@uon.edu.au";
                var user = await UserManager.FindByEmailAsync(email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Input error.  Please check and try again.");
                    return View(model);
                }

                if (user.PhoneNumber != model.PhoneNumber)
                {
                    ModelState.AddModelError("", "Input error. The phone number does not match our records. Please check and try again.");
                    return View(model);
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                
                return RedirectToAction("ResetPassword", new { email = user.Email, code = code });
            }

            // Explanation: There is a problem, please redisplay the form
            return View(model);
        }
     
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string email, string code)
        {
            return code == null ? View("Error") : View(new ResetPasswordViewModel { Email = email, Code = code });
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> DirectResetPassword()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return HttpNotFound();
            }

            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            return RedirectToAction("ResetPassword", new { email = user.Email, code = code });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "No user found with this email address.");
                return View(model);
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View(model);
        }


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

     
 
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }






        #region Helper
        // Used to provide XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
