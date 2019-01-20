using Logik;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class SearchController : BaseController
    {
        [Authorize]
        public ActionResult Index(string searchString)
        {
            try
            {
                searchString.Trim();
                if (String.IsNullOrWhiteSpace(searchString))
                {
                    var profiles = db.Profiles.Include(x => x.User).Where(x => x.AllowSearch == true).Select(x => new UserAndProfiles { Profile = x, User = x.User }).ToList();
                    return View(profiles);
                }
                else
                {
                    var profiles = db.Profiles.Include(x => x.User).Where(x => x.AllowSearch == true && x.User.Name.ToLower().Contains(searchString.ToLower())).Select(x => new UserAndProfiles { Profile = x, User = x.User }).ToList();
                    return View(profiles);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("CatchError", "Base");
            }

        }
    }
}