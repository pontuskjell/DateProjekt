using Logik;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
        protected ApplicationUser GetUser()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Single(x => x.Id == userId);
                return user;
            }
            catch (Exception)
            {
                throw new Exception("Något gick fel!");
            }
            
        }

        protected Profiles GetProfileUser(int id)
        {
            try
            {
                var profile = db.Profiles.Single(x => x.Id == id);
                return profile;
            }
            catch (Exception)
            {
                throw new Exception("Något gick fel!");
            }
           
        }

        public ActionResult CatchError()
        {
            return View();
        }
    }
}