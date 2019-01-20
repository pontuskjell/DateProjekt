using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Logik;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.InformationChanged ? "Your User Information has been Changed"
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }
        [Authorize]
        public ActionResult ChangeProfile()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                return View(profile);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }
            
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(HttpPostedFileBase upload, Profiles model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                profile.Description = model.Description;
                profile.AllowSearch = model.AllowSearch;

                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        profile.Picture = reader.ReadBytes(upload.ContentLength);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("CatchError", "Base");
            }
            

        }
        [Authorize]
        public ActionResult ChangeInformation()
        {
            try
            {
                var list = new List<string>
            {
                "Male",
                "Female"
            };

                var selectListItems = list.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
                var user = GetUser();
                var editInformation = new EditInformation
                {
                    genders = selectListItems,
                    Name = user.Name,
                    City = user.City,
                    Gender = user.Gender

                };
                return View(editInformation);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }
           
        }
        [Authorize]
        public ActionResult InactivateAccount()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                profile.Inactivate = true;
                profile.AllowSearch = false;
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }
            
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeInformation(EditInformation model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var list = new List<string>
            {
                "Male",
                "Female"
            };

                    var selectListItems = list.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
                    model.genders = selectListItems;
                    return View(model);
                }

                var user = GetUser();
                user.Name = model.Name;
                user.City = model.City;
                user.Gender = model.Gender;

                db.SaveChanges();
                return RedirectToAction("ChangeInformation", new { Message = ManageMessageId.InformationChanged });
            }
            catch (Exception)
            {
                return RedirectToAction("CatchError", "Base");
            }
            
        }
        [Authorize]
        public ActionResult SaveProfileData()
        {
            try
            {
                var xmlSerializer = new SaveProfile();
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var sparaProfil = new SaveProfileModel
                {
                    City = user.City,
                    Description = profile.Description,
                    Gender = user.Gender,
                    Name = user.Name,
                    Picture = profile.Picture
                };
                xmlSerializer.CreateXML(user.Name);
                xmlSerializer.Spara(sparaProfil, user.Name);
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {
                return RedirectToAction("CatchError", "Base");
            }
            
            
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
#region Helpers
        // Used for XSRF protection when adding external logins
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            
            ChangePasswordSuccess,           
            SetPasswordSuccess,
            RemoveLoginSuccess,
            InformationChanged,
            Error
        }

#endregion
    }
}